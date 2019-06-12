// ==================================================
// Copyright 2018(C) , DotLogix
// File:  RepositoryProvider.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Architecture.Infrastructure.Repositories.Factories;
#endregion

namespace DotLogix.Architecture.Infrastructure.Repositories.Provider {
    /// <summary>
    /// An implementation of the <see cref="IRepositoryProvider"/>
    /// </summary>
    public class RepositoryProvider : IRepositoryProvider {
        /// <summary>
        /// The internal registered repository factories
        /// </summary>
        protected Dictionary<Type, IRepositoryFactory> RepositoryFactories { get; } = new Dictionary<Type, IRepositoryFactory>();


        /// <inheritdoc />
        public virtual TRepoInterface Create<TRepoInterface>(IEntitySetProvider entitySetProvider) where TRepoInterface : IRepository {
            if(RepositoryFactories.TryGetValue(typeof(TRepoInterface), out var factory) == false)
                throw new ArgumentException($"Type {typeof(TRepoInterface)} is not registered for this provider", nameof(TRepoInterface));
            return (TRepoInterface)factory.Create(entitySetProvider);
        }

        /// <summary>
        /// Registers a factory new factory
        /// </summary>
        public virtual void RegisterFactory(Type repoInterface, IRepositoryFactory factory) {
            RepositoryFactories.Add(repoInterface, factory);
        }

        /// <summary>
        /// Registers a new <see cref="DynamicRepositoryFactory"/>
        /// </summary>
        public virtual void RegisterFactory(Type repoType, Type repoInterface) {
            var factory = DynamicRepositoryFactory.CreateFor(repoType);
            RegisterFactory(repoInterface, factory);
        }
    }
}
