// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EfEntityContextFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
#endregion

#region
using DotLogix.Architecture.Infrastructure.EntityContext;
#endregion

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
