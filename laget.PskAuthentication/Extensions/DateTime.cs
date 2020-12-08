using System;

namespace laget.PskAuthentication.Extensions
{
    public static class DateTimeExtensions
    {
        static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long ToUnix(this DateTime dateTime)
        {
            return Convert(dateTime);
        }

        public static DateTime ToDateTime(this long epoch)
        {
            return Convert(epoch);
        }

        public static DateTime ToDateTime(this int epoch)
        {
            return Convert(epoch);
        }

        static long Convert(DateTime date)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var diff = date.ToUniversalTime() - origin;
            return (long)Math.Floor(diff.TotalSeconds);
        }

        static DateTime Convert(long epoch)
        {
            return Epoch.AddSeconds(epoch);
        }
    }
}
