using laget.PskAuthentication.Client;
using laget.PskAuthentication.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace laget.PskAuthentication.Generator
{
    public class Service : IHostedService
    {
        private readonly IConfiguration _configuration;

        public Service(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var ttl = 315360000; // 10 years in seconds
            var options = new PskAuthenticationOptions
            {
                IV = _configuration.GetValue<string>("Security:IV"),
                Key = _configuration.GetValue<string>("Security:Key"),
                Salt = _configuration.GetValue<string>("Security:Salt"),
                Secret = _configuration.GetValue<string>("Security:Secret"),
                Ttl = ttl
            };
            var generator = new PskGenerator(options);
            var psk = generator.Generate(ttl);

            Log.Information("PSK:");
            Log.Information(psk);

            Log.Information("Done!!!");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Environment.Exit(0);
        }
    }
}
