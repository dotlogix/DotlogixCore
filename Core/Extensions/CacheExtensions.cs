// ==================================================
// Copyright 2018(C) , DotLogix
// File:  CacheExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using DotLogix.Core.Caching;
#endregion

namespace DotLogix.Core.Extensions {
    public static class CacheExtensions {
        public static void Store<TKey, TValue>(this ICache<TKey, TValue> cache, TKey key, TValue value, DateTime validUntilUtc) {
            cache.Store(key, value, new ValidUntilCachePolicy(validUntilUtc));
        }

        public static void Store<TKey, TValue>(this ICache<TKey, TValue> cache, TKey key, TValue value, TimeSpan duration) {
            cache.Store(key, value, new ValidUntilCachePolicy(duration));
        }
    }
}
