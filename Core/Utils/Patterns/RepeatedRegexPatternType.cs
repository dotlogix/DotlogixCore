namespace DotLogix.Core.Utils.Patterns {
    public class RepeatedRegexPatternType : IRegexPatternType
    {
        private readonly string _pattern;

        public RepeatedRegexPatternType(string name, string pattern) {
            _pattern = pattern;
            Name = name;
        }
        public string Name { get; }
        public string GetRegexPattern(string[] args) {
            if((args.Length > 0) && PatternRange.TryParse(args[0], out var range))
                return _pattern + range;
            return _pattern + "+?";
        }
    }
}