// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IPluginDefinition.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Utils.Plugins {
    /// <summary>
    /// A generic definition for plugins
    /// </summary>
    public interface IPluginDefinition {
        /// <summary>
        /// The unique id of the plugin
        /// </summary>
        Guid Guid { get; }
        /// <summary>
        /// The name of the plugin
        /// </summary>
        string Name { get; }
    }
}