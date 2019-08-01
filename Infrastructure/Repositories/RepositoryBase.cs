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
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class, ISimpleEntity, new() {
        private IEntitySet<TEntity> _entitySet;
        private IEntitySet<TEntity> _originalEntitySet;

        /// <summary>
        ///     The cached list of entity set modifiers
        /// </summary>
        protected static IEnumerable<Func<IEntitySet<TEntity>, IEntitySet<TEntity>>> ModifyEntitySetAttributeCache { get; set; }

        /// <summary>
        ///     The internal entity set
        /// </summary>
        protected IEntitySet<TEntity> EntitySet => _entitySet ?? (_entitySet = OnModifyEntitySet(OriginalEntitySet));

        /// <summary>
        ///     The undecorated and unmodified entity set
        /// </summary>
        public IEntitySet<TEntity> OriginalEntitySet => _originalEntitySet ?? (_originalEntitySet = EntitySetProvider.UseSet<TEntity>());

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
        public virtual Task<TEntity> GetAsync(int id, CancellationToken cancellationToken = default) {
            return EntitySet.GetAsync(id, cancellationToken);
        }

        /// <inheritdoc />
        public virtual Task<IEnumerable<TEntity>> GetRangeAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default) {
            return EntitySet.GetRangeAsync(ids, cancellationToken);
        }

        /// <inheritdoc />
        public virtual Task<TEntity> GetAsync(Guid guid, CancellationToken cancellationToken = default) {
            return EntitySet.GetAsync(guid, cancellationToken);
        }

        /// <inheritdoc />
        public virtual Task<IEnumerable<TEntity>> GetRangeAsync(IEnumerable<Guid> guids, CancellationToken cancellationToken = default) {
            return EntitySet.GetRangeAsync(guids, cancellationToken);
        }

        /// <inheritdoc />
        public virtual Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default) {
            return EntitySet.GetAllAsync(cancellationToken);
        }

        /// <inheritdoc />
        public virtual Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> filterExpression, CancellationToken cancellationToken = default) {
            return EntitySet.Query().Where(filterExpression).ToEnumerableAsync(cancellationToken);
        }

        /// <inheritdoc />
        public virtual void Add(TEntity entity) {
            EntitySet.Add(entity);
        }

        /// <inheritdoc />
        public virtual void AddRange(IEnumerable<TEntity> entities) {
            EntitySet.AddRange(entities);
        }

        /// <inheritdoc />
        public virtual void Remove(TEntity entity) {
            EntitySet.Remove(entity);
        }

        /// <inheritdoc />
        public virtual void RemoveRange(IEnumerable<TEntity> entities) {
            EntitySet.RemoveRange(entities);
        }

        /// <summary>
        ///     A callback method to apply the entity set modifiers
        /// </summary>
        protected virtual IEntitySet<TEntity> OnModifyEntitySet(IEntitySet<TEntity> set) {
            if(ModifyEntitySetAttributeCache == null)
                ModifyEntitySetAttributeCache = CreateEntitySetModifiers().ToList();

            return ModifyEntitySetAttributeCache.Aggregate(set, (current, func) => func.Invoke(current));
        }

        /// <summary>
        ///     A callback method to create the entity set modifiers
        /// </summary>
        protected virtual IEnumerable<Func<IEntitySet<TEntity>, IEntitySet<TEntity>>> CreateEntitySetModifiers() {
            var entityType = typeof(TEntity);
            var repoType = GetType();

            var types = new List<Type>();
            types.Add(repoType);
            types.AddRange(repoType.GetTypesAssignableTo());
            types.Add(entityType);
            types.AddRange(entityType.GetTypesAssignableTo());

            var decoratorAttributes = types.SelectMany(t => t.GetCustomAttributes<EntitySetModifierAttribute>());

            var decorators = decoratorAttributes.Distinct().OrderBy(d => d.Priority);
            return decorators.Select(d => d.GetModifierFunc<TEntity>());
        }
    }
}
