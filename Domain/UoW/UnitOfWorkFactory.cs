using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Architecture.Infrastructure.Repositories.Provider;

namespace DotLogix.Architecture.Domain.UoW {
    public class UnitOfWorkFactory : IUnitOfWorkFactory {
        protected IRepositoryProvider RepositoryProvider { get; }
        protected IEntityContextFactory EntityContextFactory { get; }

        public UnitOfWorkFactory(IRepositoryProvider repositoryProvider, IEntityContextFactory entityContextFactory) {
            RepositoryProvider = repositoryProvider;
            EntityContextFactory = entityContextFactory;
        }

        public virtual IUnitOfWork Create() {
            return new UnitOfWork(EntityContextFactory, RepositoryProvider);
        }
    }
}