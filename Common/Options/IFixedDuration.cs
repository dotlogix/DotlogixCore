// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IFixedDuration.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Architecture.Common.Options {
    /// <summary>
    /// An interface to represent durations
    /// </summary>
    public interface IFixedDuration : IDuration {
        /// <summary>
        /// The end of the duration in universal time
        /// </summary>
        new DateTime UntilUtc { get; set; }
    }
}
