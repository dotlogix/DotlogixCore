// ==================================================
// Copyright 2016(C) , DotLogix
// File:  EfInsertOnlyRepository.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  24.08.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.Entities.Options;
using DotLogix.Architecture.Infrastructure.EntityFramework.EntityContext;
using DotLogix.Architecture.Infrastructure.Repositories;
using DotLogix.Architecture.Infrastructure.Repositories.Enums;
using Microsoft.EntityFrameworkCore;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityFramework.Repositories {
    public class EfInsertOnlyRepository<TEntity> : EfRepositoryBase<TEntity>, IInsertOnlyRepository<TEntity>
        where TEntity : class, ISimpleEntity, IInsertOnly {
        protected EfInsertOnlyRepository(IEfEntityContext entityContext) : base(entityContext) { }
        public override void Add(TEntity entity) {
            entity.IsActive = true;
            DbSet.Add(entity);
        }

        public override void AddRange(IEnumerable<TEntity> entities) {
            foreach(var entity in entities) {
                entity.IsActive = true;
                DbSet.Add(entity);
            }
        }

        public override void Remove(TEntity entity) {
            entity.IsActive = false;
        }

        public override void RemoveRange(IEnumerable<TEntity> entities) {
            foreach (var entity in entities) {
                entity.IsActive = false;
            }
        }

        public virtual async Task<TEntity> GetAsync(int id, InsertOnlyFilterModes includeFilter) {
            return await Query(includeFilter).SingleOrDefaultAsync(e => e.Id == id);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(InsertOnlyFilterModes includeFilter) {
            return await Query(includeFilter).ToListAsync();
        }

        protected override IQueryable<TEntity> Query() {
            return Query(InsertOnlyFilterModes.Active);
        }

        protected virtual IQueryable<TEntity> Query(InsertOnlyFilterModes includeFilter) {
            switch(includeFilter) {
                case InsertOnlyFilterModes.Active:
                    return base.Query().Where(e => e.IsActive);
                case InsertOnlyFilterModes.InActive:
                    return base.Query().Where(e => e.IsActive == false);
                case InsertOnlyFilterModes.All:
                    return base.Query();
                default:
                    throw new ArgumentOutOfRangeException(nameof(includeFilter), includeFilter, null);
            }
        }
    }
}
