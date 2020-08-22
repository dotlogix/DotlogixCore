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
    /// <summary>
    ///     An implementation of the <see cref="IDbContextFactory" /> interface for entity framework
    /// </summary>
    public class DbContextFactory : IDbContextFactory {
        private readonly Func<DbContext> _createDbContext;

        /// <summary>
        ///     Create a new instance of <see cref="DbContextFactory" />
        /// </summary>
        public DbContextFactory(Func<DbContext> createDbContext) {
            _createDbContext = createDbContext;
        }

        /// <inheritdoc />
        public virtual DbContext Create() {
            return _createDbContext.Invoke();
        }
    }
}
