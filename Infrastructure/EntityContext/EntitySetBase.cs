// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EntitySetBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  04.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.Queries;
using DotLogix.Architecture.Infrastructure.Queries.Queryable;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityContext {
    /// <summary>
    /// A base implementation of an entity set
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class EntitySetBase<TEntity> : IEntitySet<TEntity> where TEntity : ISimpleEntity {
        /// <summary>
        /// Get a single entity by id
        /// </summary>
        public virtual Task<TEntity> GetAsync(int id, CancellationToken cancellationToken = default) {
            return Query().Where(e => e.Id == id).FirstOrDefaultAsync(cancellationToken);
        }


        /// <inheritdoc />
        public virtual Task<IEnumerable<TEntity>> GetRangeAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default) {
            return Query().Where(e => ids.Contains(e.Id)).ToEnumerableAsync(cancellationToken);
        }

        /// <inheritdoc />
        public virtual Task<TEntity> GetAsync(Guid guid, CancellationToken cancellationToken = default) {
            return Query().Where(e => e.Guid == guid).SingleOrDefaultAsync(cancellationToken);
        }

        /// <inheritdoc />
        public virtual Task<IEnumerable<TEntity>> GetRangeAsync(IEnumerable<Guid> guids, CancellationToken cancellationToken = default) {
            return Query().Where(e => guids.Contains(e.Guid)).ToEnumerableAsync(cancellationToken);
        }

        /// <inheritdoc />
        public virtual Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default) {
            return Query().ToEnumerableAsync(cancellationToken);
        }

        /// <inheritdoc />
        public abstract void Add(TEntity entity);
        /// <inheritdoc />
        public abstract void AddRange(IEnumerable<TEntity> entities);
        /// <inheritdoc />
        public abstract void Remove(TEntity entity);
        /// <inheritdoc />
        public abstract void RemoveRange(IEnumerable<TEntity> entities);
        /// <inheritdoc />
        public abstract void ReAttach(TEntity entity);
        /// <inheritdoc />
        public abstract void ReAttachRange(IEnumerable<TEntity> entities);
        /// <inheritdoc />
        public abstract IQuery<TEntity> Query();

        /// <summary>
        /// Get all entities matching an expression
        /// </summary>
        public virtual Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> filterExpression, CancellationToken cancellationToken = default) {
            return Query().Where(filterExpression).ToEnumerableAsync(cancellationToken);
        }
    }
}
