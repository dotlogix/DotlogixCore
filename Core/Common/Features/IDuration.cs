// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IDuration.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Common.Features {
    /// <summary>
    ///     An interface to represent durations
    /// </summary>
    public interface IDuration {
        /// <summary>
        ///     The start of the duration in universal time
        /// </summary>
        DateTime FromUtc { get; set; }

        /// <summary>
        ///     The end of the duration in universal time
        /// </summary>
        DateTime? UntilUtc { get; set; }
    }
}
