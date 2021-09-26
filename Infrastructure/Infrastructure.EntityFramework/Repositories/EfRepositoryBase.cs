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
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Core.Diagnostics;
using DotLogix.Infrastructure.EntityFramework.Hooks;
using DotLogix.Infrastructure.Extensions;
using DotLogix.Infrastructure.Queries;
using DotLogix.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
#endregion

namespace DotLogix.Infrastructure.EntityFramework.Repositories {
    /// <summary>
    ///     A generic ef repository providing crud functionality
    /// </summary>
    public class EfRepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class, new() {
        private IEntitySet<TEntity> _entitySet;

        /// <summary>
        ///     Gets the entity that maps the given entity class.
        /// </summary>
        protected IEntityType EntityType { get; }
        
        /// <summary>
        ///     Gets the entity clr type that maps the given entity class.
        /// </summary>
        protected Type EntityClrType => EntityType.ClrType;

        /// <summary>
        ///     The hooks for this instance
        /// </summary>
        protected IEfEventHandler EventHandler { get; }

        /// <summary>
        ///     The entity type
        /// </summary>
        protected DbContext DbContext => EntityContext.DbContext;

        /// <summary>
        ///     The current entity context
        /// </summary>
        protected IEfEntityContext EntityContext { get; }

        /// <summary>
        ///     The current logger instance
        /// </summary>
        protected ILogSource LogSource { get; }

        /// <summary>
        ///     The undecorated and unmodified entity set
        /// </summary>
        public IEntitySet<TEntity> EntitySet => _entitySet ??= EntityContext.GetEntitySet<TEntity>();

        /// <summary>
        ///     Creates a new instance of <see cref="EfRepositoryBase{TEntity}" />
        /// </summary>
        protected EfRepositoryBase(IEfEntityContext entityContext) {
            EntityType = entityContext.DbContext.Model.FindEntityType(typeof(TEntity));
            EventHandler = entityContext.EventManager.GetHandler(EntityType.ClrType);
            EntityContext = entityContext;
            LogSource = entityContext.LogSourceProvider.Create(GetType().FullName);
        }

        /// <inheritdoc />
        public virtual async Task<ICollection<TEntity>> GetAllAsync(CancellationToken cancellationToken = default) {
            return await Query().ToListAsync(cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task<ICollection<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> filterExpression, CancellationToken cancellationToken = default) {
            return await Query().Where(filterExpression).ToListAsync(cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task<ICollection<TEntity>> WhereAsync(IQueryModifier<TEntity, TEntity> filter, CancellationToken cancellationToken = default) {
            return await Query(filter).ToListAsync(cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task<TEntity> AddAsync(TEntity entity) {
            return await EntitySet.AddAsync(entity);
        }

        /// <inheritdoc />
        public virtual async Task<ICollection<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities) {
            return await EntitySet.AddRangeAsync(entities);
        }

        /// <inheritdoc />
        public virtual async Task<TEntity> RemoveAsync(TEntity entity) {
            return await EntitySet.RemoveAsync(entity);
        }

        /// <inheritdoc />
        public virtual async Task<ICollection<TEntity>> RemoveRangeAsync(IEnumerable<TEntity> entities) {
            return await EntitySet.RemoveRangeAsync(entities);
        }

        /// <summary>
        ///     Create a linq query to allow more advanced requests to the entity set
        /// </summary>
        protected virtual IQueryable<TEntity> Query() {
            return EntitySet.Query();
        }

        /// <summary>
        ///     Create a linq query to allow more advanced requests to the entity set
        /// </summary>
        protected virtual IQueryable<TEntity> Query(params IQueryModifier<TEntity, TEntity>[] filters) {
            return Query().Apply(EntityContext, filters);
        }
    }

    /// <summary>
    ///     A generic ef repository providing crud functionality
    /// </summary>
    public class EfRepositoryBase<TKey, TEntity> : EfRepositoryBase<TEntity>, IRepository<TKey, TEntity> where TEntity : class, new() {
        /// <summary>
        ///     Gets primary key for this entity type. Returns <see langword="null" /> if no primary key is defined.
        /// </summary>
        protected IKey PrimaryKey { get; }
        /// <summary>
        ///     Gets the property that make up the key.
        /// </summary>
        protected IProperty KeyProperty => PrimaryKey.Properties.Single();

        /// <inheritdoc />
        public EfRepositoryBase(IEfEntityContext entityContext) : base(entityContext) {
            PrimaryKey = EntityType.FindPrimaryKey();
            ValidatePrimaryKey();
        }

        /// <inheritdoc />
        public virtual async Task<TEntity> GetAsync(TKey key, CancellationToken cancellationToken = default) {
            var keyColumn = KeyProperty.Name;
            return await Query()
                        .Where(e => Equals(EF.Property<TKey>(e, keyColumn), key))
                        .SingleOrDefaultAsync(cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task<ICollection<TEntity>> GetRangeAsync(IEnumerable<TKey> keys, CancellationToken cancellationToken = default) {
            var keyColumn = KeyProperty.Name;
            return await Query()
                        .Where(e => keys.Contains(EF.Property<TKey>(e, keyColumn)))
                        .ToListAsync(cancellationToken);
        }

        private void ValidatePrimaryKey() {
            if(PrimaryKey.Properties.Count != 1) {
                var message = $"Expected a single property of type {typeof(TKey)} as primary key, but got properties " +
                              string.Join(", ", PrimaryKey.Properties.Select(e => e.ClrType.Name));
                throw new ArgumentException(message);
            }

            var primaryKeyProperty = PrimaryKey.Properties[0];
            if(primaryKeyProperty.ClrType != typeof(TKey)) {
                throw new ArgumentException($"Expected a single property of type {typeof(TKey)} as primary key, but got property {primaryKeyProperty.Name} with type {primaryKeyProperty.ClrType}");
            }
        }
    }
}
