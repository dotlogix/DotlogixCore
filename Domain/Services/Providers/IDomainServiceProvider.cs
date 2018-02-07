using DotLogix.Architecture.Domain.Context;
using DotLogix.Architecture.Domain.UoW;

namespace DotLogix.Architecture.Domain.Services.Providers {
    public interface IDomainServiceProvider {
        TService Create<TService>(IDomainContext domainContext, IUnitOfWorkContextFactory uowContextFactory) where TService : IDomainService;
    }
}