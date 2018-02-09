// ==================================================
// Copyright 2017(C) , DotLogix
// File:  EfEntityContextFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  10.12.2017
// LastEdited:  10.12.2017
// ==================================================

#region
#endregion

using DotLogix.Architecture.Infrastructure.EntityContext;

namespace DotLogix.Architecture.Infrastructure.EntityFramework.EntityContext.Factories {
    public class EfEntityContextFactory : IEntityContextFactory {
        protected IDbContextFactory DbContextFactory { get; }

        public EfEntityContextFactory(IDbContextFactory contextFactory) {
            DbContextFactory = contextFactory;
        }

        public virtual IEntityContext Create() {
            return new EfEntityContext(DbContextFactory.Create());
        }
    }
}
