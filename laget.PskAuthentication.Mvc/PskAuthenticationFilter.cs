using System;
using System.Text;
using laget.PskAuthentication.Core;
using laget.PskAuthentication.Core.Exceptions;
using laget.PskAuthentication.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace laget.PskAuthentication.Mvc
{
    public class PskAuthenticationFilter : IAuthorizationFilter
    {
        readonly PskAuthenticationOptions _options;

        public PskAuthenticationFilter(PskAuthenticationOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options), ""); //TODO: Write error message
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                string authHeader = context.HttpContext.Request.Headers[_options.HeaderName];

                if (authHeader != null)
                {
                    var psk = PskAuthenticationHeaderValue.Parse(authHeader, _options.RijndaelKey, _options.RijndaelIV);

                    if (psk.IsValid())
                    {
                        if (IsAuthorized(context, psk))
                        {
                            return;
                        }
                    }
                }

                ReturnUnauthorizedResult(context, "Unknown");
            }
            catch (PskExpiredException ex)
            {
                ReturnUnauthorizedResult(context, $"PskExpiredException: {ex.Message}");
            }
            catch (FormatException ex)
            {
                ReturnUnauthorizedResult(context, $"FormatException: {ex.Message}");
            }
        }

        public bool IsAuthorized(AuthorizationFilterContext context, Psk psk)
        {
            using (var algorithm = psk.Algorithm)
            {
                var hash = algorithm.ComputeHash(Encoding.Default.GetBytes(_options.Secret + _options.Salt));
                algorithm.Clear();

                return psk.IsEqualTo(Convert.ToBase64String(hash));
            }
        }

        static void ReturnUnauthorizedResult(AuthorizationFilterContext context, string reason)
        {
            context.HttpContext.Response.Headers["WWW-Authenticate"] = $"PSK {reason}";
            context.Result = new UnauthorizedResult();
        }
    }
}
