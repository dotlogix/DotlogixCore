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
    /// <inheritdoc />
    public class RegexPatternType : IRegexPatternType {
        /// <summary>
        /// The default regex variant
        /// </summary>
        protected string DefaultVariant { get; }

        /// <summary>
        /// The pattern variants
        /// </summary>
        protected IReadOnlyDictionary<string, string> PatternVariants { get; }

        /// <summary>
        /// Creates an instance of <see cref="RegexPatternType"/>
        /// </summary>
        /// <param name="name">The type name</param>
        /// <param name="defaultVariant">The default variant</param>
        /// <param name="patternVariants">The available variants</param>
        public RegexPatternType(string name, string defaultVariant, IReadOnlyDictionary<string, string> patternVariants) {
            DefaultVariant = defaultVariant;
            PatternVariants = patternVariants;
            Name = name;
        }

        /// <summary>
        /// Creates an instance of <see cref="RegexPatternType"/>
        /// </summary>
        /// <param name="name">The type name</param>
        /// <param name="pattern">The regex pattern</param>
        public RegexPatternType(string name, string pattern) {
            PatternVariants = new Dictionary<string, string> {{"", pattern}};
            DefaultVariant = "";
            Name = name;
        }


        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public virtual bool TryGetRegexPattern(string variant, string[] args, out string pattern) {
            return PatternVariants.TryGetValue(variant ?? DefaultVariant, out pattern);
        }
    }
}
