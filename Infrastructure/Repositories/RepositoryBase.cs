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
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Architecture.Infrastructure.Queries.Queryable;
#endregion

namespace DotLogix.Architecture.Infrastructure.Repositories {
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class, ISimpleEntity, new() {
        private IEntitySet<TEntity> _entitySet;

        protected bool AllowCaching { get; }
        protected IEntitySet<TEntity> EntitySet => _entitySet ?? (_entitySet = OnModifyEntitySet(EntitySetProvider.UseSet<TEntity>()));
        protected IEntitySetProvider EntitySetProvider { get; }

        protected RepositoryBase(IEntitySetProvider entitySetProvider, bool allowCaching = true) {
            EntitySetProvider = entitySetProvider;
            AllowCaching = allowCaching;
        }

        public virtual Task<TEntity> GetAsync(int id, CancellationToken cancellationToken = default) {
            return EntitySet.GetAsync(id, cancellationToken);
        }

        public virtual Task<IEnumerable<TEntity>> GetRangeAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default) {
            return EntitySet.GetRangeAsync(ids, cancellationToken);
        }

        public virtual Task<TEntity> GetAsync(Guid guid, CancellationToken cancellationToken = default) {
            return EntitySet.GetAsync(guid, cancellationToken);
        }

        public virtual Task<IEnumerable<TEntity>> GetRangeAsync(IEnumerable<Guid> guids, CancellationToken cancellationToken = default) {
            return EntitySet.GetRangeAsync(guids, cancellationToken);
        }

        public virtual Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default) {
            return EntitySet.GetAllAsync(cancellationToken);
        }

        public virtual Task<IEnumerable<TEntity>> FilterAllAsync(Expression<Func<TEntity, bool>> filterExpression, CancellationToken cancellationToken = default) {
            return EntitySet.Query().Where(filterExpression).ToEnumerableAsync(cancellationToken);
        }

        public virtual void Add(TEntity entity) {
            EntitySet.Add(entity);
        }

        public virtual void AddRange(IEnumerable<TEntity> entities) {
            EntitySet.AddRange(entities);
        }

        public virtual void Remove(TEntity entity) {
            EntitySet.Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entities) {
            EntitySet.RemoveRange(entities);
        }

        protected virtual IEntitySet<TEntity> OnModifyEntitySet(IEntitySet<TEntity> set) {
            if(AllowCaching)
                set = new GuidIndexedEntitySetDecorator<TEntity>(set, OnCreateCache());
            return set;
        }

        protected virtual EntityCollection<TEntity> OnCreateCache() {
            return new EntityCollection<TEntity>();
        }
    }
}
