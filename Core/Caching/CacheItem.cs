// ==================================================
// Copyright 2018(C) , DotLogix
// File:  CacheItem.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.02.2018
// LastEdited:  17.02.2018
// ==================================================

namespace DotLogix.Core.Caching {
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
