// ==================================================
// Copyright 2018(C) , DotLogix
// File:  UnitOfWork.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Architecture.Infrastructure.Repositories.Provider;
#endregion

namespace DotLogix.Architecture.Domain.UoW {
    public class UnitOfWork : IUnitOfWork {
        protected IEntityContextFactory EntityContextFactory { get; }
        protected IRepositoryProvider RepoProvider { get; }

        public UnitOfWork(IEntityContextFactory entityContextFactory, IRepositoryProvider repoProvider) {
            EntityContextFactory = entityContextFactory;
            RepoProvider = repoProvider;
        }

        public virtual IUnitOfWorkContext BeginContext() {
            var entityContext = EntityContextFactory.Create();
            return new UnitOfWorkContext(entityContext, RepoProvider);
        }
    }
}
