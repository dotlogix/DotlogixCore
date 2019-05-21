// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EfEntityContext.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  19.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Threading.Tasks;
using DotLogix.Architecture.Common.Entities;
using DotLogix.Architecture.Infrastructure.EntityContext;
using Microsoft.EntityFrameworkCore;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityFramework.EntityContext {
    public class EfEntityContext : IEntityContext {
        public DbContext DbContext { get; }

        public EfEntityContext(DbContext dbContext) {
            DbContext = dbContext;
        }

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
}
