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

namespace DotLogix.Core.Utils.Patterns; 

/// <inheritdoc />
public class RepeatedRegexPatternType : RegexPatternType {
    /// <summary>
    /// Creates a new instance of <see cref="RepeatedRegexPatternType"/>
    /// </summary>
    public RepeatedRegexPatternType(string name, string defaultVariant, IReadOnlyDictionary<string, string> patternVariants) : base(name, defaultVariant, patternVariants) { }
    /// <summary>
    /// Creates a new instance of <see cref="RepeatedRegexPatternType"/>
    /// </summary>
    public RepeatedRegexPatternType(string name, string pattern) : base(name, pattern) { }

    /// <inheritdoc />
    public override bool TryGetRegexPattern(string variant, string[] args, out string pattern) {
        if(base.TryGetRegexPattern(variant, args, out pattern) == false)
            return false;

        if((args.Length == 0) || (Range.TryParse(args[0], out var range) == false)) {
            pattern = string.Concat(pattern, "+?");
            return true;
        }

        pattern = string.Concat(pattern, range.ToRegexRange());
        return true;
    }
}