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
        private readonly Func<TEntity, object> _keySelectorFunc;
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
        public IndexedEntitySetDecorator(IEntitySet<TEntity> innerEntitySet, Func<EntityIndex<TEntity>> cacheFunc)
        {
	        _innerEntitySet = innerEntitySet;
	        _cacheFunc = cacheFunc;
		}

		/// <summary>
		/// Creates a new instance of <see cref="IndexedEntitySetDecorator{TEntity}"/>
		/// </summary>
		public IndexedEntitySetDecorator(IEntitySet<TEntity> innerEntitySet, Func<TEntity, object> keySelectorFunc)
		{
			_innerEntitySet = innerEntitySet;
			_keySelectorFunc = keySelectorFunc;
		}


		/// <inheritdoc />
		public async ValueTask<TEntity> GetAsync(object key, CancellationToken cancellationToken = default) {
	        if(Index.TryGet(key, out var entity)) {
		        return await _innerEntitySet.ReAttachAsync(entity);
	        }

	        entity = await _innerEntitySet.GetAsync(key, cancellationToken);
	        if(entity != null) {
		        Index.AddOrUpdate(entity);
	        } else
		        Index.MarkNonPresent(key);

	        return entity;
        }

        /// <inheritdoc />
        public async ValueTask<IEnumerable<TEntity>> GetRangeAsync(IEnumerable<object> keys, CancellationToken cancellationToken = default) {
	        var foundAll = Index.TryGetRange(keys, out var foundEntities, out var missingIds);
	        var list = foundEntities.AsCollection();
	        await _innerEntitySet.ReAttachRangeAsync(list);

	        if(foundAll)
		        return list;

	        var missing = missingIds.ToHashSet();
	        foreach(var entity in await _innerEntitySet.GetRangeAsync(missing, cancellationToken)) {
		        missing.Remove(entity.Id);
		        Index.AddOrUpdate(entity);
		        list.Add(entity);
	        }

	        Index.MarkRangeNonPresent(missing);
	        return list;
        }

        /// <inheritdoc />
        public async ValueTask<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default) {
	        var entities = (await _innerEntitySet.GetAllAsync(cancellationToken)).AsCollection();
	        Index.AddOrUpdateRange(entities);
	        return entities;
        }

        /// <inheritdoc />
        public ValueTask<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> filterExpression, CancellationToken cancellationToken = default) {
            return _innerEntitySet.WhereAsync(filterExpression, cancellationToken);
        }

        /// <inheritdoc />
        public ValueTask<TEntity> AddAsync(TEntity entity) {
            Index.AddOrUpdate(entity);
            return _innerEntitySet.AddAsync(entity);
        }

        /// <inheritdoc />
        public ValueTask<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities)
        {
	        var collection = entities.AsCollection();
	        Index.AddOrUpdateRange(collection);
	        return _innerEntitySet.AddRangeAsync(collection);
		}

        /// <inheritdoc />
        public ValueTask<TEntity> RemoveAsync(TEntity entity)
        {
	        Index.Remove(entity);
	        return _innerEntitySet.RemoveAsync(entity);
		}

        /// <inheritdoc />
        public ValueTask<IEnumerable<TEntity>> RemoveRangeAsync(IEnumerable<TEntity> entities) {
            var collection = entities.AsCollection();
            Index.RemoveRange(collection);
            return _innerEntitySet.RemoveRangeAsync(collection);
        }

        /// <inheritdoc />
        public ValueTask<TEntity> ReAttachAsync(TEntity entity)
        {
	        Index.AddOrUpdate(entity);
	        return _innerEntitySet.ReAttachAsync(entity);
		}

        /// <inheritdoc />
        public ValueTask<IEnumerable<TEntity>> ReAttachRangeAsync(IEnumerable<TEntity> entities) {
            var collection = entities.AsCollection();
	        Index.AddOrUpdateRange(collection);
			return _innerEntitySet.ReAttachRangeAsync(collection);
        }

        /// <inheritdoc />
        public IQuery<TEntity> Query() {
            object InterceptQueryResult(object result) {
                switch(result) {
                    case TEntity entity:
                        Index.AddOrUpdate(entity);
                        break;
                    case IEnumerable<TEntity> entities:
                        entities = entities.AsCollection();
                        Index.AddOrUpdateRange(entities);
                        break;
                }

                return result;
            }

            return _innerEntitySet.Query().InterceptQueryResult(InterceptQueryResult);
        }

        /// <summary>
        /// A callback function to create the underlying entity index
        /// </summary>
        /// <returns></returns>
        protected virtual EntityIndex<TEntity> OnCreateCache() {
            return _cacheFunc?.Invoke() ?? new EntityIndex<TEntity>(_keySelectorFunc);
        }
    }
}
