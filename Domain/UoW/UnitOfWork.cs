using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Architecture.Infrastructure.Repositories.Provider;

namespace DotLogix.Architecture.Domain.UoW {
    public class UnitOfWork : IUnitOfWork {
        protected IEntityContextFactory EntityContextFactory { get; }
        protected IRepositoryProvider RepoProvider { get; }

        public UnitOfWork(IEntityContextFactory entityContextFactory, IRepositoryProvider repoProvider) {
            EntityContextFactory = entityContextFactory;
            RepoProvider = repoProvider;
        }

        public virtual IUnitOfWorkContext BeginContext() {
            var entityContext = EntityContextFactory.Create();
            return new UnitOfWorkContext(entityContext, RepoProvider);
        }
    }
}