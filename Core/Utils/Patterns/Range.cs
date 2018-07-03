using System;
using System.Text.RegularExpressions;

namespace DotLogix.Core.Utils.Patterns {
    public struct Range {
        public int? Min;
        public int? Max;

        public Range(int? min = null, int? max = null) {
            Min = min;
            Max = max;
        }


        public override string ToString() {
            if(Max.HasValue && Min.HasValue)
                return $"({Max.Value}..{Min.Value})";
            if(Max.HasValue)
                return $"(..{Max.Value})";
            if(Min.HasValue)
                return $"({Min.Value}..)";
            return "(..)";
        }

        public static Range Parse(string value) {
            if(TryParse(value, out var range))
                return range;
            throw new ArgumentException("Value is not a valid range statement", nameof(value));
        }

        public static bool TryParse(string value, out Range range) {
            var matches = Regex.Match(value, "^\\(?(?:(?<exact>-?\\d+)|(?<min>-?\\d)?\\.\\.(?<max>-?\\d))?\\)?$");
            if(matches.Success) {
                if(matches.Groups["exact"].Success) {
                    var exact = int.Parse(matches.Groups["exact"].Value);
                    range = new Range(exact, exact);
                    return true;
                }

                int? min = null;
                int? max = null;

                if(matches.Groups["min"].Success)
                    min = int.Parse(matches.Groups["min"].Value);
                if(matches.Groups["max"].Success)
                    max = int.Parse(matches.Groups["max"].Value);

                range = new Range(min, max);
                return true;
            }
            range = default(Range);
            return false;
        }
    }
}