// ==================================================
// Copyright 2018(C) , DotLogix
// File:  RepeatedRegexPatternType.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  15.08.2018
// LastEdited:  31.08.2018
// ==================================================

#region
using System.Collections.Generic;
#endregion

namespace DotLogix.Core.Utils.Patterns {
    public class RepeatedRegexPatternType : RegexPatternType {
        public RepeatedRegexPatternType(string name, string defaultVariant, IReadOnlyDictionary<string, string> patternVariants) : base(name, defaultVariant, patternVariants) { }
        public RepeatedRegexPatternType(string name, string pattern) : base(name, pattern) { }

        public override bool TryGetRegexPattern(string variant, string[] args, out string pattern) {
            if(base.TryGetRegexPattern(variant, args, out pattern) == false)
                return false;

            if((args.Length == 0) || (PatternRange.TryParse(args[0], out var range) == false)) {
                pattern = string.Concat(pattern, "+?");
                return true;
            }

            pattern = string.Concat(pattern, range.ToRegexString());
            return true;
        }
    }
}
