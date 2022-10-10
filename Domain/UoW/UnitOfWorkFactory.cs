// ==================================================
// Copyright 2018(C) , DotLogix
// File:  UnitOfWorkFactory.cs
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
    /// An implementation of the <see cref="IUnitOfWorkFactory"/> interface
    /// </summary>
    public class UnitOfWorkFactory : IUnitOfWorkFactory {
        /// <summary>
        /// The internal <see cref="IRepositoryProvider"/>
        /// </summary>
        protected IRepositoryProvider RepositoryProvider { get; }
        /// <summary>
        /// The internal <see cref="IEntityContextFactory"/>
        /// </summary>
        protected IEntityContextFactory EntityContextFactory { get; }


        /// <summary>
        /// Create a new instance of <see cref="UnitOfWorkFactory"/>
        /// </summary>
        public UnitOfWorkFactory(IRepositoryProvider repositoryProvider, IEntityContextFactory entityContextFactory) {
            RepositoryProvider = repositoryProvider;
            EntityContextFactory = entityContextFactory;
        }

        /// <inheritdoc />
        public virtual IUnitOfWork Create() {
            return new UnitOfWork(EntityContextFactory, RepositoryProvider);
        }
    }
}
