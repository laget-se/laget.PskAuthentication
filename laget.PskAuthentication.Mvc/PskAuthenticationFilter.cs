using System;
using System.Text;
using laget.PskAuthentication.Core;
using laget.PskAuthentication.Core.Exceptions;
using laget.PskAuthentication.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace laget.PskAuthentication.Mvc
{
    public class PskAuthenticationFilter : IAuthorizationFilter
    {
        readonly string _prefix;

        public PskAuthenticationFilter(string prefix)
        {
            _prefix = prefix;

            if (string.IsNullOrWhiteSpace(_prefix))
            {
                throw new ArgumentNullException(nameof(_prefix), "Please provide a non-empty prefix value.");
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
                    var rijndaelKey = config[$"{_prefix}:RijndaelKey"];
                    var rijndaelIV = config[$"{_prefix}:RijndaelIV"];

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

            using (var algorithm = psk.Algorithm)
            {
                var hash = algorithm.ComputeHash(Encoding.Default.GetBytes(config[$"{_prefix}:Key"] + config[$"{_prefix}:Salt"]));
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
