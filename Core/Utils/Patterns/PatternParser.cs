using System;
using System.Collections.Generic;

namespace DotLogix.Core.Utils.Patterns {
    public class PatternParser {
        public static PatternParser Default { get; }

        static PatternParser() {
            var parser = new PatternParser();
            parser.AddPattern(new RegexPatternType("guid", "[0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12}"));
            parser.AddPattern(new RegexPatternType("guid:n", "[0-9A-Fa-f]{32}"));
            parser.AddPattern(new RegexPatternType("guid:d", "[0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12}"));
            parser.AddPattern(new RegexPatternType("guid:b", "\\{[0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12}\\}"));
            parser.AddPattern(new RegexPatternType("guid:p", "\\([0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12}\\)"));
            parser.AddPattern(new RepeatedRegexPatternType("any", "."));
            parser.AddPattern(new RepeatedRegexPatternType("number", "\\d"));
            parser.AddPattern(new RepeatedRegexPatternType("hex", "[0-9a-fA-F]"));
            parser.AddPattern(new RepeatedRegexPatternType("letter", "[a-zA-Z]"));
            parser.AddPattern(new RepeatedRegexPatternType("word", "\\w"));
            parser.AddPattern(new RepeatedRegexPatternType("word:s", "[\\w\\s]"));
            parser.AddPattern(new RepeatedRegexPatternType("letter:u", "[A-Z]"));
            parser.AddPattern(new RepeatedRegexPatternType("letter:l", "[a-z]"));
            Default = parser;
        }


        private readonly Dictionary<string, IRegexPatternType> _patterns = new Dictionary<string, IRegexPatternType>(StringComparer.OrdinalIgnoreCase);

        public IRegexPatternType this[string name] {
            get => _patterns[name];
            set => _patterns[name]=value;
        }

        public IRegexPatternType GetPattern(string name) {
            return _patterns.TryGetValue(name, out var pattern) ? pattern : null;
        }

        public bool TryGetPattern(string name, out IRegexPatternType pattern)
        {
            return _patterns.TryGetValue(name, out pattern);
        }

        public bool ContainsPattern(string name)
        {
            return _patterns.ContainsKey(name);
        }

        public void AddPattern(IRegexPatternType patternType) {
            _patterns.Add(patternType.Name, patternType);
        }


        public bool RemovePattern(string name)
        {
            return _patterns.Remove(name);
        }
    }
}