// ==================================================
// Copyright 2018(C) , DotLogix
// File:  VisibilityModifiers.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Reflection.Dynamics {
    /// <summary>
    /// Visibility modifiers for type members
    /// </summary>
    [Flags]
    public enum VisibilityModifiers {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// Private
        /// </summary>
        Private = 1 << 0,
        /// <summary>
        /// Protected
        /// </summary>
        Protected = 1 << 1,
        /// <summary>
        /// Internal
        /// </summary>
        Internal = 1 << 2,
        /// <summary>
        /// Protected internal
        /// </summary>
        ProtectedInternal = Protected | Internal,
        /// <summary>
        /// Public
        /// </summary>
        Public = 1 << 3,
        /// <summary>
        /// NonePublic Mask
        /// </summary>
        NonPublic = Private | Protected | Internal,
        /// <summary>
        /// Any
        /// </summary>
        Any = NonPublic | Public
    }
}