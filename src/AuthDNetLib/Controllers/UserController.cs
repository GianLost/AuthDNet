using AuthDNetLib.Data;
using AuthDNetLib.Helper.Transfer.Data;
using AuthDNetLib.Interfaces.Tokens;
using AuthDNetLib.Interfaces.Users;
using AuthDNetLib.Interfaces.Validation;
using AuthDNetLib.Models.Tokens;
using AuthDNetLib.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AuthDNetLib.Controllers;

[Area("Auth")]
public class UserController(ApplicationDbContext database, IUserService<TUser> userService, ITokenService token, IValidator<TUser> validation) : Controller
{
    private readonly ApplicationDbContext _database = database ?? throw new ArgumentNullException(nameof(database));
    private readonly IUserService<TUser> _userService = userService ?? throw new InvalidOperationException(nameof(userService));
    private readonly ITokenService _token = token ?? throw new InvalidOperationException(nameof(token));
    private readonly IValidator<TUser> _validation = validation ?? throw new InvalidOperationException(nameof(validation));

    [HttpGet]
    public IActionResult Profile() => View();

    [HttpGet]
    public IActionResult Register() => View(new RegisterModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterModel user, string? userEncrypted = null)
    {
        try
        {
            if (!string.IsNullOrEmpty(userEncrypted))
            {
                // Desserializa o userEncrypted para um objeto TUser
                user = await JSONDataTransfer<RegisterModel>.JSONSecureDataDesserialize(userEncrypted);

                // Atualiza as propriedades do login com os valores desserializados
                foreach (var prop in typeof(RegisterModel).GetProperties())
                {
                    var value = prop.GetValue(user);
                    prop.SetValue(user, value);

                    // Atualiza o ModelState com os novos valores
                    ModelState.Remove(prop.Name);
                    ModelState.SetModelValue(prop.Name, new ValueProviderResult(value?.ToString() ?? string.Empty));
                }

                // Remova a entrada userEncrypted do ModelState
                ModelState.Remove(nameof(userEncrypted));

                // Revalida o modelo após a atualização
                TryValidateModel(user);
            }

            if (ModelState.IsValid)
            {
                // Método auxiliar que realiza as validações das propriedades de usuário.
                if (!await FullUserValidationAsync(user))
                    return View(user);

                // Garante que a senha possua uma hash única
                if (!await _validation.GuaranteeUniqueHashAsync(user, "Password", user.ConfirmPassword, ModelState, _database.Users))
                    return View(user);

                // Adiciona o usuário ao banco de dados utilizando a classe de serviço de usuários.
                await _userService.CreateUserAsync(user);

                // Gera um novo token para o usuário relacionando-o pelo id do usuário.
                Token newToken = await _token.SetTokenForUserAsync(user.Id ?? throw new ArgumentNullException(nameof(user)));

                // Garante que o token possua uma hash única
                if (!await _validation.GuaranteeUniqueHashAsync(user, "AuthToken", newToken.SessionToken, ModelState, _database.Users))
                {
                    if (newToken.Id == null)
                        throw new ArgumentNullException(nameof(user));

                    // Exclu o token caso a validação de hash unica falhe.
                    await _token.DeleteTokenAsync(newToken.Id);

                    return View(user);
                }

                await _userService.UpdateUserAsync(user);
                return RedirectToAction("SignIn", "Login");
            }

            return View(user);
        }
        catch (Exception)
        {
            return View(user);
        }
    }

    /// <summary>
    /// Método auxiliar da controller de usuários que centraliza a validação de propriedades.
    /// </summary>
    /// <param name="user">Objeto do usuário a ser validado.</param>
    /// <returns>Retorna true se todas as validações forem bem-sucedidas; caso contrário, false.</returns>
    /// <exception cref="ArgumentNullException">Lançada se qualquer campo obrigatório for nulo ou vazio.</exception>
    private async Task<bool> FullUserValidationAsync(RegisterModel user)
    {
        if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.ConfirmEmail) || string.IsNullOrEmpty(user.ConfirmPassword))
            throw new ArgumentNullException(nameof(user));

        // variável de controle do estado das validações
        bool isValid = true;

        try
        {
            isValid &= !_validation.ArePropertiesNull(user, ModelState);
            isValid &= !await _validation.IsPropertyUniqueAsync(user, ModelState, _database.Users);
            isValid &= _validation.AreEmailsMatching(user.Email, user.ConfirmEmail, ModelState);
            isValid &= _validation.IsPasswordStrong(user.Password, ModelState);
            isValid &= _validation.ArePasswordsMatching(user.Password, user.ConfirmPassword, ModelState);

            return isValid;
        }
        catch (ArgumentNullException)
        {
            isValid = false;
        }

        return isValid;
    }
}