// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IRegexPatternType.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.02.2018
// LastEdited:  17.02.2018
// ==================================================

namespace DotLogix.Core.Utils.Patterns {
    public interface IRegexPatternType {
        string Name { get; }
        string GetRegexPattern(string variant, string[] args);
    }
}
