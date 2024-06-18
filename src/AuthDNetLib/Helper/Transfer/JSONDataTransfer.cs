using System.Text;
using System.Text.Json;
using System.Security.Cryptography;
using AuthDNetLib.Helper.Messages;

namespace AuthDNetLib.Helper.Transfer.Data;

/// <summary>
/// Classe estática para serializar e desserializar dados de forma segura utilizando criptografia AES.
/// </summary>
/// <typeparam name="TKey">O tipo de dados a serem serializados/desserializados.</typeparam>
public static class JSONDataTransfer<TKey> where TKey : class
{
    private static readonly byte[] _Key = Encoding.UTF8.GetBytes("character-key0@55YssY??-&&36A9W=");
    private static readonly byte[] _IV = Encoding.UTF8.GetBytes("char-iv1=Key00?#");

    /// <summary>
    /// Serializa e criptografa um objeto em uma string JSON.
    /// </summary>
    /// <param name="data">O objeto a ser serializado e criptografado.</param>
    /// <returns>Uma string JSON criptografada representando o objeto.</returns>
    public static string JSONSecureDataSerialize(TKey data)
    {
        string jsonData = JsonSerializer.Serialize(data);

        using Aes aesAlg = Aes.Create();

        aesAlg.Key = _Key;
        aesAlg.IV = _IV;

        ICryptoTransform crypt = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        using MemoryStream msCrypt = new();

        using (CryptoStream csCrypt = new(msCrypt, crypt, CryptoStreamMode.Write))
        {
            using StreamWriter swCrypt = new(csCrypt);
            swCrypt.Write(jsonData);
        }

        return Convert.ToBase64String(msCrypt.ToArray());
    }

    /// <summary>
    /// Desserializa e descriptografa uma string JSON para um objeto.
    /// </summary>
    /// <param name="secureJson">A string JSON criptografada a ser descriptografada e desserializada.</param>
    /// <returns>O objeto desserializado.</returns>
    /// <exception cref="InvalidOperationException">Lançada se ocorrer um erro durante a desserialização JSON.</exception>
    public static async Task<TKey> JSONSecureDataDesserialize(string secureJson)
    {
        byte[] cipherText = Convert.FromBase64String(secureJson);

        using Aes aesAlg = Aes.Create();

        aesAlg.Key = _Key;
        aesAlg.IV = _IV;

        ICryptoTransform decrypt = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        using MemoryStream msDecrypt = new(cipherText);

        using CryptoStream csDecrypt = new(msDecrypt, decrypt, CryptoStreamMode.Read);
        using StreamReader srDecrypt = new(csDecrypt);

        string jsonString = srDecrypt.ReadToEnd();

        TKey? data = await JsonSerializer.DeserializeAsync<TKey>(new MemoryStream(Encoding.UTF8.GetBytes(jsonString))) ?? throw new InvalidOperationException($"{ErrorMessages.MsgJSONDesserializeError} Object: {secureJson}");

        return data;
    }
}