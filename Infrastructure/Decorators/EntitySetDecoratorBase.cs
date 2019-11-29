// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EntitySetDecoratorBase.cs
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
using DotLogix.Core;
#endregion

namespace DotLogix.Architecture.Infrastructure.Decorators {
    /// <summary>
    /// A decorator for <see cref="IEntitySet{TEntity}"/> to intercept requests
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class EntitySetDecoratorBase<TEntity> : IEntitySet<TEntity> where TEntity : ISimpleEntity {
        /// <summary>
        /// The internal base entity set
        /// </summary>
        protected IEntitySet<TEntity> BaseEntitySet { get; }

        /// <summary>
        /// Creates a new instance of <see cref="EntitySetDecoratorBase{TEntity}"/>
        /// </summary>
        protected EntitySetDecoratorBase(IEntitySet<TEntity> baseEntitySet) {
	        BaseEntitySet = baseEntitySet;
        }

        /// <inheritdoc />
        public virtual ValueTask<TEntity> GetAsync(object key, CancellationToken cancellationToken = default)
        {
	        return BaseEntitySet.GetAsync(key, cancellationToken);
        }

        /// <inheritdoc />
        public virtual ValueTask<IEnumerable<TEntity>> GetRangeAsync(IEnumerable<object> keys,
	        CancellationToken cancellationToken = default)
        {
	        return BaseEntitySet.GetRangeAsync(keys, cancellationToken);
        }

        /// <inheritdoc />
        public virtual ValueTask<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
	        return BaseEntitySet.GetAllAsync(cancellationToken);
        }

        /// <inheritdoc />
        public virtual ValueTask<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> filterExpression,
	        CancellationToken cancellationToken = default)
        {
	        return BaseEntitySet.WhereAsync(filterExpression, cancellationToken);
        }

        /// <inheritdoc />
		public virtual ValueTask<TEntity> AddAsync(TEntity entity) {
            return BaseEntitySet.AddAsync(entity);
        }

        /// <inheritdoc />
        public virtual ValueTask<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities) {
            return BaseEntitySet.AddRangeAsync(entities);
        }

        /// <inheritdoc />
        public virtual ValueTask<TEntity> RemoveAsync(TEntity entity) {
            return BaseEntitySet.RemoveAsync(entity);
        }

        /// <inheritdoc />
        public virtual ValueTask<IEnumerable<TEntity>> RemoveRangeAsync(IEnumerable<TEntity> entities) {
            return BaseEntitySet.RemoveRangeAsync(entities);
        }

        /// <inheritdoc />
        public virtual ValueTask<TEntity> ReAttachAsync(TEntity entity) {
            return BaseEntitySet.ReAttachAsync(entity);
        }

        /// <inheritdoc />
        public virtual ValueTask<IEnumerable<TEntity>> ReAttachRangeAsync(IEnumerable<TEntity> entities) {
            return BaseEntitySet.ReAttachRangeAsync(entities);
        }

        /// <inheritdoc />
        public virtual IQuery<TEntity> Query()
        {
	        var query = BaseEntitySet.Query();
	        return query;
        }
    }
}
