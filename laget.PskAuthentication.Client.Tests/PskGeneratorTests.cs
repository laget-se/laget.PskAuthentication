using System.Security.Cryptography;
using FluentAssertions;
using laget.PskAuthentication.Core;
using Xunit;

namespace laget.PskAuthentication.Client.Tests
{
    public class PskGeneratorTests
    {
        private readonly PskAuthenticationOptions _options;

        public PskGeneratorTests()
        {
            _options = new PskAuthenticationOptions
            {
                RijndaelIV = "fWN4n4pXsrXSJCdN9HfjiA==",
                RijndaelKey = "J/IsWTGD5Sx2B124mtDg0Pg8AGslPADgGgiOj0kfxh0=",
                Salt = "fenga1N3BYtRzwV9",
                Secret = "Banana"
            };
        }

        [Fact]
        public void ShouldHandleSha256()
        {
            var generator = new PskGenerator(_options).UseAlgorithm(SHA256.Create());
            var hash = generator.Generate();

            hash.Should().NotBeEmpty();
        }

        [Fact]
        public void ShouldHandleSha384()
        {
            var generator = new PskGenerator(_options).UseAlgorithm(SHA384.Create());
            var hash = generator.Generate();

            hash.Should().NotBeEmpty();
        }

        [Fact]
        public void ShouldHandleSha512()
        {
            var generator = new PskGenerator(_options);
            var hash = generator.Generate();

            hash.Should().NotBeEmpty();
        }
    }
}
