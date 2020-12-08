using System;
using System.IO;
using System.Security.Cryptography;

namespace laget.PskAuthentication
{
    public static class PskEncryptor
    {
        public static string Encrypt(string text, string key, string iv)
        {
            return Encrypt(text, Convert.FromBase64String(key), Convert.FromBase64String(iv));
        }

        public static string Encrypt(string text, byte[] key, byte[] iv)
        {
            using (var rijndael = new RijndaelManaged())
            {
                rijndael.Padding = PaddingMode.PKCS7;
                rijndael.Key = key;
                rijndael.IV = iv;

                using (var encryptor = rijndael.CreateEncryptor(rijndael.Key, rijndael.IV))
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }

                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
        }

        public static string Decrypt(string text, string key, string iv)
        {
            return Decrypt(text, Convert.FromBase64String(key), Convert.FromBase64String(iv));
        }

        public static string Decrypt(string text, byte[] key, byte[] iv)
        {
            using (var rijndael = new RijndaelManaged())
            {
                rijndael.Padding = PaddingMode.PKCS7;
                rijndael.Key = key;
                rijndael.IV = iv;

                using (var decryptor = rijndael.CreateDecryptor(rijndael.Key, rijndael.IV))
                using (var msDecrypt = new MemoryStream(Convert.FromBase64String(text)))
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
