using DotLogix.Architecture.Domain.Services;
using DotLogix.Architecture.Infrastructure.UoW;

namespace DotLogix.Architecture.Domain.Context
{
    public interface IDomainContext {
        TService UseService<TService>(IUnitOfWorkContextFactory contextFactory) where TService : class, IDomainService;
    }
}
