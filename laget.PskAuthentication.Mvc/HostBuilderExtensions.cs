using laget.PskAuthentication.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace laget.PskAuthentication.Mvc
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder ConfigurePskAuthentication(this IHostBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<IPskAuthenticationOptions>(context.Configuration.GetSection("Security").Get<PskAuthenticationOptions>());
                services.AddSingleton<PskAuthenticationOptions>(context.Configuration.GetSection("Security").Get<PskAuthenticationOptions>());
            });

            return builder;
        }

        public static IHostBuilder ConfigurePskAuthentication(this IHostBuilder builder, PskAuthenticationOptions options)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<IPskAuthenticationOptions>(options);
                services.AddSingleton<PskAuthenticationOptions>(options);
            });

            return builder;
        }
    }
}
