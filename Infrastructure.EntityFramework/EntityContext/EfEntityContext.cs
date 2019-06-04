// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EfEntityContext.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  19.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.EntityContext;
using Microsoft.EntityFrameworkCore;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityFramework.EntityContext {
    public class EfEntityContext : IEntityContext {
        public DbContext DbContext { get; }
        public IDictionary<string, object> Variables { get; } = new Dictionary<string, object>();

        public EfEntityContext(DbContext dbContext) {
            DbContext = dbContext;
        }

        public virtual void Dispose() {
            DbContext.Dispose();
        }

        public virtual Task CompleteAsync() {
            return DbContext.SaveChangesAsync();
        }

        public virtual IEntitySet<TEntity> UseSet<TEntity>() where TEntity : class, ISimpleEntity {
            return new EfEntitySet<TEntity>(DbContext.Set<TEntity>());
        }
    }
}
