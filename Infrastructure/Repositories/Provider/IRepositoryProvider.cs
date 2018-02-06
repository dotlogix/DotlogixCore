using DotLogix.Architecture.Infrastructure.EntityContext;

namespace DotLogix.Architecture.Infrastructure.Repositories.Provider {
    public interface IRepositoryProvider {
        TRepoInterface Create<TRepoInterface>(IEntityContext entityContext) where TRepoInterface : IRepository;
    }
}