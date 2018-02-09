using DotLogix.Architecture.Domain.Services;
using DotLogix.Architecture.Domain.Services.Providers;
using DotLogix.Architecture.Domain.UoW;

namespace DotLogix.Architecture.Domain.Context {
    public class DomainContext : IDomainContext {
        protected IDomainServiceProvider DomainServiceProvider { get; }

        public DomainContext(IDomainServiceProvider domainServiceProvider) {
            DomainServiceProvider = domainServiceProvider;
        }

        public TService UseService<TService>(IUnitOfWorkContextFactory contextFactory) where TService : class, IDomainService {
            return DomainServiceProvider.Create<TService>(this, contextFactory);
        }
    }
}