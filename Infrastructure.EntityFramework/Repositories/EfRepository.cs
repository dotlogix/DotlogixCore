// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EfRepository.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Collections.Generic;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.EntityFramework.EntityContext;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityFramework.Repositories {
    public abstract class EfRepository<TEntity> : EfRepositoryBase<TEntity>
        where TEntity : class, ISimpleEntity {
        protected EfRepository(IEfEntityContext entityContext) : base(entityContext) { }

        public override void Add(TEntity entity) {
            DbSet.Add(entity);
        }

        public override void AddRange(IEnumerable<TEntity> entities) {
            DbSet.AddRange(entities);
        }

        public override void Remove(TEntity entity) {
            DbSet.Remove(entity);
        }

        public override void RemoveRange(IEnumerable<TEntity> entities) {
            DbSet.RemoveRange(entities);
        }
    }
}
