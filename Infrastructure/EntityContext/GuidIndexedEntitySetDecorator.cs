// ==================================================
// Copyright 2018(C) , DotLogix
// File:  GuidIndexedEntitySetDecorator.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  04.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.Queries;
using DotLogix.Core.Extensions;
using DotLogix.Core.Utils;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityContext {
    public class GuidIndexedEntitySetDecorator<TEntity> : IEntitySet<TEntity> where TEntity : class, ISimpleEntity, new() {
        private readonly EntityCollection<TEntity> _cache;
        private readonly IEntitySet<TEntity> _innerEntitySet;

        public GuidIndexedEntitySetDecorator(IEntitySet<TEntity> baseEntitySet, EntityCollection<TEntity> cache = null) {
            _innerEntitySet = baseEntitySet;
            _cache = cache ?? new EntityCollection<TEntity>();
        }

        public void Add(TEntity entity) {
            _cache.AddOrUpdate(entity);
            _innerEntitySet.Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities) {
            var list = entities.AsList();
            _cache.AddOrUpdateRange(list);
            _innerEntitySet.AddRange(list);
        }

        public void Remove(TEntity entity) {
            _cache.Remove(entity);
            _innerEntitySet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities) {
            var list = entities.AsList();
            _cache.RemoveRange(list);
            _innerEntitySet.RemoveRange(list);
        }

        public void ReAttach(TEntity entity) {
            _innerEntitySet.ReAttach(entity);
        }

        public void ReAttachRange(IEnumerable<TEntity> entities) {
            _innerEntitySet.ReAttachRange(entities);
        }

        public IQuery<TEntity> Query() {
            return _innerEntitySet.Query();
        }

        public async Task<TEntity> GetAsync(int id, CancellationToken cancellationToken = default) {
            if(_cache.TryGet(id, out var entity)) {
                _innerEntitySet.ReAttach(entity);
                return entity;
            }

            entity = await _innerEntitySet.GetAsync(id, cancellationToken);
            if(entity != null)
                _cache.AddOrUpdate(entity);
            return entity;
        }

        public async Task<IEnumerable<TEntity>> GetRangeAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default) {
            var foundAll = _cache.TryGetRange(ids, out var foundEntities, out var missingIds);
            var list = foundEntities.AsList();
            _innerEntitySet.ReAttachRange(list);

            if(foundAll)
                return list;

            var dbEntities = (await _innerEntitySet.GetRangeAsync(missingIds, cancellationToken)).AsList();

            _cache.AddOrUpdateRange(dbEntities);
            list.AddRange(dbEntities);
            return list;
        }

        public async Task<TEntity> GetAsync(Guid guid, CancellationToken cancellationToken = default) {
            if (_cache.TryGet(guid, out var entity))
            {
                _innerEntitySet.ReAttach(entity);
                return entity;
            }

            entity = await _innerEntitySet.GetAsync(guid, cancellationToken);
            if(entity != null)
                _cache.AddOrUpdate(entity);
            else
                _cache.MarkNonPresent(guid);
            return entity;
        }

        public async Task<IEnumerable<TEntity>> GetRangeAsync(IEnumerable<Guid> guids, CancellationToken cancellationToken = default) {
            var foundAll = _cache.TryGetRange(guids, out var foundEntities, out var missingGuids);
            var list = foundEntities.AsList();
            _innerEntitySet.ReAttachRange(list);

            if (foundAll)
                return list;

            var missing = missingGuids.ToHashSet();
            foreach(var entity in await _innerEntitySet.GetRangeAsync(missing, cancellationToken)) {
                missing.Remove(entity.Guid);
                _cache.AddOrUpdate(entity);
                list.Add(entity);
            }
            _cache.MarkRangeNonPresent(missing);
            return list;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default) {
            var list = (await _innerEntitySet.GetAllAsync(cancellationToken)).AsList();
            _cache.AddOrUpdateRange(list);
            return list;
        }
    }
}
