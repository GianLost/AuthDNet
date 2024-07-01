using AuthDNetLib.Data;
using AuthDNetLib.Helper.Messages;
using AuthDNetLib.Helper.Transfer.Data;
using AuthDNetLib.Interfaces.Users;
using AuthDNetLib.Interfaces.Users.Session;
using AuthDNetLib.Interfaces.Validation;
using AuthDNetLib.Keys.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace AuthDNetLib.Helper.Session;

/// <summary>
/// Implementação do gerenciador de sessões de usuários.
/// </summary>
/// <remarks>
/// Inicializa uma nova instância da classe <see cref="SessionMenager{T}"/>.
/// </remarks>
/// <param name="userService">O serviço de usuário.</param>
/// <param name="validator">O validador de usuários.</param>
/// <param name="httpContextAccessor">Acessao do contexto HTTP.</param>
/// <param name="database">O contexto do banco de dados.</param>
/// <exception cref="ArgumentNullException">Lançado se qualquer um dos parâmetros for nulo.</exception>
/// <exception cref="InvalidOperationException">Lançado em casos de opeções inválidas.</exception>
public class SessionMenager<T>(IUserService<T> userService, IValidator<T> validator, IHttpContextAccessor httpContextAccessor, ApplicationDbContext database) : ISessionMenager<T> where T : class
{
    private readonly IUserService<T> _userService = userService ?? throw new InvalidOperationException(nameof(userService));

    private readonly IValidator<T> _validator = validator ?? throw new InvalidOperationException(nameof(validator));

    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor ?? throw new InvalidOperationException(nameof(httpContextAccessor));

    private readonly ApplicationDbContext _database = database ?? throw new ArgumentNullException(nameof(database));

    private readonly IConfiguration _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build() ?? throw new ArgumentNullException(nameof(_configuration));

    /// <summary>
    /// Obtém a sessão atual do usuário.
    /// </summary>
    /// <returns>A sessão atual do usuário ou null se não houver sessão.</returns>
    public async Task<T?> GetSessionAsync()
    {
        HttpContext? httpContext = _httpContextAccessor.HttpContext ?? throw new InvalidOperationException(ErrorMessages.MsgNullHttpContext);

        ISession session = httpContext.Session;

        string? userData = session.GetString(SessionString.SessionConnectString);

        if (string.IsNullOrEmpty(userData))
            return null;

        try
        {
            return await JSONDataTransfer<T>.JSONSecureDataDesserialize(userData) ?? throw new JsonException(ErrorMessages.MsgJSONDesserializeError);
        }
        catch (InvalidOperationException)
        {
            return null;
        }
        catch (JsonException)
        {
            return null;
        }
    }

    /// <summary>
    /// Realiza o login do usuário e configura a sessão.
    /// </summary>
    /// <param name="login">O nome de login do usuário.</param>
    /// <param name="password">A senha do usuário.</param>
    /// <returns>O usuário autenticado.</returns>
    /// <exception cref="UnauthorizedAccessException">Lançado em que casos onde o usuário não obtém permissão para ser autenticado.</exception>
    public async Task<T> SignInAsync(string login, string password)
    {
        // Obter a propriedade 'Login' usando reflexão
        T? user = await _database.Set<T>().FirstOrDefaultAsync(u => EF.Property<string>(u, "Login") == login);

        if (user == null)
        {
            LogFailedAttempt(login);
            throw new UnauthorizedAccessException(ErrorMessages.MsgUnauthorizedAccess);
        }

        dynamic userEntity = user;

        if (userEntity.IsLockedOut && userEntity.LockoutEnd > DateTime.UtcNow)
            throw new UnauthorizedAccessException(ErrorMessages.MsgLockedAccount);

        // Reflexão para obter a propriedade 'Password'
        PropertyInfo passwordProperty = user.GetType().GetProperty("Password") ?? throw new InvalidOperationException(ErrorMessages.MsgPasswordNotExist);

        // Valor da propriedade 'Password'
        string? hashedPassword = (string?)passwordProperty.GetValue(user);

        // Verifica se a senha está correta utilizando a verificação do BCrypt.
        if (hashedPassword == null || !_validator.IsPasswordValid(password, hashedPassword))
        {
            LogFailedAttempt(login);
            throw new UnauthorizedAccessException(ErrorMessages.MsgUnauthorizedAccess);
        }

        // Resetar o contador de tentativas falhas em caso de sucesso no login
        await ResetFailedAttempts(userEntity);

        // Configurar a sessão
        ConfigureSession(user);

        return user;
    }

