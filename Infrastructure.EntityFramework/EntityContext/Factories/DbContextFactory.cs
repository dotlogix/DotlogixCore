// ==================================================
// Copyright 2017(C) , DotLogix
// File:  DbContextFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  10.12.2017
// LastEdited:  10.12.2017
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
