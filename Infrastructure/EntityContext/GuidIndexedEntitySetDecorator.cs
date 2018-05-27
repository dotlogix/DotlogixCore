using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.Queries;
using DotLogix.Core.Extensions;

namespace DotLogix.Architecture.Infrastructure.EntityContext {
    public class GuidIndexedEntitySetDecorator<TEntity> : IEntitySet<TEntity> where TEntity : class, ISimpleEntity
    {
        private readonly IEntitySet<TEntity> _innerEntitySet;
        private readonly EntityCollection<TEntity> _cache;

        public GuidIndexedEntitySetDecorator(IEntitySet<TEntity> baseEntitySet, EntityCollection<TEntity> cache = null) {
            _innerEntitySet = baseEntitySet;
            _cache = cache ?? new EntityCollection<TEntity>();
        }

        public void Add(TEntity entity) {
            _cache.AddOrUpdate(entity);
            _innerEntitySet.Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            var list = entities.AsList();
            _cache.AddOrUpdateRange(list);
            _innerEntitySet.AddRange(list);
        }

        public void Remove(TEntity entity) {
            _cache.Remove(entity);
            _innerEntitySet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            var list = entities.AsList();
            _cache.RemoveRange(list);
            _innerEntitySet.RemoveRange(list);
        }

        public IQuery<TEntity> Query()
        {
            return _innerEntitySet.Query();
        }

        public async Task<TEntity> GetAsync(int id, CancellationToken cancellationToken = default(CancellationToken)) {
            if(_cache.TryGet(id, out var entity))
                return entity;

            entity = await _innerEntitySet.GetAsync(id, cancellationToken);
            if(entity != null)
                _cache.AddOrUpdate(entity);
            return entity;
        }

        public async Task<IEnumerable<TEntity>> GetRangeAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default(CancellationToken)) {
            if(_cache.TryGetRange(ids, out var foundEntities, out var missingIds))
                return foundEntities;

            var list = foundEntities.AsList();
            var dbEntities = (await _innerEntitySet.GetRangeAsync(missingIds, cancellationToken)).AsList();

            _cache.AddOrUpdateRange(dbEntities);
            list.AddRange(dbEntities);
            return list;
        }

        public async Task<TEntity> GetAsync(Guid guid, CancellationToken cancellationToken = default(CancellationToken)) {
            if(_cache.TryGet(guid, out var entity))
                return entity;

            entity = await _innerEntitySet.GetAsync(guid, cancellationToken);
            if (entity != null)
                _cache.AddOrUpdate(entity);
            else
                _cache.MarkNonPresent(guid);
            return entity;
        }

        public async Task<IEnumerable<TEntity>> GetRangeAsync(IEnumerable<Guid> guids, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_cache.TryGetRange(guids, out var foundEntities, out var missingGuids))
                return foundEntities;

            var missing = missingGuids.ToHashSet();

            var list = foundEntities.AsList();
            foreach(var entity in await _innerEntitySet.GetRangeAsync(missing, cancellationToken)) {
                missing.Remove(entity.Guid);
                _cache.AddOrUpdate(entity);
                list.Add(entity);
            }
            _cache.MarkRangeNonPresent(missing);
            return list;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken)) {
            var list = (await _innerEntitySet.GetAllAsync(cancellationToken)).AsList();
            _cache.AddOrUpdateRange(list);
            return list;
        }
    }
}