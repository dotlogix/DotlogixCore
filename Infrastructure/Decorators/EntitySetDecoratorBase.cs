// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EntitySetDecoratorBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  04.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Architecture.Infrastructure.Queries;
using DotLogix.Architecture.Infrastructure.Repositories;
#endregion

namespace DotLogix.Architecture.Infrastructure.Decorators {
    /// <summary>
    /// A decorator for <see cref="IEntitySet{TEntity}"/> to intercept requests
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class EntitySetDecoratorBase<TEntity> : IEntitySet<TEntity> where TEntity : class, new() {
        /// <summary>
        /// The internal base entity set
        /// </summary>
        protected IEntitySet<TEntity> BaseEntitySet { get; }

        /// <summary>
        /// Creates a new instance of <see cref="EntitySetDecoratorBase{TEntity}"/>
        /// </summary>
        protected EntitySetDecoratorBase(IEntitySet<TEntity> baseEntitySet) {
	        BaseEntitySet = baseEntitySet;
            EntityContext = baseEntitySet.EntityContext;
        }

        /// <inheritdoc />
        public IEntityContext EntityContext { get; }

        /// <inheritdoc />
		public virtual Task<TEntity> AddAsync(TEntity entity) {
            return BaseEntitySet.AddAsync(entity);
        }

        /// <inheritdoc />
        public virtual Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities) {
            return BaseEntitySet.AddRangeAsync(entities);
        }

        /// <inheritdoc />
        public virtual Task<TEntity> RemoveAsync(TEntity entity) {
            return BaseEntitySet.RemoveAsync(entity);
        }

        /// <inheritdoc />
        public virtual Task<IEnumerable<TEntity>> RemoveRangeAsync(IEnumerable<TEntity> entities) {
            return BaseEntitySet.RemoveRangeAsync(entities);
        }

        /// <inheritdoc />
        public virtual Task<TEntity> ReAttachAsync(TEntity entity) {
            return BaseEntitySet.ReAttachAsync(entity);
        }

        /// <inheritdoc />
        public virtual Task<IEnumerable<TEntity>> ReAttachRangeAsync(IEnumerable<TEntity> entities) {
            return BaseEntitySet.ReAttachRangeAsync(entities);
        }

        /// <inheritdoc />
        public virtual IQuery<TEntity> Query()
        {
	        var query = BaseEntitySet.Query();
	        return query;
        }

        public IQuery<TEntity> Query(params IQueryModifier<TEntity>[] filters) {
            var query = BaseEntitySet.Query(filters);
            return query;
        }
    }
}
