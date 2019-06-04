// ==================================================
// Copyright 2018(C) , DotLogix
// File:  CacheExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Core.Caching;
#endregion

namespace DotLogix.Core.Extensions {
    /// <summary>
    /// A static class providing extension methods for <see cref="ICache{TKey,TValue}"/>
    /// </summary>
    public static class CacheExtensions {
        /// <summary>
        ///     Stores a value in a cache using a <see cref="ValidUntilCachePolicy" /> with the given timestamp
        /// </summary>
        /// <param name="cache">The cache</param>
        /// <param name="key">The key of the value</param>
        /// <param name="value">The value</param>
        /// <param name="validUntilUtc">The time when the value exceed</param>
        public static void Store<TKey, TValue>(this ICache<TKey, TValue> cache, TKey key, TValue value, DateTime validUntilUtc) {
            cache.Store(key, value, new ValidUntilCachePolicy(validUntilUtc));
        }

        /// <summary>
        ///     Stores a value in a cache using a <see cref="ValidUntilCachePolicy" /> with the given duration
        /// </summary>
        /// <param name="cache">The cache</param>
        /// <param name="key">The key of the value</param>
        /// <param name="value">The value</param>
        /// <param name="duration">The duration until the value exceeds</param>
        public static void Store<TKey, TValue>(this ICache<TKey, TValue> cache, TKey key, TValue value, TimeSpan duration) {
            cache.Store(key, value, new ValidUntilCachePolicy(duration));
        }
    }
}
