// ==================================================
// Copyright 2018(C) , DotLogix
// File:  PatternParser.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  15.08.2018
// LastEdited:  31.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Core.Utils.Patterns {
    /// <summary>
    /// A parser for patterns
    /// </summary>
    public class PatternParser : ICloneable<PatternParser> {
        private static readonly Regex PatternRegex = new(@"<<(?<name>\w+)(?:\|(?<type>\w+)(?::(?<variant>\w+))?(?:\|(?<args>[^|>]+(?:\|[^|>]+)+))?)?>>");
        /// <summary>
        /// Patterns
        /// </summary>
        protected Dictionary<string, IRegexPatternType> Patterns { get; } = new(StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// The default pattern parser
        /// </summary>
        public static PatternParser Default { get; }

        /// <summary>
        /// Get a regex pattern type by name
        /// </summary>
        public IRegexPatternType this[string name] {
            get => Patterns[name];
            set => Patterns[name] = value;
        }

        static PatternParser() {
            var parser = new PatternParser();
            parser.AddPattern(new RegexPatternType("guid", "d", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) {
                {"n", "[0-9A-Fa-f]{32}"},
                {"d", "[0-9A-Fa-f]{8}-(?:[0-9A-Fa-f]{4}-){3}[0-9A-Fa-f]{12}"},
                {"b", "\\{[0-9A-Fa-f]{8}-(?:[0-9A-Fa-f]{4}-){3}[0-9A-Fa-f]{12}\\}"},
                {"p", "\\([0-9A-Fa-f]{8}-(?:[0-9A-Fa-f]{4}-){3}[0-9A-Fa-f]{12}\\)"}
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

        /// <summary>
        /// Get a regex pattern type by name
        /// </summary>
        public IRegexPatternType GetPattern(string name) {
            return Patterns.TryGetValue(name, out var pattern) ? pattern : null;
        }

        /// <summary>
        /// Try get a regex pattern type by name
        /// </summary>
        public bool TryGetPattern(string name, out IRegexPatternType pattern) {
            return Patterns.TryGetValue(name, out pattern);
        }

        /// <summary>
        /// Check if a regex pattern already exists
        /// </summary>
        public bool ContainsPattern(string name) {
            return Patterns.ContainsKey(name);
        }

        /// <summary>
        /// Add a regex pattern
        /// </summary>
        public void AddPattern(IRegexPatternType patternType) {
            Patterns.Add(patternType.Name, patternType);
        }
        /// <summary>
        /// Remove a regex pattern
        /// </summary>
        public bool RemovePattern(string name) {
            return Patterns.Remove(name);
        }

        /// <summary>
        /// Convert a pattern to a regex expression
        /// </summary>
        public string ToRegexPattern(string pattern) {
            return PatternRegex.Replace(pattern, ToRegexPattern);
        }

        private string ToRegexPattern(Match match) {
            var name = match.Groups["name"].Value;
            var type = match.Groups["type"].GetValueOrDefault("any");
            var variant = match.Groups["variant"].GetValueOrDefault();
            var args = match.Groups["args"].Value.Split('|');

            if(TryGetPattern(type, out var patternType) == false)
                throw new ArgumentException($"Pattern {name} with type {type} is not registered in default pattern parser");

            var succeed = patternType.TryGetRegexPattern(variant, args, out var pattern);
            return succeed ? string.Concat("(?<", name, ">", pattern, ")") : match.Value;
        }

        /// <summary>
        /// Extracts the pattern parameter information of a pattern
        /// </summary>
        public IEnumerable<PatternParameter> ExtractParameters(string pattern) {
            return PatternRegex.Matches(pattern).Cast<Match>().Select(ToParameter);
        }
        
        private PatternParameter ToParameter(Match match) {
            var name = match.Groups["name"].Value;
            var type = match.Groups["type"].GetValueOrDefault("any");
            var variant = match.Groups["variant"].GetValueOrDefault();
            var args = match.Groups["args"].Value.Split('|');

            if(TryGetPattern(type, out var patternType) == false)
                throw new ArgumentException($"Pattern {name} with type {type} is not registered in default pattern parser");

            var succeed = patternType.TryGetRegexPattern(variant, args, out var pattern);
            if(succeed) {
                return new PatternParameter {
                    Type = patternType,
                    Name = name,
                    Variant = variant,
                    Args = args,
                    Regex = pattern,
                    Offset = match.Index,
                    Count = match.Length
                };
            }
            
            return null;
        }

        /// <summary>
        /// Clone the pattern parser
        /// </summary>
        /// <returns></returns>
        public PatternParser Clone() {
            var parser = new PatternParser();
            foreach(var pattern in Patterns.Values)
                parser.AddPattern(pattern);
            return parser;
        }

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        object ICloneable.Clone() {
            return Clone();
        }
    }
}