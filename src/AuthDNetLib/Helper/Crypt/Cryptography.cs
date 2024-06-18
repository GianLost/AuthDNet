using AuthDNetLib.Interfaces.Crypt;

namespace AuthDNetLib.Helper.Crypt;

/// <summary>
/// Serviço que implementa a interface <see cref="ICryptography"/> para fornecer operações de criptografia e verificação de chaves sensíveis utilizando Bcrypt.
/// </summary>
public class Cryptography : ICryptography
{
    /// <summary>
    /// Criptografa uma chave usando Bcrypt com configurações padrão de salt e work factor.
    /// </summary>
    /// <param name="key">A chave em texto simples a ser criptografada.</param>
    /// <returns>O hash da chave criptografada usando Bcrypt.</returns>
    /// <remarks>
    /// Este método utiliza as configurações padrão de Bcrypt, que incluem um salt gerado aleatoriamente e um work factor padrão.
    /// O salt é uma cadeia de caracteres aleatória adicionada à chave antes do hashing para aumentar a segurança.
    /// </remarks>
    public string EncryptKey(string key)
    {
        return BCrypt.Net.BCrypt.HashPassword(key);
    }

    /// <summary>
    /// Criptografa uma chave usando Bcrypt com um work factor especificado.
    /// </summary>
    /// <param name="key">A chave em texto simples a ser criptografada.</param>
    /// <param name="workFactor">O fator de trabalho (work factor) para o processo de hashing em Bcrypt, que controla o número de iterações de hashing.</param>
    /// <returns>O hash da chave criptografada usando Bcrypt com o work factor especificado.</returns>
    /// <remarks>
    /// O work factor determina a complexidade do hashing e, consequentemente, o tempo necessário para o processamento.
    /// Um work factor mais alto aumenta a segurança, mas também o tempo de criptografia.
    /// O valor padrão do work factor é 15.
    /// </remarks>
    public string EncryptKey(string key, int workFactor)
    {
        return BCrypt.Net.BCrypt.HashPassword(key, workFactor);
    }

    /// <summary>
    /// Converte uma chave em texto simples para uma string codificada em Base64.
    /// </summary>
    /// <param name="key">A chave em texto simples a ser codificada.</param>
    /// <returns>A chave codificada em Base64.</returns>
    /// <remarks>
    /// Este método converte a chave de texto simples em uma sequência de bytes usando a codificação UTF-8, 
    /// e depois converte esses bytes em uma string Base64.
    /// </remarks>
    public string EncryptToBase64(string key)
    {
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(key));
    }

    /// <summary>
    /// Decodifica uma string codificada em Base64 para uma chave em texto simples.
    /// </summary>
    /// <param name="encryptedData">A string codificada em Base64 a ser decodificada.</param>
    /// <returns>A chave decodificada em texto simples.</returns>
    /// <remarks>
    /// Este método converte a string Base64 de volta em uma sequência de bytes e 
    /// depois converte esses bytes em uma string de texto simples usando a codificação UTF-8.
    /// </remarks>
    public string DecryptFromBase64(string encryptedData)
    {
        return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encryptedData));
    }

    /// <summary>
    /// Verifica se uma chave em texto simples corresponde ao hash criptografado usando Bcrypt.
    /// </summary>
    /// <param name="key">A chave em texto simples a ser verificada.</param>
    /// <param name="hashedKey">O hash da chave criptografada.</param>
    /// <returns>True se a chave em texto simples corresponder ao hash criptografado, caso contrário, false.</returns>
    /// <remarks>
    /// Este método compara a chave em texto simples (`key`) com o hash criptografado (`hashedKey`) usando o algoritmo Bcrypt.
    /// Isso permite autenticar uma chave fornecida pelo usuário contra a chave armazenada de forma segura.
    /// </remarks>
    public bool VerifyEncryptedKey(string key, string hashedKey)
    {
        return BCrypt.Net.BCrypt.Verify(key, hashedKey);
    }
}