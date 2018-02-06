// ==================================================
// Copyright 2017(C) , DotLogix
// File:  EfEntityContext.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  10.12.2017
// LastEdited:  10.12.2017
// ==================================================

#region
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityFramework.EntityContext {
    public class EfEntityContext : IEfEntityContext
    {
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
