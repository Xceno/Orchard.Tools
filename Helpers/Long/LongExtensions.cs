namespace Orchard.Tools.Helpers.Long {
    using System;
    using System.Collections.Generic;

    public static class LongExtensions {
        // Should be good enough for now. Will be fun reading this comment in 20 years ;)
        private static readonly List<string> units = new List<string>(5) { "B", "KB", "MB", "GB", "TB" };

        public static string ToHumanReadableString(this long bytes) {
            var sizeAndUnit = ConvertToNextUnit(bytes, units[0]);
            var numberOfDecimalPlaces = units[0] == sizeAndUnit.Item2 ? 0 : units.IndexOf(sizeAndUnit.Item2) - 1;
            return string.Format("{0} {1}", Math.Round(sizeAndUnit.Item1, numberOfDecimalPlaces), sizeAndUnit.Item2);
        }

        private static Tuple<double, string> ConvertToNextUnit(double size, string unit) {
            var indexOfUnit = units.IndexOf(unit);

            if ( size > 1024 && indexOfUnit < units.Count - 1 ) {
                size = size / 1024;
                unit = units[indexOfUnit + 1];
                return ConvertToNextUnit(size, unit);
            }

            return new Tuple<double, string>(size, unit);
        }
    }
}