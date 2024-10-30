using laget.PskAuthentication.Core;
using System;
using System.Security.Cryptography;
using System.Text;

namespace laget.PskAuthentication.Client
{
    public class PskGenerator
    {
        private readonly string _iv;
        private readonly string _key;
        private readonly string _salt;
        private readonly string _secret;
        private readonly int _ttl;

        private HashAlgorithm Algorithm { get; set; } = SHA512.Create();

        public PskGenerator(string iv, string key)
        {
            _iv = iv;
            _key = key;
            _ttl = 900;
        }

        public PskGenerator(PskAuthenticationOptions options)
        {
            _iv = options.IV;
            _key = options.Key;
            _salt = options.Salt;
            _secret = options.Secret;
            _ttl = options.Ttl;
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

            var cryptographer = new Cryptography.Cryptographer(_key, _iv);
            return cryptographer.Encrypt(psk);
        }

        private string GetHash(string salt, string secret)
        {
            var type = Algorithm.GetType();

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
