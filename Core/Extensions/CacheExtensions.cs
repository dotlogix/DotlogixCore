using System;
using DotLogix.Core.Caching;

namespace DotLogix.Core.Extensions
{
    public static class CacheExtensions {
        public static void Store<TKey, TValue>(this ICache<TKey, TValue> cache, TKey key, TValue value, DateTime validUntilUtc) {
            cache.Store(key, value, new ValidUntilCachePolicy(validUntilUtc));
        }

        public static void Store<TKey, TValue>(this ICache<TKey, TValue> cache, TKey key, TValue value, TimeSpan duration) {
            cache.Store(key, value, new ValidUntilCachePolicy(duration));
        }
    }
}
