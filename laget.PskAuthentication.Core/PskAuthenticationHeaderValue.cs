using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using laget.PskAuthentication.Core.Exceptions;

namespace laget.PskAuthentication.Core
{
    public class PskAuthenticationHeaderValue
    {
        private static readonly string[] RequiredAttributes = { "algorithm", "hash", "ts" };
        private static readonly string[] OptionalAttributes = { "iss", "sub", "ttl" };
        private static readonly string[] SupportedAttributes;

        static PskAuthenticationHeaderValue()
        {
            SupportedAttributes = RequiredAttributes.Concat(OptionalAttributes).ToArray();
        }

        public static Psk Parse(string authorization, string rijndaelKey, string rijndaelIV)
        {
            authorization = PskEncryptor.Decrypt(authorization, rijndaelKey, rijndaelIV);

            var attributes = new NameValueCollection();

            foreach (var attribute in authorization.Split(','))
            {
                var index = attribute.IndexOf('=');
                if (index <= 0) continue;

                var key = attribute.Substring(0, index).Trim();
                var value = attribute.Substring(index + 1).Trim();

                if (value.StartsWith("\""))
                    value = value.Substring(1, value.Length - 2);

                attributes.Add(key, value);
            }

            Validate(attributes);

            var psk = new Psk
            {
                Algorithm = GetAlgorithm(attributes["algorithm"]),
                Hash = attributes["hash"],
                Timestamp = long.Parse(attributes["ts"])
            };

            if (attributes["iss"] != null)
            {
                psk.Issuer = attributes["iss"];
            }
            if (attributes["sub"] != null)
            {
                psk.Issuer = attributes["sub"];
            }
            if (attributes["ttl"] != null)
            {
                psk.Ttl = int.Parse(attributes["ttl"]);
            }

            return psk;
        }

        private static HashAlgorithm GetAlgorithm(string algorithm)
        {
            switch (algorithm)
            {
                case "SHA-256":
                    return SHA256.Create();
                case "SHA-384":
                    return SHA384.Create();
                case "SHA-512":
                    return SHA512.Create();
                default:
                    throw new PskAlgorithmException($"{algorithm} is unsupported, please provide a supported algorithm");
            }
        }

        private static void Validate(NameValueCollection attributes)
        {
            if (!RequiredAttributes.All(a => attributes.AllKeys.Any(k => k == a)))
                throw new PskAttributeException("Missing attributes");

            if (!attributes.AllKeys.All(a => SupportedAttributes.Any(k => k == a)))
                throw new PskAttributeException("Unknown attributes");
        }
    }
}