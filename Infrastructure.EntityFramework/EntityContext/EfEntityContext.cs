// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EfEntityContext.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Collections.Generic;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Architecture.Infrastructure.EntityFramework.Query;
using DotLogix.Architecture.Infrastructure.Queries;
using Microsoft.EntityFrameworkCore;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityFramework.EntityContext {
    public class EfEntityContext : IEntityContext {
        public EfEntityContext(DbContext dbContext) {
            DbContext = dbContext;
        }

        public DbContext DbContext { get; }

        public void Dispose() {
            DbContext.Dispose();
        }

        public Task CompleteAsync() {
            return DbContext.SaveChangesAsync();
        }

        public IEntitySet<TEntity> UseSet<TEntity>() where TEntity : class, ISimpleEntity {
            return new EfEntitySet<TEntity>(DbContext.Set<TEntity>());
        }
    }

    public class EfEntitySet<TEntity> : EntitySetBase<TEntity> where TEntity : class, ISimpleEntity {
        private readonly DbSet<TEntity> _dbSet;

        public EfEntitySet(DbSet<TEntity> dbSet) {
            _dbSet = dbSet;
        }

        public override void Add(TEntity entity) {
            _dbSet.Add(entity);
        }

        public override void AddRange(IEnumerable<TEntity> entities) {
            _dbSet.AddRange(entities);
        }

        public override void Remove(TEntity entity) {
            _dbSet.Remove(entity);
        }

        public override void RemoveRange(IEnumerable<TEntity> entities) {
            _dbSet.RemoveRange(entities);
        }
        public override IQuery<TEntity> Query() {
            return EfQueryableQueryFactory.Instance.CreateQuery(_dbSet);
        }
    }
}
