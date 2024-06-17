using AuthDNetLib.Helper.Expressions;
using AuthDNetLib.Helper.Messages;
using AuthDNetLib.Interfaces.Crypt;
using AuthDNetLib.Interfaces.Validation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AuthDNetLib.Helper.Validation;

/// <summary>
/// Implementação dos métodos de validação para entidades de usuário.
/// </summary>
/// <typeparam name="T">O tipo de entidade de usuário a ser validada. Deve ser uma classe.</typeparam>
public partial class UserValidator<T> : IValidator<T> where T : class
{
    /// <summary>
    /// Interface para utilizar serviços de criptografia.
    /// </summary>
    private readonly ICryptography _crypt;

    /// <summary>
    /// Dicionário que associa nomes de propriedades a mensagens de erro para validação de nulidade.
    /// A chave do dicionário é o nome da propriedade e o valor é a mensagem de erro correspondente.
    /// </summary>
    public Dictionary<string, string> PropertiesOfNullValidation { get; set; }

    /// <summary>
    /// Dicionário que associa nomes de propriedades a mensagens de erro para validação de unicidade.
    /// A chave do dicionário é o nome da propriedade e o valor é a mensagem de erro correspondente.
    /// </summary>
    public Dictionary<string, string> ValidationOfUniqueProperties { get; set; }

    /// <summary>
    /// Inicializa uma nova instância de UserValidator com os serviços de criptografia e os dicionários opcionais para validação.
    /// </summary>
    /// <param name="crypt">O serviço de criptografia a ser injetado.</param>
    /// <param name="propertiesOfNullValidation">Dicionário para validação de propriedades nulas. Se não fornecido, são usados valores padrão.</param>
    /// <param name="validationOfUniqueProperties">Dicionário para validação de propriedades únicas. Se não fornecido, são usados valores padrão.</param>
    public UserValidator(ICryptography crypt, Dictionary<string, string>? propertiesOfNullValidation = null, Dictionary<string, string>? validationOfUniqueProperties = null)
    {

        _crypt = crypt ?? throw new ArgumentNullException(nameof(crypt));

        PropertiesOfNullValidation = propertiesOfNullValidation ?? new Dictionary<string, string>
        {
            { "Id", ErrorMessages.MsgIdRequired },
            { "Name", ErrorMessages.MsgNameRequired },
            { "Login", ErrorMessages.MsgLoginRequired },
            { "Password", ErrorMessages.MsgPasswordRequired },
            { "ConfirmPassword", ErrorMessages.MsgConfirmPasswordRequired },
            { "Email", ErrorMessages.MsgEmailRequired },
            { "ConfirmEmail", ErrorMessages.MsgConfirmEmailRequired },
            { "CellPhone", ErrorMessages.MsgPhoneRequired },
            { "Workplace", ErrorMessages.MsgWorkplaceRequired },
            { "RegisterDate", ErrorMessages.MsgRegisterDateRequired },
        };

        ValidationOfUniqueProperties = validationOfUniqueProperties ?? new Dictionary<string, string>
        {
            { "Id", ErrorMessages.MsgDuplicatedId },
            { "Name", ErrorMessages.MsgDuplicatedName },
            { "Login", ErrorMessages.MsgDuplicatedLogin },
            { "Email", ErrorMessages.MsgDuplicatedEmail },
            { "CellPhone", ErrorMessages.MsgDuplicatedPhone },
            { "AuthToken", ErrorMessages.MsgDuplicateSessionToken }
        };

    }

    /// <summary>
    /// Verifica se as propriedades obrigatórias do usuário são nulas ou vazias.
    /// </summary>
    /// <param name="user">O objeto de usuário a ser validado.</param>
    /// <param name="modelState">O ModelStateDictionary para adicionar erros de validação.</param>
    /// <returns>True se houver erros, caso contrário, false.</returns>
    public virtual bool ArePropertiesNull(T user, ModelStateDictionary modelState)
    {
        bool hasErrors = false;

        foreach (var property in PropertiesOfNullValidation)
        {
            PropertyInfo? propertyInfo = typeof(T).GetProperty(property.Key) ?? throw new ArgumentException($"A propriedade '{property.Key}' não existe no tipo '{typeof(T).Name}'.");

            object? propertyValue = propertyInfo.GetValue(user);

            if (propertyValue == null || string.IsNullOrWhiteSpace(propertyValue.ToString()))
            {
                modelState.AddModelError(property.Key, property.Value);
                hasErrors = true;
            }
        }

        return hasErrors;
    }

