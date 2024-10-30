using FluentAssertions;
using System;
using Xunit;

namespace laget.PskAuthentication.Core.Tests
{
    public class DateTimeExtensionsTests
    {
        private const long Epoch = 1577836800;

        [Fact]
        public void ShouldConvertDateTimeToUnixTimestamp()
        {
            var datetime = new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc).ToUniversalTime();

            datetime.ToUnix().Should().Be(Epoch);
        }

        [Fact]
        public void ShouldConvertUnixTimestampToDateTime_long()
        {
            Epoch.ToDateTime().Should().Be(new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc).ToUniversalTime());
        }

        [Fact]
        public void ShouldConvertUnixTimestampToDateTime_int()
        {
            Epoch.ToDateTime().Should().Be(new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc).ToUniversalTime());
        }
    }
}
