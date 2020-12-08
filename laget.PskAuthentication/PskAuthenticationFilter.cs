using System;
using System.Text;
using laget.PskAuthentication.Exceptions;
using laget.PskAuthentication.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace laget.PskAuthentication
{
    public class PskAuthenticationFilter : IAuthorizationFilter
    {
        readonly string _pskKey;

        public PskAuthenticationFilter(string pskKey)
        {
            _pskKey = pskKey;

            if (string.IsNullOrWhiteSpace(_pskKey))
            {
                throw new ArgumentNullException(nameof(pskKey), "Please provide a non-empty psk value.");
            }
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                string authHeader = context.HttpContext.Request.Headers["X-PSK-Authorization"];

                if (authHeader != null)
                {
                    var config = context.HttpContext.RequestServices.GetService<IConfiguration>();
                    var rijndaelKey = config["Security:RijndaelKey"];
                    var rijndaelIV = config["Security:RijndaelIV"];

                    var psk = PskAuthenticationHeaderValue.Parse(authHeader, rijndaelKey, rijndaelIV);

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
            var config = context.HttpContext.RequestServices.GetService<IConfiguration>();
            var salt = config["Security:Salt"];

            using (var algorithm = psk.Algorithm)
            {
                var hash = algorithm.ComputeHash(Encoding.Default.GetBytes(config[_pskKey] + salt));
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
