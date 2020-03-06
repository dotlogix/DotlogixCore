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
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.Attributes;
using DotLogix.Architecture.Infrastructure.Decorators;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Architecture.Infrastructure.Queries;
using DotLogix.Architecture.Infrastructure.Queries.Queryable;
using DotLogix.Core.Extensions;
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
        public IEntitySet<TEntity> EntitySet => _entitySet ?? (_entitySet = EntitySetProvider.UseSet<TEntity>());

        /// <summary>
        ///     The internal entity set provider
        /// </summary>
        protected IEntitySetProvider EntitySetProvider { get; }

        /// <summary>
        ///     Creates a new instance of <see cref="RepositoryBase{TEntity}" />
        /// </summary>
        protected RepositoryBase(IEntitySetProvider entitySetProvider) {
            EntitySetProvider = entitySetProvider;
        }

        /// <inheritdoc />
        public virtual ValueTask<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default) {
            var task = Query().ToEnumerableAsync(cancellationToken);
            return new ValueTask<IEnumerable<TEntity>>(task);
        }

        /// <inheritdoc />
        public virtual ValueTask<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> filterExpression, CancellationToken cancellationToken = default) {
            var task = Query().Where(filterExpression).ToEnumerableAsync(cancellationToken);
            return new ValueTask<IEnumerable<TEntity>>(task);
        }

        /// <inheritdoc />
        public virtual ValueTask<TEntity> AddAsync(TEntity entity) {
            return EntitySet.AddAsync(entity);
        }

        /// <inheritdoc />
        public virtual ValueTask<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities) {
            return EntitySet.AddRangeAsync(entities);
        }

        /// <inheritdoc />
        public virtual ValueTask<TEntity> RemoveAsync(TEntity entity) {
            return EntitySet.RemoveAsync(entity);
        }

        /// <inheritdoc />
        public virtual ValueTask<IEnumerable<TEntity>> RemoveRangeAsync(IEnumerable<TEntity> entities) {
            return EntitySet.RemoveRangeAsync(entities);
        }


        /// <summary>
        /// Create a linq style query to allow more advanced requests to the entity set
        /// </summary>
        protected virtual IQuery<TEntity> Query() {
            return EntitySet.Query();
        }
    }
}
