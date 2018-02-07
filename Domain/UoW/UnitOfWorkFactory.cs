using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Architecture.Infrastructure.Repositories.Provider;

namespace DotLogix.Architecture.Domain.UoW {
    public class UnitOfWorkFactory : IUnitOfWorkFactory {
        private readonly IRepositoryProvider _repositoryProvider;
        private readonly IEntityContextFactory _entityContextFactory;

        public UnitOfWorkFactory(IRepositoryProvider repositoryProvider, IEntityContextFactory entityContextFactory) {
            _repositoryProvider = repositoryProvider;
            _entityContextFactory = entityContextFactory;
        }

        public IUnitOfWork Create() {
            return new UnitOfWork(_entityContextFactory, _repositoryProvider);
        }
    }
}