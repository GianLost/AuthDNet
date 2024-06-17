namespace AuthDNetLib.Interfaces.Crypt;

/// <summary>
/// Interface para operações de criptografia de chaves sensíveis utilizando o Bcrypt.
/// </summary>
public interface ICryptography
{
    /// <summary>
    /// Criptografa uma chave usando Bcrypt com as configurações padrão de salt e work factor.
    /// </summary>
    /// <param name="key">A chave a ser criptografada.</param>
    /// <returns>O hash criptografado da chave.</returns>
    /// <remarks>
    /// Este método utiliza as configurações padrão de Bcrypt, que incluem um work factor padrão e um salt gerado aleatoriamente.
    /// </remarks>
    string EncryptKey(string key);

    /// <summary>
    /// Criptografa uma chave usando Bcrypt com um `work factor` especificado.
    /// </summary>
    /// <param name="key">A chave a ser criptografada.</param>
    /// <param name="workFactor">O fator de trabalho (`work factor`) para Bcrypt, que determina o número de iterações de hashing. 
    /// Um valor maior aumenta a segurança, mas também aumenta o tempo necessário para a criptografia.</param>
    /// <returns>O hash criptografado da chave.</returns>
    /// <remarks>
    /// Este método permite especificar um `work factor` para controlar o nível de segurança da criptografia.
    /// Um valor maior torna a chave mais segura contra ataques, mas também aumenta o tempo de processamento.
    /// </remarks>
    string EncryptKey(string key, int workFactor);

    /// <summary>
    /// Criptografa uma chave usando Base64String.
    /// </summary>
    /// <param name="key">A chave a ser criptografada.</param>
    /// <returns>A string criptografada da chave.</returns>
    abstract string EncryptToBase64(string key);

    /// <summary>
    /// Descriptografa uma chave usando FromBase64String.
    /// </summary>
    /// <param name="key">A chave a ser descriptografada.</param>
    /// <returns>A string descriptografada da chave.</returns>
    abstract string DecryptFromBase64(string encryptedKey);

    /// <summary>
    /// Verifica se uma chave corresponde a um hash criptografado usando Bcrypt.
    /// </summary>
    /// <param name="key">A chave não criptografada a ser verificada.</param>
    /// <param name="hashedKey">O hash da chave criptografada.</param>
    /// <returns>Retorna true se a chave corresponder ao hash criptografado, caso contrário false.</returns>
    /// <remarks>
    /// Este método verifica se a chave não criptografada (`key`) corresponde ao hash criptografado (`hashedKey`).
    /// Isso é útil para autenticação de chaves ao verificar sua chave contra a chave armazenada em um banco de dados.
    /// </remarks>
    bool VerifyEncryptedKey(string key, string hashedKey);
}