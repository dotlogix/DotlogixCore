using System;
using DotLogix.Core.Caching;

namespace DotLogix.Core.Extensions
{
    public static class CacheExtensions {
        public static void Store<TKey>(this ICache<TKey> cache, TKey key, object value, DateTime validUntilUtc) {
            cache.Store(key, value, new ValidUntilCachePolicy(validUntilUtc));
        }

        public static void Store<TKey>(this ICache<TKey> cache, TKey key, object value, TimeSpan duration) {
            cache.Store(key, value, new ValidUntilCachePolicy(duration));
        }
    }
}
