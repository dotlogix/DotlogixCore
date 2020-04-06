// ==================================================
// Copyright 2018(C) , DotLogix
// File:  CacheItemsDiscardedEventArgs.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
#endregion

namespace DotLogix.Core.Caching {
    /// <summary>
    /// EventArgs used for discarded cache items
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class CacheItemsDiscardedEventArgs<TKey, TValue> {
        /// <summary>
        /// The reason why the cache items were discarded
        /// </summary>
        public CacheItemDiscardReason Reason { get; }

        /// <summary>
        /// The discarded items
        /// </summary>
        public IReadOnlyList<CacheItemDiscardedEventArgs<TKey, TValue>> DiscardedItems { get; }

        /// <summary>
        /// Creates a new instance of <see cref="CacheItemsDiscardedEventArgs{TKey,TValue}"/>
        /// </summary>
        /// <param name="discardedItems"></param>
        /// <param name="reason"></param>
        public CacheItemsDiscardedEventArgs(IReadOnlyList<CacheItemDiscardedEventArgs<TKey, TValue>> discardedItems, CacheItemDiscardReason reason = CacheItemDiscardReason.Expired) {
            DiscardedItems = discardedItems;
            Reason = reason;
        }
    }
}
