using DotLogix.Architecture.Domain.Services;
using DotLogix.Architecture.Domain.Services.Providers;
using DotLogix.Architecture.Domain.UoW;

namespace DotLogix.Architecture.Domain.Context {
    public class DomainContext : IDomainContext {
        private readonly IDomainServiceProvider _domainServiceProvider;

        public DomainContext(IDomainServiceProvider domainServiceProvider) {
            _domainServiceProvider = domainServiceProvider;
        }

        public TService UseService<TService>(IUnitOfWorkContextFactory contextFactory) where TService : class, IDomainService {
            return _domainServiceProvider.Create<TService>(this, contextFactory);
        }
    }
}