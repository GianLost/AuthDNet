using AuthDNetLib.Data;
using AuthDNetLib.Interfaces.Tokens;
using AuthDNetLib.Models.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace AuthDNetLib.Services.Tokens;

public class TokenService(ApplicationDbContext database) : ITokenService
{
    private readonly ApplicationDbContext _database = database ?? throw new ArgumentNullException(nameof(database));

    public string GenerateRandomToken(int length = 64)
    {
        byte[] randomBytes = new byte[length];

        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            rng.GetBytes(randomBytes);

        return Convert.ToBase64String(randomBytes);
    }

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

    public async Task DeleteTokenAsync(string id)
    {
        Token token = await GetTokenByIdAsync(id);

        _database.Tokens.Remove(token);
        await _database.SaveChangesAsync();
    }

    public async Task<bool> IsTokenUniqueAsync(string token)
    {
        return !await _database.Tokens.AnyAsync(t => t.SessionToken == token);
    }

    public async Task<Token> GetTokenByIdAsync(string id)
    {
        Token token = await _database.Tokens.FindAsync(id) ?? throw new ArgumentNullException(nameof(id), "Token não encontrado.");
        return token;
    }
}
