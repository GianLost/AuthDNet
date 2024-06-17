using System.Text;
using System.Text.Json;
using System.Security.Cryptography;
using AuthDNetLib.Helper.Messages;

namespace AuthDNetLib.Helper.Transfer.Data;

public static class JSONDataTransfer<TKey> where TKey : class
{
    private static readonly byte[] _Key = Encoding.UTF8.GetBytes("character-key0@55YssY??-&&36A9W=");
    private static readonly byte[] _IV = Encoding.UTF8.GetBytes("char-iv1=Key00?#");

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