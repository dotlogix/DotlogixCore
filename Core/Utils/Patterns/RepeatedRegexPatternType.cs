using System.Collections.Generic;

namespace DotLogix.Core.Utils.Patterns {
    public class RepeatedRegexPatternType : RegexPatternType
    {
        public RepeatedRegexPatternType(string name, string defaultVariant, IReadOnlyDictionary<string, string> patternVariants) : base(name, defaultVariant, patternVariants) { }
        public RepeatedRegexPatternType(string name, string pattern) : base(name, pattern) { }

        public override string GetRegexPattern(string variant, string[] args) {
            var pattern = base.GetRegexPattern(variant, args);
            if(pattern == null)
                return null;

            if((args.Length > 0) && PatternRange.TryParse(args[0], out var range))
                return pattern + range.ToRegexString();
            return pattern + "+?";
        }
    }
}