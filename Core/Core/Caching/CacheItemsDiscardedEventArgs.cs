// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  CacheItemsDiscardedEventArgs.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 22.08.2020 13:51
// LastEdited:  26.09.2021 22:27
// ==================================================

#region
using System.Collections.Generic;
#endregion

namespace DotLogix.Core.Caching {
    /// <summary>
    /// An event args with the discarded items
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class CacheItemsDiscardedEventArgs<TKey, TValue> {
        /// <summary>
        /// The discarded items
        /// </summary>
        public IReadOnlyList<CacheItem<TKey, TValue>> DiscardedItems { get; }

        /// <summary>
        /// Creates a new instance of <see cref="CacheItemsDiscardedEventArgs{TKey,TValue}"/>
        /// </summary>
        /// <param name="discardedItems"></param>
        public CacheItemsDiscardedEventArgs(IReadOnlyList<CacheItem<TKey, TValue>> discardedItems) {
            DiscardedItems = discardedItems;
        }
    }
}
