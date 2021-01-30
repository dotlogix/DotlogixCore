using System;
using System.Collections.Generic;
using DotLogix.Architecture.Infrastructure.Queries.Queryable;
using DotLogix.Core.Caching;

namespace DotLogix.Architecture.Infrastructure.Queries.Interceptors {
    
    public abstract class CacheQueryResultInterceptor : IQueryInterceptor{
        private readonly ICache<object, object> _queryCache;
        private readonly object _cacheKey;
        private readonly Func<IQueryInterceptionContext, object> _getCacheKeyFunc;

        public bool IgnoreCache { get; set; }
        public bool PreserveContext { get; set; }
        public IReadOnlyCollection<object> DependsOn { get; set; }
        public IReadOnlyCollection<object> Dependencies { get; set; }
        public ICachePolicy CachePolicy { get; set; }

        public CacheQueryResultInterceptor(ICache<object, object> queryCache) {
            _queryCache = queryCache;
        }

        /// <summary>
        /// Get a unique cache key based on the provided interception context
        /// </summary>
        protected abstract object GetCacheKey(IQueryInterceptionContext context);

        /// <inheritdoc />
        public virtual bool BeforeExecute(IQueryInterceptionContext context) {
            if(IgnoreCache)
                return true;
            
            var key = _cacheKey ?? _getCacheKeyFunc(context);

            if (_queryCache.TryRetrieve(key, out var result)) {
                context.Result = result;
            }
            context.DisableQueryModification();
            return true;
        }

        /// <inheritdoc />
        public virtual bool AfterExecute(IQueryInterceptionContext context) {
            if (IgnoreCache || context.Result.IsUndefined)
                return true;

            var key = _cacheKey ?? _getCacheKeyFunc(context);
            
            var item = _queryCache.Store(key, context.Result.Value, CachePolicy, PreserveContext);
            if(Dependencies != null && Dependencies.Count > 0) {
                item.Children.Add(Dependencies);
            }
                
            if(DependsOn != null && DependsOn.Count > 0) {
                foreach(var dependencyKey in DependsOn) {
                    var parent = _queryCache.RetrieveOrCreateItem(dependencyKey, null, CachePolicy, false);
                    parent.Children.Add(key);
                }
            }
            return true;
        }

        /// <inheritdoc />
        public virtual bool HandleError(IQueryInterceptionContext context) {
            return true;
        }
    }
}
