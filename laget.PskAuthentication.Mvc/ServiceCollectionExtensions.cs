using Microsoft.Extensions.DependencyInjection;
using System;

namespace laget.PskAuthentication.Mvc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPskAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(PskAuthenticationSchemeOptions.DefaultScheme)
                .AddScheme<PskAuthenticationSchemeOptions, PskAuthenticationHandler>(PskAuthenticationSchemeOptions.DefaultScheme, _ => { });

            return services;
        }

        public static IServiceCollection AddPskAuthentication(this IServiceCollection services, Action<PskAuthenticationSchemeOptions> configureOptions)
        {
            services.AddAuthentication(PskAuthenticationSchemeOptions.DefaultScheme)
                .AddScheme<PskAuthenticationSchemeOptions, PskAuthenticationHandler>(PskAuthenticationSchemeOptions.DefaultScheme, configureOptions);

            return services;
        }
    }
}
