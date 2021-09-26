#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotLogix.Common.Features;
using DotLogix.Core.Collections;
using DotLogix.WebServices.Core;
using DotLogix.WebServices.Core.Errors;
using DotLogix.WebServices.Core.Extensions;
using DotLogix.WebServices.EntityFramework.Repositories;
#endregion

namespace DotLogix.WebServices.AspCore.Services {
    public class StaticDomainService<TEntity, TResponse> : ReadOnlyDomainService<TEntity, TResponse>
        where TEntity : class, IGuid, new()
        where TResponse : class, IGuid, new() {
        protected ICacheProvider CacheProvider { get; }

        public StaticDomainService(ICacheProvider cacheProvider, IGenericRepository<Guid, TEntity> repository)
            : base(repository) {
            CacheProvider = cacheProvider;
        }

        public override async Task<ICollection<TResponse>> GetAllAsync() {
            return await PreloadAsync();
        }

        public override async Task<TResponse> GetAsync(Guid guid) {
            var models = await PreloadAsync();
            var model = models.Get(guid);
            if(model == null) {
                throw new FilterNotFoundException(guid) {
                    ClrType = typeof(TResponse)
                };
            }
            return model;
        }

        public override async Task<ICollection<TResponse>> GetRangeAsync(IEnumerable<Guid> guids) {
            var models = await PreloadAsync();
            return models.Get(guids).ToList();
        }

        protected virtual async Task<KeyedCollection<Guid, TResponse>> PreloadAsync() {
            var cache = CacheProvider.GetOrCreateGlobalCache();
            var cacheKey = GetType();
            if(cache.Retrieve(cacheKey) is KeyedCollection<Guid, TResponse> collection) {
                return collection;
            }

            var entities = await Repository.GetAllAsync();
            var models = await ToModelsAsync(entities);
            collection = new KeyedCollection<Guid, TResponse>(m => m.Guid, models);
            cache.Store(cacheKey, collection);
            return collection;
        }
    }
}
