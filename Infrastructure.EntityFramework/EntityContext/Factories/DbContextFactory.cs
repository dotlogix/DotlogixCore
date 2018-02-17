// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DbContextFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
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

        public DbContext Create() {
            return _createDbContext.Invoke();
        }
    }
}
