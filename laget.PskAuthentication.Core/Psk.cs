using System;
using System.Security.Cryptography;
using laget.PskAuthentication.Core.Exceptions;
using laget.PskAuthentication.Core.Extensions;

namespace laget.PskAuthentication.Core
{
    public class Psk
    {
        private const int DefaultTtl = 900;

        public HashAlgorithm Algorithm { get; set; }

        public long Timestamp { get; set; }
        public string Hash { get; set; }
        public string Subject { get; set; } = null;
        public string Issuer { get; set; } = null;
        public int Ttl { get; set; } = DefaultTtl;


        public bool IsValid()
        {
            if (IsExpired(Timestamp, Ttl))
            {
                throw new PskExpiredException("The Psk (Pre-shared Key) has expired, please re-generate the psk and re-send the request");
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

            if (!string.IsNullOrEmpty(Subject))
            {
                psk += $", sub={Subject}";
            }
            if (!string.IsNullOrEmpty(Issuer))
            {
                psk += $", iss={Issuer}";
            }
            if (Ttl != 0 || Ttl != DefaultTtl)
            {
                psk += $", ttl={Ttl}";
            }

            return psk;
        }
    }
}
