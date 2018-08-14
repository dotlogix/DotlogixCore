// ==================================================
// Copyright 2018(C) , DotLogix
// File:  PatternParser.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
#endregion

namespace DotLogix.Core.Utils.Patterns {
    public class PatternParser {
        private readonly Dictionary<string, IRegexPatternType> _patterns = new Dictionary<string, IRegexPatternType>(StringComparer.OrdinalIgnoreCase);
        public static PatternParser Default { get; }

        public IRegexPatternType this[string name] {
            get => _patterns[name];
            set => _patterns[name] = value;
        }

        static PatternParser() {
            var parser = new PatternParser();
            parser.AddPattern(new RegexPatternType("guid", "d", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) {
                                                                                                                                     {"n", "[0-9A-Fa-f]{32}"},
                                                                                                                                     {"d", "[0-9A-Fa-f]{8}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{12}"},
                                                                                                                                     {"b", "\\{[0-9A-Fa-f]{8}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{12}\\}"},
                                                                                                                                     {"p", "\\([0-9A-Fa-f]{8}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{12}\\)"}
                                                                                                                                 }));
            parser.AddPattern(new RepeatedRegexPatternType("any", "."));
            parser.AddPattern(new RepeatedRegexPatternType("number", "\\d"));
            parser.AddPattern(new RepeatedRegexPatternType("hex", "[0-9a-fA-F]"));
            parser.AddPattern(new RepeatedRegexPatternType("word", "", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) {
                                                                                                                                            {"", "\\w"},
                                                                                                                                            {"s", "[\\w\\s]"}
                                                                                                                                        }));
            parser.AddPattern(new RepeatedRegexPatternType("letter", "", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) {
                                                                                                                                              {"", "[a-zA-Z]"},
                                                                                                                                              {"u", "[A-Z]"},
                                                                                                                                              {"l", "[a-z]"}
                                                                                                                                          }));
            Default = parser;
        }

        public IRegexPatternType GetPattern(string name) {
            return _patterns.TryGetValue(name, out var pattern) ? pattern : null;
        }

        public bool TryGetPattern(string name, out IRegexPatternType pattern) {
            return _patterns.TryGetValue(name, out pattern);
        }

        public bool ContainsPattern(string name) {
            return _patterns.ContainsKey(name);
        }

        public void AddPattern(IRegexPatternType patternType) {
            _patterns.Add(patternType.Name, patternType);
        }


        public bool RemovePattern(string name) {
            return _patterns.Remove(name);
        }
    }
}
