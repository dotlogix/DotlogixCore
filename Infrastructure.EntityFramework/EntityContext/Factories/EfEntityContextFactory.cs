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
        private readonly IDbContextFactory _contextFactory;

        public EfEntityContextFactory(IDbContextFactory contextFactory) {
            _contextFactory = contextFactory;
        }

        public IEntityContext Create() {
            return new EfEntityContext(_contextFactory.Create());
        }
    }
}
