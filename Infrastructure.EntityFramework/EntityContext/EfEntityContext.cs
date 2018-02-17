// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EfEntityContext.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityFramework.EntityContext {
    public class EfEntityContext : IEfEntityContext {
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
    }
}
