using AuthDNetLib.Models.Tokens;

namespace AuthDNetLib.Interfaces.Tokens;

/// <summary>
/// Interface para geração e gerenciamento de tokens atrelados a usuários.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Gera um token aleatório com o comprimento especificado.
    /// </summary>
    /// <param name="length">O comprimento do token a ser gerado.</param>
    /// <returns>Um token aleatório do tipo string.</returns>
    string GenerateRandomToken(int length);

    /// <summary>
    /// Verifica se um token quefoi gerado é único.
    /// </summary>
    /// <param name="token">O token a ser verificado.</param>
    /// <returns>True se o token for único, caso contrário, false.</returns>
    Task<bool> IsTokenUniqueAsync(string token);

    /// <summary>
    /// Gera e atribui um token a um usuário específico.
    /// </summary>
    /// <param name="userId">O ID do usuário ao qual o token será atribuído.</param>
    /// <returns>O token gerado e associado ao usuário.</returns>
    Task<Token> SetTokenForUserAsync(string userId);

    /// <summary>
    /// Exclui um token com base no ID.
    /// </summary>
    /// <param name="id">O ID do token a ser excluído.</param>
    Task DeleteTokenAsync(string id);

    /// <summary>
    /// Obtém um token pelo ID.
    /// </summary>
    /// <param name="id">O ID do token a ser recuperado.</param>
    /// <returns>O token correspondente ao ID.</returns>
    Task<Token> GetTokenByIdAsync(string id);
}