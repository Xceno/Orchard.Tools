namespace Orchard.Tools.Helpers.Time {
    using System;

    public static class DateTimeOffsetExtensions {

        /// <summary>
        /// Adds the given number of seconds on top oft the UNIX Epoch (1970-01-01) and constructs a new DateTimeOffset accordingly.
        /// </summary>
        public static DateTimeOffset ToDateTimeOffsetFromUnixTimeSeconds(this long seconds) {
            var unixEpoch = new DateTimeOffset(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
            unixEpoch = unixEpoch.AddSeconds(seconds);
            return unixEpoch;
        }

        /// <summary>
        /// Converts a DateTimeOffset to the number of milliseconds since the UNIX Epoch (1970-01-01).
        /// </summary>
        public static long ToUnixTimeSeconds(this DateTimeOffset dateTimeOffset) {
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var unixTimeStampInTicks = (dateTimeOffset.ToUniversalTime() - unixEpoch).Ticks;
            return unixTimeStampInTicks / TimeSpan.TicksPerSecond;
        }
    }
}