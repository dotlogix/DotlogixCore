// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EfRepositoryBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.EntityFramework.EntityContext;
using DotLogix.Architecture.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityFramework.Repositories {
    public abstract class EfRepositoryBase<TEntity> : RepositoryBase<IEfEntityContext>, IRepository<TEntity> where TEntity : class, ISimpleEntity {
        protected DbContext DbContext { get; }
        protected DbSet<TEntity> DbSet { get; }

        protected EfRepositoryBase(IEfEntityContext entityContext) : base(entityContext) {
            DbContext = EntityContext.DbContext;
            DbSet = DbContext.Set<TEntity>();
        }

        public Task<TEntity> GetAsync(int id) {
            return Query().FirstOrDefaultAsync(e => e.Id == id);
        }

        public Task<TEntity> GetAsync(Guid guid) {
            return Query().FirstOrDefaultAsync(e => e.Guid == guid);
        }

        public Task<IEnumerable<TEntity>> GetAllAsync() {
            return Query().ToEnumerableAsync();
        }

        public Task<IEnumerable<TEntity>> FilterAllAsync(Expression<Func<TEntity, bool>> filterExpression) {
            return Query().Where(filterExpression).ToEnumerableAsync();
        }

        public abstract void Add(TEntity entity);
        public abstract void AddRange(IEnumerable<TEntity> entities);
        public abstract void Remove(TEntity entity);
        public abstract void RemoveRange(IEnumerable<TEntity> entities);

        protected virtual IQueryable<TEntity> Query() {
            return DbSet;
        }
    }
}
