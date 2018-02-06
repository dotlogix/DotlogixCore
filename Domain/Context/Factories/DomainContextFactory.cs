using DotLogix.Architecture.Domain.Services.Providers;

namespace DotLogix.Architecture.Domain.Context.Factories {
    public class DomainContextFactory : IDomainContextFactory
    {
        private readonly IDomainServiceProvider _domainServiceProvider;
        public DomainContextFactory(IDomainServiceProvider domainServiceProvider) {
            _domainServiceProvider = domainServiceProvider;
        }

        public IDomainContext Create()
        {
            return new DomainContext(_domainServiceProvider);
        }
    }
}