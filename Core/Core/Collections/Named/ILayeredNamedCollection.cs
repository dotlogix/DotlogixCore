// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  ILayeredNamedCollection.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 22.08.2020 13:51
// LastEdited:  26.09.2021 22:27
// ==================================================

using System.Collections.Generic;

namespace DotLogix.Core.Collections {
    /// <summary>
    /// A settings collection with multiple layers
    /// </summary>
    public interface ILayeredNamedCollection : IReadOnlyNamedCollection {
        /// <summary>
        /// The layers of settings
        /// </summary>
        IEnumerable<IReadOnlyNamedCollection> Layers { get; }
        /// <summary>
        /// The topmost layer
        /// </summary>
        IReadOnlyNamedCollection CurrentLayer { get; }

        /// <summary>
        /// Add a new layer to the settings
        /// </summary>
        INamedCollection PushLayer();

        /// <summary>
        /// Removes the topmost layer from the stack
        /// </summary>
        /// <returns></returns>
        IReadOnlyNamedCollection PopLayer();
        /// <summary>
        /// Get the topmost layer but don't remove it
        /// </summary>
        /// <returns></returns>
        IReadOnlyNamedCollection PeekLayer();
    }
}