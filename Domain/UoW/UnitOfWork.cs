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
    /// <summary>
    /// An implementation of the <see cref="IUnitOfWork"/>
    /// </summary>
    public class UnitOfWork : IUnitOfWork {
        /// <summary>
        /// The internal <see cref="IEntityContextFactory"/>
        /// </summary>
        protected IEntityContextFactory EntityContextFactory { get; }
        /// <summary>
        /// The internal <see cref="IRepositoryProvider"/>
        /// </summary>
        protected IRepositoryProvider RepoProvider { get; }
        /// <summary>
        /// Create a new instance of <see cref="UnitOfWork"/>
        /// </summary>
        public UnitOfWork(IEntityContextFactory entityContextFactory, IRepositoryProvider repoProvider) {
            EntityContextFactory = entityContextFactory;
            RepoProvider = repoProvider;
        }

        /// <inheritdoc />
        public virtual IUnitOfWorkContext BeginContext() {
            var entityContext = EntityContextFactory.Create();
            return new UnitOfWorkContext(entityContext, RepoProvider);
        }
    }
}
