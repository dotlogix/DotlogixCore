// ==================================================
// Copyright 2018(C) , DotLogix
// File:  CacheItemsDiscardedEventArgs.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Collections.Generic;
#endregion

namespace DotLogix.Core.Caching {
    public class CacheItemsDiscardedEventArgs<TKey, TValue> {
        public IReadOnlyList<CacheItem<TKey, TValue>> DiscardedItems { get; }

        public CacheItemsDiscardedEventArgs(IReadOnlyList<CacheItem<TKey, TValue>> discardedItems) {
            DiscardedItems = discardedItems;
        }
    }
}
