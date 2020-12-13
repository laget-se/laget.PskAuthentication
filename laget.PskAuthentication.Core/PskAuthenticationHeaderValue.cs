﻿using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using laget.PskAuthentication.Core.Exceptions;

namespace laget.PskAuthentication.Core
{
    public class PskAuthenticationHeaderValue
    {
        static readonly string[] RequiredAttributes = { "algorithm", "hash", "ts" };
        static readonly string[] OptionalAttributes = { "iss", "sub", "ttl" };
        static readonly string[] SupportedAttributes;

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

        static HashAlgorithm GetAlgorithm(string algorithm)
        {
            return algorithm switch
            {
                "SHA-256" => SHA256.Create(),
                "SHA-384" => SHA384.Create(),
                "SHA-512" => SHA512.Create(),
                _ => throw new PskAlgorithmException($"{algorithm} is unsupported, please provide a supported algorithm")
            };
        }

        static void Validate(NameValueCollection attributes)
        {
            if (!RequiredAttributes.All(a => attributes.AllKeys.Any(k => k == a)))
                throw new PskAttributeException("Missing attributes");

            if (!attributes.AllKeys.All(a => SupportedAttributes.Any(k => k == a)))
                throw new PskAttributeException("Unknown attributes");
        }
    }
}