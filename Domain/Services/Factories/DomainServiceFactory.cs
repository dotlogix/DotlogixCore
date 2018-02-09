using System;
using DotLogix.Architecture.Domain.Context;
using DotLogix.Architecture.Domain.UoW;

namespace DotLogix.Architecture.Domain.Services.Factories {
    public class DomainServiceFactory : IDomainServiceFactory {
        protected Func<IDomainContext, IUnitOfWorkContextFactory, IDomainService> CreateDomainServiceFunc { get; }

        public DomainServiceFactory(Func<IDomainContext, IUnitOfWorkContextFactory, IDomainService> createDomainServiceFunc) {
            CreateDomainServiceFunc = createDomainServiceFunc;
        }

        public IDomainService Create(IDomainContext domainContext, IUnitOfWorkContextFactory uowContextFactory) {
            return CreateDomainServiceFunc.Invoke(domainContext, uowContextFactory);
        }
    }
}