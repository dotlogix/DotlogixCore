// ==================================================
// Copyright 2018(C) , DotLogix
// File:  RepositoryBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Architecture.Infrastructure.Queries;
using DotLogix.Architecture.Infrastructure.Queries.Queryable;
#endregion

namespace DotLogix.Architecture.Infrastructure.Repositories {
    /// <summary>
    ///     A basic generic repository providing crud functionality
    /// </summary>
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class, new() {
        private IEntitySet<TEntity> _entitySet;

        /// <summary>
        ///     The undecorated and unmodified entity set
        /// </summary>
        public IEntitySet<TEntity> EntitySet => _entitySet ?? (_entitySet = EntityContext.UseSet<TEntity>());

        /// <summary>
        ///     The current entity context
        /// </summary>
        protected IEntityContext EntityContext { get; }

        /// <summary>
        ///     Creates a new instance of <see cref="RepositoryBase{TEntity}" />
        /// </summary>
        protected RepositoryBase(IEntityContext entityContext) {
            EntityContext = entityContext;
        }

        /// <inheritdoc />
        public virtual Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default) {
            return Query().ToEnumerableAsync(cancellationToken);
        }

        /// <inheritdoc />
        public virtual Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> filterExpression, CancellationToken cancellationToken = default) {
            return Query().Where(filterExpression).ToEnumerableAsync(cancellationToken);
        }

        /// <inheritdoc />
        public virtual Task<IEnumerable<TEntity>> WhereAsync(IQueryModifier<TEntity> filter, CancellationToken cancellationToken = default) {
            return Query(filter).ToEnumerableAsync(cancellationToken);
        }

        /// <inheritdoc />
        public virtual Task<TEntity> AddAsync(TEntity entity) {
            return EntitySet.AddAsync(entity);
        }

        /// <inheritdoc />
        public virtual Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities) {
            return EntitySet.AddRangeAsync(entities);
        }

        /// <inheritdoc />
        public virtual Task<TEntity> RemoveAsync(TEntity entity) {
            return EntitySet.RemoveAsync(entity);
        }

        /// <inheritdoc />
        public virtual Task<IEnumerable<TEntity>> RemoveRangeAsync(IEnumerable<TEntity> entities) {
            return EntitySet.RemoveRangeAsync(entities);
        }


        /// <summary>
        /// Create a linq style query to allow more advanced requests to the entity set
        /// </summary>
        protected virtual IQuery<TEntity> Query() {
            return EntitySet.Query();
        }

        /// <summary>
        /// Create a linq style query to allow more advanced requests to the entity set
        /// </summary>
        protected virtual IQuery<TEntity> Query(params IQueryModifier<TEntity>[] filters)
        {
            return EntitySet.Query(filters);
        }
    }
}
