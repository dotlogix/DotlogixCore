using DotLogix.Architecture.Domain.Context;
using DotLogix.Architecture.Domain.UoW;

namespace DotLogix.Architecture.Domain.Services {
    public abstract class DomainServiceBase : IDomainService{
        protected IDomainContext DomainContext { get; }
        protected IUnitOfWorkContextFactory UowContextFactory { get; }

        protected DomainServiceBase(IDomainContext domainContext, IUnitOfWorkContextFactory uowContextFactory) {
            DomainContext = domainContext;
            UowContextFactory = uowContextFactory;
        }

        protected IUnitOfWorkContext BeginContext() {
            return UowContextFactory.BeginContext();
        }
        protected TService UseService<TService>(IUnitOfWorkContextFactory uowContextFactory) where TService : class, IDomainService {
            return DomainContext.UseService<TService>(uowContextFactory);
        }
    }
}