using DotLogix.Architecture.Domain.Context;
using DotLogix.Architecture.Domain.UoW;

namespace DotLogix.Architecture.Domain.Services {
    public abstract class DomainServiceBase : IDomainService{
        private readonly IDomainContext _domainContext;
        private readonly IUnitOfWorkContextFactory _uowContextFactory;

        protected DomainServiceBase(IDomainContext domainContext, IUnitOfWorkContextFactory uowContextFactory) {
            _domainContext = domainContext;
            _uowContextFactory = uowContextFactory;
        }

        protected IUnitOfWorkContext BeginContext() {
            return _uowContextFactory.BeginContext();
        }
        protected TService UseService<TService>(IUnitOfWorkContextFactory uowContextFactory) where TService : class, IDomainService {
            return _domainContext.UseService<TService>(uowContextFactory);
        }
    }
}