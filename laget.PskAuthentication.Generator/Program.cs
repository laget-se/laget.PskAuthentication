using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace laget.PskAuthentication.Generator
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder()
                //.ConfigureContainer<ContainerBuilder>((context, builder) =>
                //{
                //    builder.RegisterModule<RepositoryModule>();
                //    builder.RegisterModule<ServiceModule>();
                //})
                .ConfigureServices((context, services) =>
                {
                    services.AddHostedService<Service>();
                })
                .UseConsoleLifetime()
                .UseEnvironment(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development")
                .UseSerilog((context, services, configuration) => configuration.ReadFrom.Configuration(context.Configuration))
                //.UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .Build()
                .RunAsync();
        }
    }
}
