namespace AuthDNetLib.Interfaces.Users.Session;

/// <summary>
/// Interface para gerenciar sessões de usuário.
/// </summary>
/// <typeparam name="T">O tipo de usuário.</typeparam>
public interface ISessionMenager<T> where T : class
{
    /// <summary>
    /// Obtém a sessão atual do usuário.
    /// </summary>
    /// <returns>A sessão atual do usuário ou null se não houver sessão.</returns>
    Task<T?> GetSessionAsync();

    /// <summary>
    /// Realiza o login do usuário e configura a sessão.
    /// </summary>
    /// <param name="login">O login do usuário.</param>
    /// <param name="password">A senha do usuário.</param>
    /// <returns>O usuário autenticado.</returns>
    Task<T> SignInAsync(string login, string password);

    /// <summary>
    /// Registra as falhas em tentativas de login mal sucedidas.
    /// </summary>
    /// <param name="login">O nome de login do usuário.</param>
    void LogFailedAttempt(string login);

    /// <summary>
    /// Reseta o contador de tentativas falhas de login.
    /// </summary>
    /// <param name="userEntity">A entidade do usuário.</param>
    /// <returns>A entidade do usuário atualizada.</returns>
    Task<dynamic> ResetFailedAttempts(dynamic userEntity);

    /// <summary>
    /// Configura a sessão com os dados do usuário autenticado.
    /// </summary>
    /// <param name="user">O usuário autenticado.</param>
    void ConfigureSession(T user);

    /// <summary>
    /// Gera um token JWT para o usuário autenticado.
    /// </summary>
    /// <param name="user">O usuário autenticado.</param>
    /// <returns>Um token JWT.</returns>
    Task<string> GenerateJwtTokenAsync(T user);

    /// <summary>
    /// Valida um token JWT.
    /// </summary>
    /// <param name="token">O token JWT a ser validado.</param>
    /// <returns>True se o token for válido, caso contrário, false.</returns>
    Task<bool> ValidateJwtTokenAsync(string token);

    /// <summary>
    /// Encerra a sessão do usuário removendo os tokens, cookies e dados armazenados na sessão.
    /// </summary>
    void SignOut();
}