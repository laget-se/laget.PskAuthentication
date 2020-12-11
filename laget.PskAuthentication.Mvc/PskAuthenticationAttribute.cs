using System;
using Microsoft.AspNetCore.Mvc;

namespace laget.PskAuthentication.Mvc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PskAuthenticationAttribute : TypeFilterAttribute
    {
        public PskAuthenticationAttribute(string prefix = "Security")
            : base(typeof(PskAuthenticationFilter))
        {
            Arguments = new object[] { prefix };
        }
    }
}
