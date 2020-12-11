using System;
using System.Security.Cryptography;
using System.Text;
using laget.PskAuthentication.Core;
using laget.PskAuthentication.Core.Extensions;

namespace laget.PskAuthentication.Client
{
    public class PskGenerator
    {
        readonly string _rijndaelIv;
        readonly string _rijndaelKey;
        readonly string _salt;
        readonly string _secret;

        protected HashAlgorithm Algorithm { get; set; } = SHA512.Create();

        public PskGenerator(string rijndaelIv, string rijndaelKey)
        {
            _rijndaelIv = rijndaelIv;
            _rijndaelKey = rijndaelKey;
        }

        public PskGenerator(PskAuthenticationOptions options)
        {
            _rijndaelIv = options.RijndaelIV;
            _rijndaelKey = options.RijndaelKey;
            _salt = options.Salt;
            _secret = options.Secret;
        }

        public string Generate(int ttl = 900)
        {
            return Generate(_salt, _secret, ttl);
        }

        public string Generate(string salt, string secret, int ttl = 900)
        {
            var hash = GetHash(salt, secret);
            var timestamp = DateTime.Now.ToUnix();
            var psk = $"algorithm=SHA-{Algorithm.HashSize}, ts={timestamp}, hash={hash}, ttl={ttl}";

            return PskEncryptor.Encrypt(psk, _rijndaelKey, _rijndaelIv);
        }

        string GetHash(string salt, string secret)
        {
            using (var algo = Algorithm)
            {
                var hash = algo.ComputeHash(Encoding.Default.GetBytes(secret + salt));

                algo.Clear();

                return Convert.ToBase64String(hash);
            }
        }

        public PskGenerator UseAlgorithm(HashAlgorithm algorithm)
        {
            Algorithm = algorithm;
            return this;
        }
    }
}
