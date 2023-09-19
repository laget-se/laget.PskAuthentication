using FluentAssertions;
using laget.PskAuthentication.Client.Extensions;
using laget.PskAuthentication.Core;
using System.Net.Http;
using Xunit;

namespace laget.PskAuthentication.Client.Tests.Extensions
{
    public class HttpClientTests
    {
        readonly PskAuthenticationOptions _options;

        public HttpClientTests()
        {
            _options = new PskAuthenticationOptions
            {
                IV = "fWN4n4pXsrXSJCdN9HfjiA==",
                Key = "J/IsWTGD5Sx2B124mtDg0Pg8AGslPADgGgiOj0kfxh0=",
                Salt = "fenga1N3BYtRzwV9",
                Secret = "Banana"
            };
        }


        [Fact]
        public void ShouldAddPskAuthentication()
        {
            var client = new HttpClient().AddPskAuthentication(_options);

            client.DefaultRequestHeaders.Contains("X-PSK-Authorization").Should().BeTrue();
        }
    }
}
