﻿// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EfEntityContextFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
#endregion

#region
using DotLogix.Architecture.Infrastructure.EntityContext;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityFramework.EntityContext.Factories {
    /// <summary>
    /// An implementation of the <see cref="IEntityContextFactory"/> for entity framework
    /// </summary>
    public class EfEntityContextFactory : IEntityContextFactory {
        /// <summary>
        /// The internal <see cref="IDbContextFactory"/>
        /// </summary>
        protected IDbContextFactory DbContextFactory { get; }

        /// <summary>
        /// Create a new instance of <see cref="EfEntityContextFactory"/>
        /// </summary>
        public EfEntityContextFactory(IDbContextFactory contextFactory) {
            DbContextFactory = contextFactory;
        }

        /// <inheritdoc />
        public virtual IEntityContext Create() {
            return new EfEntityContext(DbContextFactory.Create());
        }
    }
}
