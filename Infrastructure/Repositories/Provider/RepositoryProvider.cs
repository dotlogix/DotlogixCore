using System;
using System.Collections.Generic;
using System.Linq;
using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Architecture.Infrastructure.Repositories.Factories;

namespace DotLogix.Architecture.Infrastructure.Repositories.Provider {
    public class RepositoryProvider : IRepositoryProvider {
        protected Dictionary<Type, IRepositoryFactory> RepositoryFactories { get; } = new Dictionary<Type, IRepositoryFactory>();

        public virtual void RegisterFactory(Type repoInterface, IRepositoryFactory factory) {
            RepositoryFactories.Add(repoInterface, factory);
        }

        public virtual void RegisterFactory(Type repoType, Type repoInterface, Type contextType) {
            var factory = DynamicRepositoryFactory.CreateFor(repoType, contextType);
            RegisterFactory(repoInterface, factory);
        }


        public virtual TRepoInterface Create<TRepoInterface>(IEntityContext entityContext) where TRepoInterface:IRepository{
            if(RepositoryFactories.TryGetValue(typeof(TRepoInterface), out var factory) == false)
                throw new ArgumentException($"Type {typeof(TRepoInterface)} is not registered for this provider", nameof(TRepoInterface));
            return (TRepoInterface)factory.Create(entityContext);
        }
    }
}