    /// <summary>
    /// Verifica se uma propriedade do usuário é única no banco de dados.
    /// </summary>
    /// <param name="user">O objeto de usuário a ser validado.</param>
    /// <param name="modelState">O ModelStateDictionary para adicionar erros de validação.</param>
    /// <param name="dbSet">O DbSet para consultar no banco de dados.</param>
    /// <returns>True se houver erros, caso contrário, false.</returns>
    public virtual async Task<bool> IsPropertyUniqueAsync(T user, ModelStateDictionary modelState, DbSet<T> dbSet)
    {
        bool hasErrors = false;

        foreach (var property in ValidationOfUniqueProperties)
        {
            PropertyInfo? propertyInfo = typeof(T).GetProperty(property.Key) ?? throw new ArgumentException($"A propriedade '{property.Key}' não existe no tipo '{typeof(T).Name}'.");

            object? propertyValue = propertyInfo.GetValue(user);

            Expression<Func<T, bool>> propertyFilter = CreatePropertyFilter(user, propertyInfo);

            bool propertyExists = await dbSet.Where(propertyFilter).AnyAsync();

            if (propertyExists)
            {
                modelState.AddModelError(property.Key, property.Value);
                hasErrors = true;
            }
        }

        return hasErrors;
    }

    /// <summary>
    /// Verifica se os emails fornecidos são iguais.
    /// </summary>
    /// <param name="email">O email a ser comparado.</param>
    /// <param name="confirmEmail">O email de confirmação a ser comparado.</param>
    /// <param name="modelState">O ModelStateDictionary para adicionar erros de validação.</param>
    /// <returns>True se os emails coincidirem, caso contrário, false.</returns>
    public virtual bool AreEmailsMatching(string email, string confirmEmail, ModelStateDictionary modelState)
    {
        if (email != confirmEmail)
        {
            modelState.AddModelError("ConfirmEmail", ErrorMessages.MsgEmailsUnmatching);
            return false;
        }

        return true;
    }

    /// <summary>
    /// Verifica se a senha atende aos critérios de segurança.
    /// </summary>
    /// <param name="password">A senha a ser verificada.</param>
    /// <param name="modelState">O ModelStateDictionary para adicionar erros de validação.</param>
    /// <returns>True se a senha atender aos critérios, caso contrário, false.</returns>
    public virtual bool IsPasswordStrong(string password, ModelStateDictionary modelState)
    {
        if (!MyRegex().IsMatch(password) || password.Length < 8)
        {
            modelState.AddModelError("Password", ErrorMessages.MsgPasswordNotStrong);
            return false;
        }

        return true;
    }

    /// <summary>
    /// Verifica se as senhas fornecidas são iguais.
    /// </summary>
    /// <param name="password">A senha a ser comparada.</param>
    /// <param name="confirmPassword">A senha de confirmação a ser comparada.</param>
    /// <param name="modelState">O ModelStateDictionary para adicionar erros de validação.</param>
    /// <returns>True se as senhas coincidirem, caso contrário, false.</returns>
    public virtual bool ArePasswordsMatching(string password, string confirmPassword, ModelStateDictionary modelState)
    {
        if (password != confirmPassword)
        {
            modelState.AddModelError("ConfirmPassword", ErrorMessages.MsgPasswordsUnmatching);
            return false;
        }

        return true;
    }

    /// <summary>
    /// Verifica se a senha digitada para fins de autenticação corresponde ao has criptografado gerado no moemento do cadastro da senha.
    /// </summary>
    /// <param name="password">A senha a ser verificada.</param>
    /// <param name="hashedPassword">O hash da senha criptografado para validar a autenticidade.</param>
    /// <param name="modelState">O ModelStateDictionary para adicionar erros de validação.</param>
    /// <returns>True se a senha corresponder ao hash, caso contrário, false.</returns>
    public virtual bool IsPasswordValid(string password, string hashedPassword)
    {
        if (!_crypt.VerifyEncryptedKey(password, hashedPassword))
            return false;

        return true;
    }

