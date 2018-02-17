// ==================================================
// Copyright 2018(C) , DotLogix
// File:  UnitOfWorkFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Architecture.Infrastructure.Repositories.Provider;
#endregion

namespace DotLogix.Architecture.Domain.UoW {
    public class UnitOfWorkFactory : IUnitOfWorkFactory {
        protected IRepositoryProvider RepositoryProvider { get; }
        protected IEntityContextFactory EntityContextFactory { get; }

        public UnitOfWorkFactory(IRepositoryProvider repositoryProvider, IEntityContextFactory entityContextFactory) {
            RepositoryProvider = repositoryProvider;
            EntityContextFactory = entityContextFactory;
        }

        public virtual IUnitOfWork Create() {
            return new UnitOfWork(EntityContextFactory, RepositoryProvider);
        }
    }
}
