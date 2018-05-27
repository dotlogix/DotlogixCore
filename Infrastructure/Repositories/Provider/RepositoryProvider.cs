// ==================================================
// Copyright 2018(C) , DotLogix
// File:  RepositoryProvider.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Architecture.Infrastructure.Repositories.Factories;
#endregion

namespace DotLogix.Architecture.Infrastructure.Repositories.Provider {
    public class RepositoryProvider : IRepositoryProvider {
        protected Dictionary<Type, IRepositoryFactory> RepositoryFactories { get; } = new Dictionary<Type, IRepositoryFactory>();


        public virtual TRepoInterface Create<TRepoInterface>(IEntitySetProvider entitySetProvider) where TRepoInterface : IRepository {
            if(RepositoryFactories.TryGetValue(typeof(TRepoInterface), out var factory) == false)
                throw new ArgumentException($"Type {typeof(TRepoInterface)} is not registered for this provider", nameof(TRepoInterface));
            return (TRepoInterface)factory.Create(entitySetProvider);
        }

        public virtual void RegisterFactory(Type repoInterface, IRepositoryFactory factory) {
            RepositoryFactories.Add(repoInterface, factory);
        }

        public virtual void RegisterFactory(Type repoType, Type repoInterface) {
            var factory = DynamicRepositoryFactory.CreateFor(repoType);
            RegisterFactory(repoInterface, factory);
        }
    }
}
