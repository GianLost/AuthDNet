using AuthDNetLib.Data;
using AuthDNetLib.Interfaces.Tokens;
using AuthDNetLib.Models.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace AuthDNetLib.Services.Tokens;

/// <summary>
/// Serviço para geração e gerenciamento de tokens atrelados a usuários.
/// </summary>
/// <remarks>
/// Inicializa uma nova instância da classe <see cref="TokenService"/>.
/// </remarks>
/// <param name="database">O contexto do banco de dados utilizado.</param>
/// <exception cref="ArgumentNullException">Lançado se o contexto do banco de dados for nulo.</exception>
public class TokenService(ApplicationDbContext database) : ITokenService
{
    private readonly ApplicationDbContext _database = database ?? throw new ArgumentNullException(nameof(database));

    /// <summary>
    /// Gera um token aleatório com o comprimento especificado.
    /// </summary>
    /// <param name="length">O comprimento do token a ser gerado. O valor padrão é 64.</param>
    /// <returns>Um token aleatório como uma string.</returns>
    public string GenerateRandomToken(int length = 64)
    {
        byte[] randomBytes = new byte[length];

        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            rng.GetBytes(randomBytes);

        return Convert.ToBase64String(randomBytes);
    }

    /// <summary>
    /// Verifica se um token é único.
    /// </summary>
    /// <param name="token">O token a ser verificado.</param>
    /// <returns>True se o token for único, caso contrário, false.</returns>
    public async Task<bool> IsTokenUniqueAsync(string token)
    {
        return !await _database.Tokens.AnyAsync(t => t.SessionToken == token);
    }

    /// <summary>
    /// Gera e atribui um token a um usuário específico.
    /// </summary>
    /// <param name="userId">O ID do usuário ao qual o token será atribuído.</param>
    /// <returns>O token gerado e associado ao usuário.</returns>
    /// <exception cref="Exception">Lançado se não for possível gerar um token único após várias tentativas.</exception>
    public async Task<Token> SetTokenForUserAsync(string userId)
    {
        string token;
        int attempts = 0;
        const int maxAttempts = 5;

        do
        {
            if (attempts >= maxAttempts)
                throw new Exception("Não foi possível gerar um token único após várias tentativas.");

            token = GenerateRandomToken();
            attempts++;
        } while (!await IsTokenUniqueAsync(token));

        Token newToken = new()
        {
            SessionToken = token,
            UserId = userId
        };

        await _database.Tokens.AddAsync(newToken);
        await _database.SaveChangesAsync();

        return newToken;
    }

    /// <summary>
    /// Exclui um token com base no ID.
    /// </summary>
    /// <param name="id">O ID do token a ser excluído.</param>
    public async Task DeleteTokenAsync(string id)
    {
        Token token = await GetTokenByIdAsync(id);

        _database.Tokens.Remove(token);
        await _database.SaveChangesAsync();
    }

    /// <summary>
    /// Obtém um token pelo ID.
    /// </summary>
    /// <param name="id">O ID do token a ser recuperado.</param>
    /// <returns>O token correspondente ao ID.</returns>
    /// <exception cref="ArgumentNullException">Lançado se o token não for encontrado.</exception>
    public async Task<Token> GetTokenByIdAsync(string id)
    {
        Token token = await _database.Tokens.FindAsync(id) ?? throw new ArgumentNullException(nameof(id), "Token não encontrado.");
        return token;
    }
}
