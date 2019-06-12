// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IRepositoryProvider.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Architecture.Infrastructure.EntityContext;
#endregion

namespace DotLogix.Architecture.Infrastructure.Repositories.Provider {
    /// <summary>
    /// An interface to represent a provider for <see cref="IRepository{TEntity}"/>
    /// </summary>
    public interface IRepositoryProvider {
        /// <summary>
        /// Get or create an instance of <see cref="IRepository"/>
        /// </summary>
        TRepoInterface Create<TRepoInterface>(IEntitySetProvider entitySetProvider) where TRepoInterface : IRepository;
    }
}
