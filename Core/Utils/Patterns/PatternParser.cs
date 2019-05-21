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
using DotLogix.Core.Interfaces;
#endregion

namespace DotLogix.Core.Utils.Patterns {
    public class PatternParser : ICloneable<PatternParser> {
        private static readonly Regex PatternRegex = new Regex(@"<<(?<name>\w+)(?:\|(?<type>\w+)(?::(?<variant>\w+))?(?:\|(?<args>[^|>]+(?:\|[^|>]+)+))?)?>>");
        protected Dictionary<string, IRegexPatternType> Patterns { get; } = new Dictionary<string, IRegexPatternType>(StringComparer.OrdinalIgnoreCase);
        public static PatternParser Default { get; }

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

        public IRegexPatternType GetPattern(string name) {
            return Patterns.TryGetValue(name, out var pattern) ? pattern : null;
        }

        public bool TryGetPattern(string name, out IRegexPatternType pattern) {
            return Patterns.TryGetValue(name, out pattern);
        }

        public bool ContainsPattern(string name) {
            return Patterns.ContainsKey(name);
        }

        public void AddPattern(IRegexPatternType patternType) {
            Patterns.Add(patternType.Name, patternType);
        }

        public bool RemovePattern(string name) {
            return Patterns.Remove(name);
        }

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
