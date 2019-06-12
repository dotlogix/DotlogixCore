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
        /// <summary>
        /// The key
        /// </summary>
        public TKey Key { get; }

        /// <summary>
        /// The value
        /// </summary>
        public TValue Value { get; }

        /// <summary>
        /// The policy to check validity
        /// </summary>
        public ICachePolicy Policy { get; }

        /// <summary>
        /// Creates a new instance of <see cref="CacheItem{TKey,TValue}"/>
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The value</param>
        /// <param name="policy">The policy to check validity</param>
        public CacheItem(TKey key, TValue value, ICachePolicy policy) {
            Key = key;
            Value = value;
            Policy = policy;
        }
    }
}
