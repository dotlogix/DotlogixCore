// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DbContextFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using Microsoft.EntityFrameworkCore;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityFramework.EntityContext.Factories {
    public class DbContextFactory : IDbContextFactory {
        private readonly Func<DbContext> _createDbContext;

        public DbContextFactory(Func<DbContext> createDbContext) {
            _createDbContext = createDbContext;
        }

        public virtual DbContext Create() {
            return _createDbContext.Invoke();
        }
    }
}
