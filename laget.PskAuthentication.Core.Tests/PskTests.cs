using System;
using System.Security.Cryptography;
using FluentAssertions;
using laget.PskAuthentication.Core.Extensions;
using Xunit;

namespace laget.PskAuthentication.Core.Tests
{
    public class PskTests
    {
        [Fact]
        public void ShouldConvertDateTimeToUnixTimestamp()
        {
            var psk = new Psk
            {
                Algorithm = SHA512.Create(),
                Hash = "ee26b0dd4af7e749aa1a8ee3c10ae9923f618980772e473f8819a5d4940e0db27ac185f8a0e1d5f84f88bc887fd67b143732c304cc5fa9ad8e6f57f50028a8ff",
                Timestamp = DateTime.UtcNow.ToUnix()
            };

            psk.Timestamp.Should().Be(DateTime.UtcNow.ToUnix());
        }
    }
}
