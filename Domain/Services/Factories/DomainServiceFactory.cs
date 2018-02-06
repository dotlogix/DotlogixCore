using System;
using DotLogix.Architecture.Domain.Context;
using DotLogix.Architecture.Infrastructure.UoW;

namespace DotLogix.Architecture.Domain.Services.Factories {
    public class DomainServiceFactory : IDomainServiceFactory {
        private readonly Func<IDomainContext, IUnitOfWorkContextFactory, IDomainService> _createDomainServiceFunc;
        public DomainServiceFactory(Func<IDomainContext, IUnitOfWorkContextFactory, IDomainService> createDomainServiceFunc) {
            _createDomainServiceFunc = createDomainServiceFunc;
        }

        public IDomainService Create(IDomainContext domainContext, IUnitOfWorkContextFactory uowContextFactory) {
            return _createDomainServiceFunc.Invoke(domainContext, uowContextFactory);
        }
    }
}