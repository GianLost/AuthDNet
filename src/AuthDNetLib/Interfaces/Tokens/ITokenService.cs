using AuthDNetLib.Models.Tokens;
using AuthDNetLib.Models.Users;

namespace AuthDNetLib.Interfaces.Tokens;

public interface ITokenService
{
    string GenerateRandomToken(int length);
    Task<bool> IsTokenUniqueAsync(string token);
    Task<Token> SetTokenForUserAsync(string userId);
    Task DeleteTokenAsync(string id);
    Task<Token> GetTokenByIdAsync(string id);
}
