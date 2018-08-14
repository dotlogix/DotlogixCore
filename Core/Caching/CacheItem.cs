// ==================================================
// Copyright 2018(C) , DotLogix
// File:  CacheItem.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Core.Caching {
    /// <summary>
    ///     An item for a cache implementation
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class CacheItem<TKey, TValue> {
        public TKey Key { get; }
        public TValue Value { get; }
        public ICachePolicy Policy { get; }

        public CacheItem(TKey key, TValue value, ICachePolicy policy) {
            Key = key;
            Value = value;
            Policy = policy;
        }
    }
}
