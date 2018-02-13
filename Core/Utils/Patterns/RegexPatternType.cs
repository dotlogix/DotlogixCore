using System.Collections.Generic;

namespace DotLogix.Core.Utils.Patterns {
    public class RegexPatternType : IRegexPatternType {
        protected string DefaultVariant { get; }
        protected IReadOnlyDictionary<string, string> PatternVariants { get; }
        public string Name { get; }

        public RegexPatternType(string name, string defaultVariant, IReadOnlyDictionary<string,string> patternVariants) {
            DefaultVariant = defaultVariant;
            PatternVariants = patternVariants;
            Name = name;
        }
        public RegexPatternType(string name, string pattern) {
            PatternVariants = new Dictionary<string, string> {{"", pattern}};
            DefaultVariant = "";
            Name = name;
        }

        public virtual string GetRegexPattern(string variant, string[] args) {
            return PatternVariants.TryGetValue(variant??DefaultVariant, out var pattern) ? pattern : null;
        }
    }
}