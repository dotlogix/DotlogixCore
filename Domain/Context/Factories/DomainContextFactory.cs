using DotLogix.Architecture.Domain.Services.Providers;

namespace DotLogix.Architecture.Domain.Context.Factories {
    public class DomainContextFactory : IDomainContextFactory
    {
        protected IDomainServiceProvider DomainServiceProvider { get; }

        public DomainContextFactory(IDomainServiceProvider domainServiceProvider) {
            DomainServiceProvider = domainServiceProvider;
        }

        public virtual IDomainContext Create()
        {
            return new DomainContext(DomainServiceProvider);
        }
    }
}