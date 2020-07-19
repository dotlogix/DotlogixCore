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
        
        /// <summary>
        /// The internal registered repository remappings
        /// </summary>
        protected Dictionary<Type, Type> RepositoryRemapping { get; } = new Dictionary<Type, Type>();

        /// <inheritdoc />
        public virtual TRepoInterface Create<TRepoInterface>(IEntityContext entityContext) where TRepoInterface : IRepository {
            var repoType = typeof(TRepoInterface);
            if (RepositoryRemapping.TryGetValue(repoType, out var remappedType))
                repoType = remappedType;

            var repositoryFactory = (IRepositoryFactory)null;
            if (repoType != null && !RepositoryFactories.TryGetValue(repoType, out repositoryFactory)) {
                foreach (var factoryCandidate in RepositoryFactories) {
                    if(repoType.IsAssignableFrom(factoryCandidate.Key) == false)
                        continue;
                    RegisterMapping(repoType, factoryCandidate.Key);
                    repositoryFactory = factoryCandidate.Value;
                    break;
                }
            }
            if (repositoryFactory == null)
                throw new ArgumentException($"Type {repoType} is not registered for this provider", nameof(TRepoInterface));
            return (TRepoInterface)repositoryFactory.Create(entityContext);
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


        /// <summary>
        /// Registers a new remapping
        /// </summary>
        public virtual void RegisterMapping(Type requestedType, Type targetType) {
            RepositoryRemapping[requestedType] = targetType;
        }
    }
}
