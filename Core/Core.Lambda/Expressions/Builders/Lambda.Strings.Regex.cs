// ==================================================
// Copyright 2014-2022(C), DotLogix
// File:  Lambda.Strings.Regex.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 12.03.2022 18:51
// LastEdited:  12.03.2022 18:51
// ==================================================

using System.Text.RegularExpressions;

namespace DotLogix.Core.Expressions {
    public static partial class Lambdas {
        public static Lambda<bool> MatchesRegex(this Lambda<string> value, string regexPattern, RegexOptions options) {
            return value.CallStatic<string, string, RegexOptions, bool>(Regex.IsMatch, regexPattern, options);
        }
    }
}