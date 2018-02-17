// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EfOrderedEfRepository.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Linq;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.Entities.Options;
using DotLogix.Architecture.Infrastructure.EntityFramework.EntityContext;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityFramework.Repositories {
    public abstract class EfOrderedEfRepository<TEntity> : EfRepository<TEntity>
        where TEntity : class, ISimpleEntity, IOrdered {
        protected EfOrderedEfRepository(IEfEntityContext entityContext) : base(entityContext) { }

        protected override IQueryable<TEntity> Query() {
            return QueryDesc();
        }

        protected IOrderedQueryable<TEntity> QueryDesc() {
            return base.Query().OrderByDescending(o => o.Order);
        }

        protected IOrderedQueryable<TEntity> QueryAsc() {
            return base.Query().OrderBy(o => o.Order);
        }
    }
}
