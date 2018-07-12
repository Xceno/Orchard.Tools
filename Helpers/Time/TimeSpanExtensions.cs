namespace Orchard.Tools.Helpers.Time {
    using System;

    public static class TimeSpanExtensions {
        public static TimeSpan RoundTo(this TimeSpan timeSpan, int n) {
            return TimeSpan.FromMinutes(n * Math.Ceiling(timeSpan.TotalMinutes / n));
        }
    }
}