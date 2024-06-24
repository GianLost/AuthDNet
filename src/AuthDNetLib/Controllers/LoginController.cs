using AuthDNetLib.Helper.Transfer.Data;
using AuthDNetLib.Interfaces.Users.Session;
using AuthDNetLib.Models.Login;
using AuthDNetLib.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AuthDNetLib.Controllers;

[Area("Auth")]
public class LoginController(ISessionMenager<TUser> sessionMenager) : Controller
{
    private readonly ISessionMenager<TUser> _sessionMenager = sessionMenager ?? throw new InvalidOperationException(nameof(sessionMenager));

    [HttpGet]
    public async Task<IActionResult> SignIn()
    {
        if (await _sessionMenager.GetSessionAsync() != null)
            return RedirectToAction("Index", "Home", new { area = "" });

        return View(new LoginModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignIn(LoginModel login, string? userEncrypted = null)
    {
        try
        {
            if (!string.IsNullOrEmpty(userEncrypted))
            {
                // Desserializa o userEncrypted para um objeto LoginModel
                login = await JSONDataTransfer<LoginModel>.JSONSecureDataDesserialize(userEncrypted);

                // Atualiza as propriedades do login com os valores desserializados
                foreach (var prop in typeof(LoginModel).GetProperties())
                {
                    var value = prop.GetValue(login);
                    prop.SetValue(login, value);

                    // Atualiza o ModelState com os novos valores
                    ModelState.Remove(prop.Name);
                    ModelState.SetModelValue(prop.Name, new ValueProviderResult(value?.ToString() ?? string.Empty));
                }

                // Remova a entrada userEncrypted do ModelState
                ModelState.Remove(nameof(userEncrypted));

                // Revalida o modelo após a atualização
                TryValidateModel(login);
            }

            if (ModelState.IsValid)
            {
                var user = await _sessionMenager.SignInAsync(login.LoginName, login.Password);

                string? token = await _sessionMenager.GenerateJwtTokenAsync(user);

                Response.Cookies.Append("jwt", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax
                });

                return RedirectToAction("Index", "Home", new { area = "" });
            }

            return View(login);

        }
        catch (UnauthorizedAccessException ex)
        {
            TempData["UnauthorizedAccess"] = ex.Message;
            ModelState.AddModelError("", string.Empty);
            return View(login);
        }
        catch (Exception)
        {
            return View(login);
        }
    }

    public IActionResult Logout()
    {
        _sessionMenager.SignOut();
        Response.Cookies.Delete("jwt");
        return RedirectToAction("Index", "Home", new { area = "" });
    }
}