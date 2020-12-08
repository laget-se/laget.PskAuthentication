using System;
using Microsoft.AspNetCore.Mvc;

namespace laget.PskAuthentication
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PskAuthenticationAttribute : TypeFilterAttribute
    {
        public PskAuthenticationAttribute(string pskKey = "Security:PSK")
            : base(typeof(PskAuthenticationFilter))
        {
            Arguments = new object[] { pskKey };
        }
    }
}
