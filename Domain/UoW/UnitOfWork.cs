using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Architecture.Infrastructure.Repositories.Provider;

namespace DotLogix.Architecture.Domain.UoW {
    public class UnitOfWork : IUnitOfWork {
        private readonly IEntityContextFactory _entityContextFactory;
        private readonly IRepositoryProvider _repoProvider;

        public UnitOfWork(IEntityContextFactory entityContextFactory, IRepositoryProvider repoProvider) {
            _entityContextFactory = entityContextFactory;
            _repoProvider = repoProvider;
        }

        public IUnitOfWorkContext BeginContext() {
            var entityContext = _entityContextFactory.Create();
            return new UnitOfWorkContext(entityContext, _repoProvider);
        }
    }
}