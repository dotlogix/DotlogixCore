using System;

namespace DotLogix.Core.Utils.Patterns {
    public struct PatternRange {
        public int Min;
        public int Max;

        public PatternRange(int min, int max) {
            Min = min;
            Max = max;
        }

        public string ToRegexString() {
            if(Max >= 0) {
                return Min == Max
                           ? $"{{{Min}}}"
                           : $"{{{Math.Max(0, Min)},{Max}}}";
            }

            return Min >= 0
                       ? $"{{{Min},}}"
                       : "+?";
        }

        public override string ToString()
        {
            if (Max >= 0)
            {
                return Min == Max
                           ? $"{Min}"
                           : $"{Math.Max(0, Min)}..{Max}";
            }

            return Min >= 0
                       ? $"{Min}.."
                       : "..";
        }


        public static PatternRange Parse(string rangeStr) {
            return TryParse(rangeStr, out var range) ? range : default(PatternRange);
        }

        public static bool TryParse(string rangeStr, out PatternRange patternRange) {
            patternRange = new PatternRange(-1, -1);

            if(string.IsNullOrEmpty(rangeStr))
                return false;

            var rangeSplit = rangeStr.IndexOf("..", StringComparison.Ordinal);
            switch(rangeSplit) {
                case -1:
                    if(int.TryParse(rangeStr, out var minmax) == false)
                        return false;
                    patternRange.Min = minmax;
                    patternRange.Max = minmax;
                    return true;
                case 0 when rangeStr.Length == 2:
                    return false;
            }

            var minStr = rangeStr.Substring(0, rangeSplit);
            var maxStr = rangeStr.Substring(rangeSplit + 2);

            if((minStr.Length != 0) && (int.TryParse(minStr, out patternRange.Min) == false))
                return false;

            if((maxStr.Length != 0) && (int.TryParse(maxStr, out patternRange.Max) == false))
                return false;

            return patternRange.Min <= patternRange.Max;
        }
    }
}