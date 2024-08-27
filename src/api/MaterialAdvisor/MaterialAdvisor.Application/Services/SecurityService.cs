using MaterialAdvisor.Application.Configuration.Options;

using Microsoft.Extensions.Options;

using System.Security.Cryptography;
using System.Text;

namespace MaterialAdvisor.Application.Services;

public class SecurityService : ISecurityService
{
    private readonly string _key;
    const int KeySize = 16;

    public SecurityService(IOptions<SecurityOptions> securityOptions)
    {
        _key = securityOptions.Value.Key;
    }

    public string GetHash(string input)
    {
        var inputBytes = Encoding.UTF8.GetBytes(input);
        var keyBytes = Encoding.UTF8.GetBytes(_key);

        using (HMACSHA256 hmac = new HMACSHA256(keyBytes))
        {
            var hashBytes = hmac.ComputeHash(inputBytes);
            var hashString = Encoding.UTF8.GetString(hashBytes);
            return hashString;
        }
    }

    public string Encrypt(string input)
    {
        byte[] iv = new byte[KeySize];
        byte[] keyBytes = Encoding.UTF8.GetBytes(_key);

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = keyBytes;
            aesAlg.IV = iv;

            using (var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
            {
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(input);
                        }

                        var encryptedBytes = msEncrypt.ToArray();
                        return Convert.ToBase64String(encryptedBytes);
                    }
                }
            }
        }
    }

    public string Decrypt(string input)
    {
        byte[] iv = new byte[KeySize];
        byte[] keyBytes = Encoding.UTF8.GetBytes(_key);
        byte[] cipherTextBytes = Convert.FromBase64String(input);

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = keyBytes;
            aesAlg.IV = iv;

            using (var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
            {
                using (var msDecrypt = new MemoryStream(cipherTextBytes))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
