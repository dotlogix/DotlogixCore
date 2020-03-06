// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ValueAccessModes.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Reflection.Dynamics {
    /// <summary>
    /// Value accessing rights
    /// </summary>
    [Flags]
    public enum ValueAccessModes {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// Read
        /// </summary>
        Read = 1 << 0,
        /// <summary>
        /// Write
        /// </summary>
        Write = 1 << 1,
        /// <summary>
        /// Read or Write
        /// </summary>
        ReadWrite = Read | Write
    }
}
