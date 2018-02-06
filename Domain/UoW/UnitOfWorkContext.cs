using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Architecture.Infrastructure.Repositories;
using DotLogix.Architecture.Infrastructure.Repositories.Provider;

namespace DotLogix.Architecture.Domain.UoW {
    public class UnitOfWorkContext : IUnitOfWorkContext {
        private readonly IEntityContext _entityContext;
        private readonly IRepositoryProvider _repoProvider;
        private readonly IDictionary<Type, IRepository> _repositoryInstances;

        public UnitOfWorkContext(IEntityContext entityContext, IRepositoryProvider repoProvider) {
            _entityContext = entityContext;
            _repoProvider = repoProvider;
            _repositoryInstances = new Dictionary<Type, IRepository>();
        }

        public IUnitOfWorkContext BeginContext() {
            return new NestedUnitOfWorkContext(this);
        }

        public TRepo UseRepository<TRepo>() where TRepo : IRepository {
            var repoInterface = typeof(TRepo);
            if(_repositoryInstances.TryGetValue(repoInterface, out var existing))
                return (TRepo)existing;

            var newInstance = _repoProvider.Create<TRepo>(_entityContext);
            _repositoryInstances.Add(repoInterface, newInstance);
            return newInstance;
        }

        public Task CompleteAsync() {
            return _entityContext.CompleteAsync();
        }

        public void Dispose() {
            _entityContext?.Dispose();
        }
    }
}