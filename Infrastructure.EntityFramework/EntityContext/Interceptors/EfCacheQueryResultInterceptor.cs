using System;
using DotLogix.Architecture.Infrastructure.Queries;
using DotLogix.Architecture.Infrastructure.Queries.Interceptors;
using DotLogix.Core.Caching;

namespace DotLogix.Architecture.Infrastructure.EntityFramework.EntityContext {
    
    public class EfCacheQueryResultInterceptor : CacheQueryResultInterceptor {
        private readonly Func<IQueryInterceptionContext, object> _getCacheKeyFunc;
        private readonly object _cacheKey;

        /// <inheritdoc />
        public EfCacheQueryResultInterceptor(ICache<object, object> queryCache, Func<IQueryInterceptionContext, object> cacheKeyFunc) : base(queryCache) {
            _getCacheKeyFunc = cacheKeyFunc ?? throw new ArgumentNullException(nameof(cacheKeyFunc));
        }

        /// <inheritdoc />
        public EfCacheQueryResultInterceptor(ICache<object, object> queryCache, object cacheKey) : base(queryCache) {
            _cacheKey = cacheKey ?? throw new ArgumentNullException(nameof(cacheKey));
        }

        /// <inheritdoc />
        protected override object GetCacheKey(IQueryInterceptionContext context) {
            return _cacheKey ?? _getCacheKeyFunc.Invoke(context);
        }
    }
}
