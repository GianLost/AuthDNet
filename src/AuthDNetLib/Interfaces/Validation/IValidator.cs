using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace AuthDNetLib.Interfaces.Validation;

/// <summary>
/// Define os métodos de validação para entidades de usuário.
/// </summary>
/// <typeparam name="T">O tipo de entidade de usuário a ser validada. Deve ser uma classe.</typeparam>
public interface IValidator<T> where T : class
{
    /// <summary>
    /// Dicionário que associa nomes de propriedades a mensagens de erro para validação de nulidade.
    /// A chave do dicionário é o nome da propriedade e o valor é a mensagem de erro correspondente.
    /// </summary>
    Dictionary<string, string> PropertiesOfNullValidation { get; set; }

    /// <summary>
    /// Dicionário que associa nomes de propriedades a mensagens de erro para validação de unicidade.
    /// A chave do dicionário é o nome da propriedade e o valor é a mensagem de erro correspondente.
    /// </summary>
    Dictionary<string, string> ValidationOfUniqueProperties { get; set; }

    /// <summary>
    /// Verifica se as propriedades passadas para um tipo de usuário são nulas.
    /// </summary>
    /// <param name="user">A entidade do usuário a ser verificada.</param>
    /// <param name="modelState">O estado do modelo a ser atualizado com erros de validação.</param>
    /// <returns>Um booleano indicando se alguma propriedade é nula.</returns>
    bool ArePropertiesNull(T user, ModelStateDictionary modelState);

    /// <summary>
    /// Compara propriedades de um tipo de usuário a fim de evitar duplicatas em um cenário de persistência dos dados em um banco de dados.
    /// </summary>
    /// <param name="user">A entidade do usuário a ser verificada.</param>
    /// <param name="modelState">O estado do modelo a ser atualizado com erros de validação.</param>
    /// <param name="dbSet">A tabela que retorna os dados para comparação das propriedades</param>
    /// <returns>Um booleano indicando se a propriedade é única ou não.</returns>
    Task<bool> IsPropertyUniqueAsync(T user, ModelStateDictionary modelState, DbSet<T> dbSet);

    /// <summary>
    /// Verifica se dois emails fornecidos são iguais.
    /// </summary>
    /// <param name="email">O email a ser comparado.</param>
    /// <param name="confirmEmail">O email de confirmação a ser comparado.</param>
    /// <param name="modelState">O estado do modelo a ser atualizado com erros de validação.</param>
    /// <returns>Um booleano indicando se os emails são iguais.</returns>
    bool AreEmailsMatching(string email, string confirmEmail, ModelStateDictionary modelState);

    /// <summary>
    /// Verifica se duas senhas fornecidas são iguais.
    /// </summary>
    /// <param name="password">A senha a ser comparada.</param>
    /// <param name="confirmPassword">A senha de confirmação a ser comparada.</param>
    /// <param name="modelState">O estado do modelo a ser atualizado com erros de validação.</param>
    /// <returns>Um booleano indicando se as senhas são iguais.</returns>
    bool ArePasswordsMatching(string password, string confirmPassword, ModelStateDictionary modelState);

    /// <summary>
    /// Compara uma senha em formato literal com uma hash criptografada utilizando BCrypt para validar a autenticidade da senha.
    /// </summary>
    /// <param name="password">A senha a ser verificada.</param>
    /// <param name="hashedPassword">O hash da senha criptografado para validar a autenticidade.</param>
    /// <param name="modelState">O ModelStateDictionary para adicionar erros de validação.</param>
    /// <returns>Um booleano indicando se a senha é autêntica.</returns>
    bool IsPasswordValid(string password, string hashedPassword);

    /// <summary>
    /// Verifica se uma senha atende aos critérios mínimos de força de senha.
    /// </summary>
    /// <param name="password">A senha a ser verificada.</param>
    /// <param name="modelState">O estado do modelo a ser atualizado com erros de validação.</param>
    /// <returns>Um booleano indicando se a senha atende aos critérios de força.</returns>
    bool IsPasswordStrong(string password, ModelStateDictionary modelState);

    /// <summary>
    /// Verifica se o hash gerado para uma determinada propriedade é único em relação as hashes salvas em um contexto de dados.
    /// </summary>
    /// <param name="user">A entidade do usuário de onde deve se buscar as propriedades.</param>
    /// <param name="modelState">O estado do modelo a ser atualizado com erros de validação.</param>
    /// <param name="propertyName">A propriedade a ser verificada para unicidade do hash.</param>
    /// <param name="dbSet">A tabela que retorna os dados para comparação das propriedades.</param>
    /// <returns>Um booleano indicando se o hash é único.</returns>
    Task<bool> IsHashUniqueAsync(T user, ModelStateDictionary modelState, string propertyName, DbSet<T> dbSet);

    /// <summary>
    /// Garante a geração de uma hash única para a propriedade de usuário passada por parâmetro.
    /// </summary>
    /// <param name="user">A entidade do usuário para a qual a hash deve ser gerada.</param>
    /// <param name="propertyValue">O valor da propriedade para o qual a hash deve ser gerada.</param>
    /// <param name="propertyName">O nome da propriedade para a qual a hash deve ser gerada.</param>
    /// <param name="modelState">O estado do modelo a ser atualizado com erros de validação.</param>
    /// <param name="dbSet">A tabela que retorna os dados para comparação das propriedades.</param>
    /// <returns>Um booleano indicando se a hash é única.</returns>
    Task<bool> GuaranteeUniqueHashAsync(T user, string propertyName, string? propertyValue, ModelStateDictionary modelState, DbSet<T> dbSet);
}