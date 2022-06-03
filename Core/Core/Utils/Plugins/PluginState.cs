// ==================================================
// Copyright 2018(C) , DotLogix
// File:  PluginState.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Core.Utils.Plugins {
    /// <summary>
    /// The loading state of a plugin assembly
    /// </summary>
    public enum PluginState {
        /// <summary>
        /// None
        /// </summary>
        None,
        /// <summary>
        /// Loaded
        /// </summary>
        Loaded,
        /// <summary>
        /// Failed
        /// </summary>
        Failed
    }
}
