using System;
using FluentAssertions;
using laget.PskAuthentication.Extensions;
using Xunit;

namespace laget.PskAuthentication.Tests.Extensions
{
    public class DateTimeTests
    {
        [Fact]
        public void ShouldConvertDateTimeToUnixTimestamp()
        {
            var datetime = new DateTime(2020, 01, 01, 00, 00, 00).ToUniversalTime();

            datetime.ToUnix().Should().Be(1577833200);
        }

        [Fact]
        public void ShouldConvertUnixTimestampToDateTime_long()
        {
            const long epoch = 1577833200;

            epoch.ToDateTime().Should().Be(new DateTime(2020, 01, 01, 00, 00, 00).ToUniversalTime());
        }

        [Fact]
        public void ShouldConvertUnixTimestampToDateTime_int()
        {
            const int epoch = 1577833200;

            epoch.ToDateTime().Should().Be(new DateTime(2020, 01, 01, 00, 00, 00).ToUniversalTime());
        }
    }
}
