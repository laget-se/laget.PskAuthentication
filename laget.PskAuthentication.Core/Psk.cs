using System;
using System.Security.Cryptography;
using laget.PskAuthentication.Core.Exceptions;
using laget.PskAuthentication.Core.Extensions;

namespace laget.PskAuthentication.Core
{
    public class Psk
    {
        const int DefaultTtl = 900;

        public HashAlgorithm Algorithm { get; set; }
        public DateTime DateTime => Timestamp.ToDateTime();
        public long Timestamp { get; set; }
        public string Hash { get; set; }

        public int Ttl { get; set; } = DefaultTtl;
        public string Issuer { get; set; } = null;

        public bool IsValid()
        {
            if (IsExpired(Timestamp, Ttl))
            {
                throw new PskExpiredException("The Psk (Pre-shared Key) has expired, please re-generate the psk and re-submit the request");
            }

            return true;
        }

        public static bool IsExpired(long ts, int ttl)
        {
            var now = DateTime.Now.ToUnix();
            var result = Math.Abs(ts - now);

            return result > ttl;
        }

        public override string ToString()
        {
            var psk = $"algorithm=SHA-{Algorithm.HashSize}, ts={Timestamp}, hash={Hash}";

            if (Ttl != 0 || Ttl != DefaultTtl)
            {
                psk += $", ttl={Ttl}";
            }

            if (!string.IsNullOrEmpty(Issuer))
            {
                psk += $", iss={Issuer}";
            }

            return psk;
        }
    }
}
