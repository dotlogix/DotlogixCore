// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  CacheItemDiscardedEventArgs.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 22.08.2020 13:51
// LastEdited:  26.09.2021 22:27
// ==================================================

using System.Collections.Generic;

namespace DotLogix.Core.Caching {
    /// <summary>
    /// Event args for a single discarded item
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class CacheItemDiscardedEventArgs<TKey, TValue> {
        /// <summary>
        /// The reason why the cache item was discarded
        /// </summary>
        public CacheItemDiscardReason Reason { get; }

        /// <summary>
        /// The reason source
        /// </summary>
        public CacheItemDiscardSource Source => Ancestors.Count > 0
            ? CacheItemDiscardSource.Ancestor
            : CacheItemDiscardSource.Self;

        /// <summary>
        /// The source elements if the item was discarded as a dependency
        /// </summary>
        public ICollection<CacheItemDiscardedEventArgs<TKey, TValue>> Ancestors { get; } = new List<CacheItemDiscardedEventArgs<TKey, TValue>>();

        /// <summary>
        /// The discarded item dependencies
        /// </summary>
        public ICollection<CacheItemDiscardedEventArgs<TKey, TValue>> Dependencies { get; } = new List<CacheItemDiscardedEventArgs<TKey, TValue>>();

        /// <summary>
        /// The discarded items
        /// </summary>
        public CacheItem<TKey, TValue> Item { get; }

        /// <inheritdoc />
        public CacheItemDiscardedEventArgs(CacheItem<TKey, TValue> item, CacheItemDiscardReason reason) {
            Reason = reason;
            Item = item;
        }
    }
}