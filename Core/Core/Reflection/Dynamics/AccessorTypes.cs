// ==================================================
// Copyright 2018(C) , DotLogix
// File:  AccessorTypes.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Reflection.Dynamics {
    /// <summary>
    /// Types of value accessors
    /// </summary>
    [Flags]
    public enum AccessorTypes {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// Property
        /// </summary>
        Property = 1 << 0,
        /// <summary>
        /// Field
        /// </summary>
        Field = 1 << 1,
        /// <summary>
        /// Any
        /// </summary>
        Any = Property | Field
    }
}
