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
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Architecture.Infrastructure.Queries;
using DotLogix.Architecture.Infrastructure.Queries.Queryable;
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Architecture.Infrastructure.Decorators {
    /// <summary>
    /// A decorator class for <see cref="IEntitySet{TEntity}"/> to allow cached random access to already loaded entities
    /// </summary>
    public class IndexedEntitySetDecorator<TEntity> : IEntitySet<TEntity> where TEntity : class, ISimpleEntity, new() {
        private readonly Func<EntityIndex<TEntity>> _cacheFunc;
        private readonly IEntitySet<TEntity> _innerEntitySet;
        private EntityIndex<TEntity> _index;

        /// <summary>
        /// The internal entity index
        /// </summary>
        public EntityIndex<TEntity> Index => _index ?? (_index = OnCreateCache());

        /// <summary>
        /// Creates a new instance of <see cref="IndexedEntitySetDecorator{TEntity}"/>
        /// </summary>
        public IndexedEntitySetDecorator(IEntitySet<TEntity> innerEntitySet, EntityIndex<TEntity> index = null) {
            _index = index;
            _innerEntitySet = innerEntitySet;
        }

        /// <summary>
        /// Creates a new instance of <see cref="IndexedEntitySetDecorator{TEntity}"/>
        /// </summary>
        public IndexedEntitySetDecorator(IEntitySet<TEntity> innerEntitySet, Func<EntityIndex<TEntity>> cacheFunc) {
            _innerEntitySet = innerEntitySet;
            _cacheFunc = cacheFunc;
        }


        /// <inheritdoc />
        public Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> filterExpression, CancellationToken cancellationToken = default) {
            return _innerEntitySet.WhereAsync(filterExpression, cancellationToken);
        }

        /// <inheritdoc />
        public void Add(TEntity entity) {
            Index.ById.AddOrUpdate(entity);
            Index.ByGuid.AddOrUpdate(entity);
            _innerEntitySet.Add(entity);
        }

        /// <inheritdoc />
        public void AddRange(IEnumerable<TEntity> entities) {
            entities = entities.AsCollection();
            Index.ById.AddOrUpdateRange(entities);
            Index.ByGuid.AddOrUpdateRange(entities);
            _innerEntitySet.AddRange(entities);
        }

        /// <inheritdoc />
        public void Remove(TEntity entity) {
            Index.ById.TryRemove(entity);
            Index.ByGuid.TryRemove(entity);
            _innerEntitySet.Remove(entity);
        }

        /// <inheritdoc />
        public void RemoveRange(IEnumerable<TEntity> entities) {
            entities = entities.AsCollection();
            Index.ById.TryRemoveRange(entities);
            Index.ByGuid.TryRemoveRange(entities);
            _innerEntitySet.RemoveRange(entities);
        }

        /// <inheritdoc />
        public void ReAttach(TEntity entity) {
            _innerEntitySet.ReAttach(entity);
        }

        /// <inheritdoc />
        public void ReAttachRange(IEnumerable<TEntity> entities) {
            entities = entities.AsCollection();
            _innerEntitySet.ReAttachRange(entities);
        }

        /// <inheritdoc />
        public IQuery<TEntity> Query() {
            object InterceptQueryResult(object result) {
                switch(result) {
                    case TEntity entity:
                        Index.ById.AddOrUpdate(entity);
                        Index.ByGuid.AddOrUpdate(entity);
                        break;
                    case IEnumerable<TEntity> entities:
                        entities = entities.AsCollection();
                        Index.ById.AddOrUpdateRange(entities);
                        Index.ByGuid.AddOrUpdateRange(entities);
                        break;
                }

                return result;
            }

            return _innerEntitySet.Query().InterceptQueryResult(InterceptQueryResult);
        }

        /// <inheritdoc />
        public async Task<TEntity> GetAsync(int id, CancellationToken cancellationToken = default) {
            if(Index.ById.TryGet(id, out var entity)) {
                _innerEntitySet.ReAttach(entity);
                return entity;
            }

            entity = await _innerEntitySet.GetAsync(id, cancellationToken);
            if(entity != null) {
                Index.ById.AddOrUpdate(entity);
                Index.ByGuid.AddOrUpdate(entity);
            } else
                Index.ById.MarkNonPresent(id);

            return entity;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TEntity>> GetRangeAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default) {
            var foundAll = Index.ById.TryGetRange(ids, out var foundEntities, out var missingIds);
            var list = foundEntities.AsCollection();
            _innerEntitySet.ReAttachRange(list);

            if(foundAll)
                return list;

            var missing = missingIds.ToHashSet();
            foreach(var entity in await _innerEntitySet.GetRangeAsync(missing, cancellationToken)) {
                missing.Remove(entity.Id);
                Index.ById.AddOrUpdate(entity);
                Index.ByGuid.AddOrUpdate(entity);
                list.Add(entity);
            }

            Index.ById.MarkRangeNonPresent(missing);
            return list;
        }

        /// <inheritdoc />
        public async Task<TEntity> GetAsync(Guid guid, CancellationToken cancellationToken = default) {
            if(Index.ByGuid.TryGet(guid, out var entity)) {
                _innerEntitySet.ReAttach(entity);
                return entity;
            }

            entity = await _innerEntitySet.GetAsync(guid, cancellationToken);
            if(entity != null) {
                Index.ById.AddOrUpdate(entity);
                Index.ByGuid.AddOrUpdate(entity);
            } else
                Index.ByGuid.MarkNonPresent(guid);

            return entity;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TEntity>> GetRangeAsync(IEnumerable<Guid> guids, CancellationToken cancellationToken = default) {
            var foundAll = Index.ByGuid.TryGetRange(guids, out var foundEntities, out var missingGuids);
            var list = foundEntities.AsCollection();
            _innerEntitySet.ReAttachRange(list);

            if(foundAll)
                return list;

            var missing = missingGuids.ToHashSet();
            foreach(var entity in await _innerEntitySet.GetRangeAsync(missing, cancellationToken)) {
                missing.Remove(entity.Guid);
                Index.ById.AddOrUpdate(entity);
                Index.ByGuid.AddOrUpdate(entity);
                list.Add(entity);
            }

            Index.ByGuid.MarkRangeNonPresent(missing);
            return list;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default) {
            var entities = (await _innerEntitySet.GetAllAsync(cancellationToken)).AsCollection();
            Index.ById.AddOrUpdateRange(entities);
            Index.ByGuid.AddOrUpdateRange(entities);
            return entities;
        }

        /// <summary>
        /// A callback function to create the underlying entity index
        /// </summary>
        /// <returns></returns>
        protected virtual EntityIndex<TEntity> OnCreateCache() {
            return _cacheFunc?.Invoke() ?? new EntityIndex<TEntity>();
        }
    }
}
