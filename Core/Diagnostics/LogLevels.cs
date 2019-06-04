// ==================================================
// Copyright 2018(C) , DotLogix
// File:  LogLevels.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Diagnostics {
    /// <summary>
    /// Log levels
    /// </summary>
    [Flags]
    public enum LogLevels {
        /// <summary>
        /// Trace messages
        /// </summary>
        Trace = 1,
        /// <summary>
        /// Debug messages
        /// </summary>
        Debug = 2,
        /// <summary>
        /// Info messages
        /// </summary>
        Info = 3,
        /// <summary>
        /// Warning messages
        /// </summary>
        Warning = 4,
        /// <summary>
        /// Error messages
        /// </summary>
        Error = 5,
        /// <summary>
        /// Critical messages
        /// </summary>
        Critical = 6,
        /// <summary>
        /// No logging
        /// </summary>
        Off = 7
    }
}
