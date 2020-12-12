using System;
using laget.PskAuthentication.Core;
using Microsoft.AspNetCore.Mvc;

namespace laget.PskAuthentication.Mvc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PskAuthenticationAttribute : TypeFilterAttribute
    {
        public PskAuthenticationAttribute(PskAuthenticationOptions options)
            : base(typeof(PskAuthenticationFilter))
        {
            Arguments = new object[] { options };
        }
    }
}
