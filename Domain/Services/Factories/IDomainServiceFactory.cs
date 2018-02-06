using DotLogix.Architecture.Domain.Context;
using DotLogix.Architecture.Infrastructure.UoW;

namespace DotLogix.Architecture.Domain.Services.Factories {
    public interface IDomainServiceFactory {
        IDomainService Create(IDomainContext domainContext, IUnitOfWorkContextFactory uowContextFactory);
    }
}