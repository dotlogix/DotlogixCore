// ==================================================
// Copyright 2018(C) , DotLogix
// File:  AccessModifiers.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Reflection.Dynamics {
    /// <summary>
    /// Modifiers for type members
    /// </summary>
    [Flags]
    public enum AccessModifiers {
        /// <summary>
        /// No modifier
        /// </summary>
        None = 0,
        /// <summary>
        /// Static
        /// </summary>
        Static = 1 << 1,
        /// <summary>
        /// Const
        /// </summary>
        Const = 1 << 2,
        /// <summary>
        /// Nested
        /// </summary>
        Nested = 1 << 3,
        /// <summary>
        /// Abstract
        /// </summary>
        Abstract = 1 << 4,
        /// <summary>
        /// Virtual
        /// </summary>
        Virtual = 1 << 5,
        /// <summary>
        /// Sealed
        /// </summary>
        Sealed = 1 << 6
    }
}