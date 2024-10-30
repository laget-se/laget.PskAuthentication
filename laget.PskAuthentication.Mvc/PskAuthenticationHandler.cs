using laget.PskAuthentication.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace laget.PskAuthentication.Mvc
{
    public class PskAuthenticationHandler : AuthenticationHandler<PskAuthenticationSchemeOptions>
    {
        private readonly PskAuthenticationOptions _pskAuthenticationOptions;

        public PskAuthenticationHandler(IOptionsMonitor<PskAuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, PskAuthenticationOptions pskAuthenticationOptions)
            : base(options, logger, encoder, clock)
        {
            _pskAuthenticationOptions = pskAuthenticationOptions;
        }

        public PskAuthenticationHandler(IOptionsMonitor<PskAuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, PskAuthenticationOptions pskAuthenticationOptions)
            : base(options, logger, encoder)
        {
            _pskAuthenticationOptions = pskAuthenticationOptions;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(Options.TokenHeaderName))
            {
                return AuthenticateResult.Fail($"Missing header: {Options.TokenHeaderName}");
            }

            string token = Request.Headers[Options.TokenHeaderName]!;
            try
            {
                var psk = PskAuthenticationHeaderValue.Parse(token, _pskAuthenticationOptions.Key, _pskAuthenticationOptions.IV);

                if (!psk.IsValid())
                {
                    if (!IsAuthorized(_pskAuthenticationOptions, psk))
                    {
                        return AuthenticateResult.Fail("Invalid PSK");
                    }

                    return AuthenticateResult.Fail("Invalid PSK");
                }

                var claims = new[] { new Claim(ClaimTypes.Expiration, psk.Timestamp.ToString()) };
                var claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name));
            }
            catch (PskExpiredException ex)
            {
                AuthenticateResult.Fail($"PskExpiredException: {ex.Message}");
            }
            catch (FormatException ex)
            {
                AuthenticateResult.Fail($"FormatException: {ex.Message}");
            }

            return AuthenticateResult.Fail("Unknown");
        }

        public bool IsAuthorized(PskAuthenticationOptions options, Psk psk)
        {
            using (var algorithm = psk.Algorithm)
            {
                var hash = algorithm.ComputeHash(Encoding.Default.GetBytes(options.Secret + options.Salt));
                algorithm.Clear();

                return psk.IsEqualTo(Convert.ToBase64String(hash));
            }
        }
    }
}
