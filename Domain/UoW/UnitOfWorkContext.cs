using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Architecture.Infrastructure.Repositories;
using DotLogix.Architecture.Infrastructure.Repositories.Provider;

namespace DotLogix.Architecture.Domain.UoW {
    public class UnitOfWorkContext : IUnitOfWorkContext {
        protected IEntityContext EntityContext { get; }
        protected IRepositoryProvider RepoProvider { get; }
        protected IDictionary<Type, IRepository> RepositoryInstances { get; }

        public UnitOfWorkContext(IEntityContext entityContext, IRepositoryProvider repoProvider) {
            EntityContext = entityContext;
            RepoProvider = repoProvider;
            RepositoryInstances = new Dictionary<Type, IRepository>();
        }

        public IUnitOfWorkContext BeginContext() {
            return new NestedUnitOfWorkContext(this);
        }

        public TRepo UseRepository<TRepo>() where TRepo : IRepository {
            var repoInterface = typeof(TRepo);
            if(RepositoryInstances.TryGetValue(repoInterface, out var existing))
                return (TRepo)existing;

            var newInstance = RepoProvider.Create<TRepo>(EntityContext);
            RepositoryInstances.Add(repoInterface, newInstance);
            return newInstance;
        }

        public Task CompleteAsync() {
            return EntityContext.CompleteAsync();
        }

        public void Dispose() {
            EntityContext?.Dispose();
        }
    }
}