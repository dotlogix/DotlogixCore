using System;
using DotLogix.Core.Caching;

namespace DotLogix.WebServices.Core.Extensions {
    public static class CacheProviderExtensions {
        public static ICache<object, object> GetGlobalCache(this ICacheProvider cacheProvider) {
            return cacheProvider.Get("webservices.global");
        }
        
        public static ICache<object, object> GetOrCreateGlobalCache(this ICacheProvider cacheProvider, TimeSpan? delay = null) {
            return cacheProvider.GetOrCreate("webservices.global", delay ?? TimeSpan.FromMinutes(5));
        }
        
        public static ICache<object, object> ReplaceGlobalCache(this ICacheProvider cacheProvider, TimeSpan? delay = null) {
            return cacheProvider.Replace("webservices.global", delay ?? TimeSpan.FromMinutes(5));
        }
    }
}
