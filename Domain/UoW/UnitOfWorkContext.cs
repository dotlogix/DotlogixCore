// ==================================================
// Copyright 2018(C) , DotLogix
// File:  UnitOfWorkContext.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Architecture.Infrastructure.Repositories;
using DotLogix.Architecture.Infrastructure.Repositories.Provider;
#endregion

namespace DotLogix.Architecture.Domain.UoW {
    /// <summary>
    /// An implementation of the <see cref="IUnitOfWorkContext"/> wrapping a <see cref="IEntityContext"/>
    /// </summary>
    public class UnitOfWorkContext : IUnitOfWorkContext {
        /// <summary>
        /// The internal <see cref="IEntityContext"/>
        /// </summary>
        public IEntityContext EntityContext { get; }
        /// <summary>
        /// The internal <see cref="IRepositoryProvider"/>
        /// </summary>
        protected IRepositoryProvider RepoProvider { get; }
        /// <summary>
        /// The internal cached repository instances
        /// </summary>
        protected IDictionary<Type, IRepository> RepositoryInstances { get; }

        /// <summary>
        /// Create a new instance of <see cref="UnitOfWorkContext"/>
        /// </summary>
        public UnitOfWorkContext(IEntityContext entityContext, IRepositoryProvider repoProvider) {
            EntityContext = entityContext;
            RepoProvider = repoProvider;
            RepositoryInstances = new Dictionary<Type, IRepository>();
        }

        /// <inheritdoc />
        public IUnitOfWorkContext BeginContext() {
            return new NestedUnitOfWorkContext(this);
        }

        /// <inheritdoc />
        public TRepo UseRepository<TRepo>() where TRepo : IRepository {
            var repoInterface = typeof(TRepo);
            if(RepositoryInstances.TryGetValue(repoInterface, out var existing))
                return (TRepo)existing;

            var newInstance = RepoProvider.Create<TRepo>(EntityContext);
            RepositoryInstances.Add(repoInterface, newInstance);
            return newInstance;
        }

        /// <inheritdoc />
        public Task CompleteAsync() {
            return EntityContext.CompleteAsync();
        }

        /// <inheritdoc />
        public void Dispose() {
            EntityContext?.Dispose();
        }
    }
}
