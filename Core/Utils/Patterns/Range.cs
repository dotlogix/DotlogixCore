// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Range.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  01.07.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Text.RegularExpressions;
#endregion

namespace DotLogix.Core.Utils.Patterns {
    /// <summary>
    /// A Range
    /// </summary>
    public struct Range {
        private static readonly char[] Enclosures = {'(', '[', '{', ')', ']', '}'};
        private static readonly Regex RangeRegex = new Regex("^(?:(?<exact>-?\\d+)|(?<min>-?\\d)?\\.\\.(?<max>-?\\d))?$", RegexOptions.Compiled);

        /// <summary>
        /// The min value
        /// </summary>
        public int? Min;

        /// <summary>
        /// Tha max value
        /// </summary>
        public int? Max;

        /// <summary>
        /// Creates a new <see cref="Range"/>
        /// </summary>
        public Range(int? min = null, int? max = null) {
            Min = min;
            Max = max;
        }

        /// <summary>
        /// Creates a regex range statement
        /// </summary>
        /// <param name="onAny"></param>
        /// <returns></returns>
        public string ToRegexRange(string onAny = "*") {
            if(Max.HasValue && Min.HasValue)
                return Max.Value == Min.Value ? $"{{{Min.Value}}}" : $"{{{Min.Value},{Max.Value}}}";
            if(Max.HasValue)
                return $"{{0,{Max.Value}}}";
            if(Min.HasValue)
                return $"{{{Min.Value},}}";
            return onAny;
        }

        /// <summary>
        /// Creates a parseable representation of the range
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            if(Max.HasValue && Min.HasValue)
                return $"({Max.Value}..{Min.Value})";
            if(Max.HasValue)
                return $"(..{Max.Value})";
            if(Min.HasValue)
                return $"({Min.Value}..)";
            return "(..)";
        }

        /// <summary>
        /// Parse
        /// </summary>
        public static Range Parse(string value) {
            if(TryParse(value, out var range))
                return range;
            throw new ArgumentException("Value is not a valid range statement", nameof(value));
        }

        /// <summary>
        /// Try to parse a range
        /// </summary>
        /// <param name="value"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public static bool TryParse(string value, out Range range) {
            range = default;
            if(string.IsNullOrWhiteSpace(value))
                return false;

            var index = Array.IndexOf(Enclosures, value[0], 0, 3);
            Match match;
            if(index >= 0) {
                if(Enclosures[index + 3] == value[value.Length - 1])
                    match = RangeRegex.Match(value, 1, value.Length - 2);
                else
                    return false;
            } else
                match = RangeRegex.Match(value);

            if(match.Success == false)
                return false;

            var matchGroup = match.Groups["exact"];
            if(matchGroup.Success) {
                var exact = int.Parse(matchGroup.Value);
                range.Min = exact;
                range.Max = exact;
                return true;
            }

            matchGroup = match.Groups["min"];
            if(matchGroup.Success)
                range.Min = int.Parse(matchGroup.Value);

            matchGroup = match.Groups["max"];
            if(matchGroup.Success)
                range.Max = int.Parse(matchGroup.Value);
            return true;
        }
    }
}
