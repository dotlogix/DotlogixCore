// ==================================================
// Copyright 2019(C) , DotLogix
// File:  ILayeredCollection.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.12.2018
// LastEdited:  07.02.2019
// ==================================================

#region
using System.Collections.Generic;
#endregion

namespace DotLogix.Core.Collections {
    /// <summary>
    /// A collection with multiple layers, useful for stacked collections
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TCollection"></typeparam>
    public interface ILayeredCollection<TValue, out TCollection> : ICollection<TValue> where TCollection : ICollection<TValue> {
        /// <summary>
        /// The layers of collections
        /// </summary>
        IEnumerable<TCollection> Layers { get; }
        /// <summary>
        /// The topmost layer
        /// </summary>
        TCollection CurrentLayer { get; }

        /// <summary>
        /// Add a new layer to the collection
        /// </summary>
        void PushLayer();
        /// <summary>
        /// Removes the topmost layer from the stack
        /// </summary>
        /// <returns></returns>
        TCollection PopLayer();
        /// <summary>
        /// Get the topmost layer but don't remove it
        /// </summary>
        /// <returns></returns>
        TCollection PeekLayer();
    }
}