    /// <summary>
    /// Registra uma falha na tentativa de login.
    /// </summary>
    /// <param name="login">O nome de login do usuário.</param>
    public void LogFailedAttempt(string login)
    {
        // Buscar o usuário pelo nome de login
        T? user = _database.Set<T>().FirstOrDefault(u => EF.Property<string>(u, "Login") == login);

        if (user != null)
        {
            dynamic userEntity = user;

            // Incrementar contador de tentativas falhas e registrar o horário da tentativa
            userEntity.FailedAttempts++;
            userEntity.LastFailedAttempt = DateTime.UtcNow;

            // Verificar se o número de tentativas falhas excedeu o limite permitido
            if (userEntity.FailedAttempts >= 3)
            {
                userEntity.IsLockedOut = true;
                userEntity.LockoutEnd = DateTime.UtcNow.AddMinutes(3); // Tempo de bloqueio
            }

            // Atualizar o usuário no banco de dados
            _database.Update(user);
            _database.SaveChanges();
        }
    }

    /// <summary>
    /// Reseta o contador de tentativas falhas de login.
    /// </summary>
    /// <param name="userEntity">A entidade do usuário.</param>
    /// <returns>A entidade do usuário atualizada.</returns>
    public async Task<dynamic> ResetFailedAttempts(dynamic userEntity)
    {
        userEntity.FailedAttempts = 0;
        userEntity.IsLockedOut = false;
        userEntity.LockoutEnd = null;
        return await _userService.UpdateUserAsync(userEntity);
    }

    /// <summary>
    /// Configura a sessão com os dados do usuário autenticado.
    /// </summary>
    /// <param name="user">O usuário autenticado.</param>
    public void ConfigureSession(T user)
    {
        HttpContext? httpContext = _httpContextAccessor.HttpContext ?? throw new InvalidOperationException(ErrorMessages.MsgNullHttpContext);

        ISession session = httpContext.Session;

        try
        {
            string? userData = JSONDataTransfer<T>.JSONSecureDataSerialize(user) ?? throw new SerializationException(ErrorMessages.MsgJSONSerializeError); ;
            session.SetString(SessionString.SessionConnectString, userData);
        }
        catch (JsonException)
        {
            throw;
        }
    }

    /// <summary>
    /// Gera um token JWT para o usuário autenticado.
    /// </summary>
    /// <param name="user">O usuário autenticado.</param>
    /// <returns>Um token JWT.</returns>
    /// <exception cref="InvalidOperationException">Lançado se a chave de segurança JWT não estiver configurada.</exception>
    /// <exception cref="SecurityTokenValidationException">Lançado se o token JWT gerado for inválido.</exception>
    public async Task<string> GenerateJwtTokenAsync(T user)
    {
        string? secret = _configuration["JwtConfig:Secret"] ?? throw new InvalidOperationException(ErrorMessages.MsgJWTSecureKeyNotConfigured);

        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(secret));

        SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

        PropertyInfo? loginProperty = typeof(T).GetProperty("Login") ?? throw new InvalidOperationException(ErrorMessages.MsgLoginNotExist);

        object? loginValue = loginProperty.GetValue(user) ?? throw new InvalidOperationException(ErrorMessages.MsgLoginIsNull);

        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Sub, (string)loginValue),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ];

        JwtSecurityToken token = new(
            issuer: _configuration["JwtConfig:Issuer"],
            audience: _configuration["JwtConfig:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials
        );

        // Convertendo o token JWT em uma string
        string? jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

        // Validar token
        bool isValidToken = await ValidateJwtTokenAsync(jwtToken);

        if (!isValidToken)
            throw new SecurityTokenValidationException(ErrorMessages.MsgJWTTokenInvalid);

        return jwtToken;
    }

    /// <summary>
    /// Valida um token JWT.
    /// </summary>
    /// <param name="token">O token JWT a ser validado.</param>
    /// <returns>True se o token for válido, caso contrário, false.</returns>
    /// <exception cref="InvalidOperationException">Lançado se a chave de segurança JWT não estiver configurada.</exception>
    public async Task<bool> ValidateJwtTokenAsync(string token)
    {
        JwtSecurityTokenHandler tokenHandler = new();

        string? secret = _configuration["JwtConfig:Secret"] ?? throw new InvalidOperationException(ErrorMessages.MsgJWTSecureKeyNotConfigured);

        byte[] key = Encoding.ASCII.GetBytes(secret);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _configuration["JwtConfig:Issuer"],
                ValidAudience = _configuration["JwtConfig:Audience"],
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return await Task.FromResult(true);
        }
        catch
        {
            return await Task.FromResult(false);
        }
    }

    /// <summary>
    /// Encerra a sessão do usuário.
    /// </summary>
    /// <exception cref="InvalidOperationException">Lançado se não houver sessão ativa.</exception>
    public void SignOut()
    {
        HttpContext httpContext = _httpContextAccessor.HttpContext ?? throw new InvalidOperationException(ErrorMessages.MsgNullHttpContext);

        ISession session = httpContext.Session;

        if (string.IsNullOrEmpty(session.GetString(SessionString.SessionConnectString)))
            throw new InvalidOperationException(ErrorMessages.MsgActiveSessionNotFound);

        try
        {
            session.Remove(SessionString.SessionConnectString);
        }
        catch(InvalidOperationException)
        {
            throw;
        }
    }
}