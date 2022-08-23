// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IRegexPatternType.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Core.Utils.Patterns {
    /// <summary>
    /// A pattern type to match regex expressions
    /// </summary>
    public interface IRegexPatternType {
        /// <summary>
        /// The name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Tries to convert the arguments to a valid regex expression
        /// </summary>
        /// <param name="variant"></param>
        /// <param name="args"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        bool TryGetRegexPattern(string variant, string[] args, out string pattern);
    }
}