namespace DotLogix.Core.Utils.Patterns {
    public class RegexPatternType : IRegexPatternType {
        private readonly string _pattern;
        public string Name { get; }

        public RegexPatternType(string name, string pattern) {
            _pattern = pattern;
            Name = name;
        }

        public string GetRegexPattern(string[] args) {
            return _pattern;
        }
    }
}