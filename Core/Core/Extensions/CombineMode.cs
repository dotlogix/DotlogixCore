// ==================================================
// Copyright 2018(C) , DotLogix
// File:  CombineMode.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Core.Extensions {
    /// <summary>
    /// Combination modes of two sequences
    /// </summary>
    public enum CombineMode {
        /// <summary>
        /// Take all of a sequence than continue to the next
        /// </summary>
        Sequential,
        /// <summary>
        /// Take one of each sequence than return
        /// </summary>
        RoundRobin,
        /// <summary>
        /// Shuffle all elements in the sequence
        /// </summary>
        Shuffled
    }
}