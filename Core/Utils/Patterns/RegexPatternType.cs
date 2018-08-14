// ==================================================
// Copyright 2018(C) , DotLogix
// File:  RegexPatternType.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
#endregion

namespace DotLogix.Core.Utils.Patterns {
    public class RegexPatternType : IRegexPatternType {
        protected string DefaultVariant { get; }
        protected IReadOnlyDictionary<string, string> PatternVariants { get; }

        public RegexPatternType(string name, string defaultVariant, IReadOnlyDictionary<string, string> patternVariants) {
            DefaultVariant = defaultVariant;
            PatternVariants = patternVariants;
            Name = name;
        }

        public RegexPatternType(string name, string pattern) {
            PatternVariants = new Dictionary<string, string> {{"", pattern}};
            DefaultVariant = "";
            Name = name;
        }

        public string Name { get; }

        public virtual string GetRegexPattern(string variant, string[] args) {
            return PatternVariants.TryGetValue(variant ?? DefaultVariant, out var pattern) ? pattern : null;
        }
    }
}
