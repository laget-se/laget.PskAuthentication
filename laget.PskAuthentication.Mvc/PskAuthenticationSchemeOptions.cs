using Microsoft.AspNetCore.Authentication;

namespace laget.PskAuthentication.Mvc
{
    public class PskAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "PskAuthenticationScheme";
        public string TokenHeaderName { get; set; } = "X-PSK-Authorization";
    }
}