    /// <summary>
    /// Verifica se um hash gerado para uma propriedade é único no banco de dados.
    /// </summary>
    /// <param name="user">O objeto de usuário a ser validado.</param>
    /// <param name="modelState">O ModelStateDictionary para adicionar erros de validação.</param>
    /// <param name="propertyName">O nome da propriedade para a qual o hash é gerado.</param>
    /// <param name="dbSet">O DbSet para consultar no banco de dados.</param>
    /// <returns>True se houver erros, caso contrário, false.</returns>
    public virtual async Task<bool> IsHashUniqueAsync(T user, ModelStateDictionary modelState, string propertyName, DbSet<T> dbSet)
    {
        bool hasErrors = false;

        PropertyInfo? propertyInfo = typeof(T).GetProperty(propertyName) ?? throw new ArgumentException($"A propriedade '{propertyName}' não existe no tipo '{typeof(T).Name}'.");

        object? propertyValue = propertyInfo.GetValue(user);

        if (propertyValue != null)
        {
            Expression<Func<T, bool>> propertyFilter = CreatePropertyFilter(user, propertyInfo);

            bool propertyExists = await dbSet.Where(propertyFilter).AnyAsync();

            if (propertyExists)
            {
                modelState.AddModelError(propertyInfo.Name, $"O hash gerado para '{propertyInfo.Name}' não pode ser utilizado.");
                hasErrors = true;
            }
        }

        return hasErrors;
    }

    /// <summary>
    /// Garante a geração de uma hash única para a propriedade de usuário passada por parâmetro.
    /// </summary>
    /// <param name="user">A entidade do usuário para a qual a hash deve ser gerada.</param>
    /// <param name="propertyValue">O valor da propriedade para o qual a hash deve ser gerada.</param>
    /// <param name="propertyName">O nome da propriedade para a qual a hash deve ser gerada.</param>
    /// <param name="modelState">O estado do modelo a ser atualizado com erros de validação.</param>
    /// <param name="dbSet">A tabela que retorna os dados para comparação das propriedades.</param>
    /// <returns>Um booleano indicando se a hash é única.</returns>
    public virtual async Task<bool> GuaranteeUniqueHashAsync(T user, string propertyName, string? propertyValue, ModelStateDictionary modelState, DbSet<T> dbSet)
    {
        // Verifica se o valor da propriedade é nulo ou vazio
        if (string.IsNullOrEmpty(propertyValue))
            throw new ArgumentNullException(nameof(propertyValue));

        bool hasErrors = true;

        // Obtém a propriedade correspondente ao nome fornecido
        PropertyInfo propertyInfo = typeof(T).GetProperty(propertyName) ?? throw new ArgumentException($"A propriedade '{propertyName}' não existe no tipo '{typeof(T).Name}'.");

        // Define o número máximo de tentativas de geração de hash
        const int maxAttempts = 3;
        int attempts = 0;

        // Loop que define uma interação limitadaà espera de se obter uma hash única
        while (hasErrors && attempts < maxAttempts)
        {
            // Define o valor da propriedade como a hash gerada
            propertyInfo.SetValue(user, _crypt.EncryptKey(propertyValue));

            hasErrors = await IsHashUniqueAsync(user, modelState, propertyName, dbSet);

            // Verifica se a hash gerada é única no banco de dados
            if (hasErrors)
            {
                attempts++;
                if (attempts >= maxAttempts)
                {
                    modelState.AddModelError(propertyInfo.Name, $"Não foi possível gerar a hash corretamente para a propriedade '{propertyInfo.Name}'.");
                }
            }

        }

        return !hasErrors;
    }

    /// <summary>
    /// Método privado utilizado para montar filtros com propriedades para realizar consultas em um banco de dados
    /// </summary>
    /// <param name="user">O objeto de usuário que fornece as propriedades.</param>
    /// <param name="propertyInfo">A propriedade utilizada para gerar o filtro para se utilizar em uma consulta.</param>
    private static Expression<Func<T, bool>> CreatePropertyFilter(T user, PropertyInfo propertyInfo)
    {
        ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
        MemberExpression property = Expression.Property(parameter, propertyInfo);
        ConstantExpression value = Expression.Constant(propertyInfo.GetValue(user));
        BinaryExpression equals = Expression.Equal(property, value);

        return Expression.Lambda<Func<T, bool>>(equals, parameter);
    }

    [GeneratedRegex(ConstExpressions.StrongPasswordRegex)]
    private static partial Regex MyRegex();
}