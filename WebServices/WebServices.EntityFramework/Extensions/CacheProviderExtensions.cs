using System;
using DotLogix.Core.Caching;
using DotLogix.WebServices.Core;

namespace DotLogix.WebServices.EntityFramework.Extensions {
    public static class CacheProviderExtensions {
        public static ICache<object, object> GetEntityCache(this ICacheProvider cacheProvider) {
            return cacheProvider.Get("webservices.entities");
        }
        
        public static ICache<object, object> GetOrCreateEntityCache(this ICacheProvider cacheProvider, TimeSpan? delay = null) {
            return cacheProvider.GetOrCreate("webservices.entities", delay ?? TimeSpan.FromMinutes(5));
        }
        
        public static ICache<object, object> ReplaceEntityCache(this ICacheProvider cacheProvider, TimeSpan? delay = null) {
            return cacheProvider.Replace("webservices.entities", delay ?? TimeSpan.FromMinutes(5));
        }
    }
}
