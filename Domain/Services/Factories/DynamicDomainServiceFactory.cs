// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DynamicDomainServiceFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Architecture.Domain.Context;
using DotLogix.Architecture.Domain.UoW;
using DotLogix.Core.Extensions;
using DotLogix.Core.Reflection.Dynamics;
#endregion

namespace DotLogix.Architecture.Domain.Services.Factories {
    public class DynamicDomainServiceFactory : IDomainServiceFactory {
        private readonly DynamicCtor _serviceCtor;

        public DynamicDomainServiceFactory(DynamicCtor serviceCtor) {
            _serviceCtor = serviceCtor;
        }

        public IDomainService Create(IDomainContext domainContext, IUnitOfWorkContextFactory uowContextFactory) {
            return (IDomainService)_serviceCtor.Invoke(domainContext, uowContextFactory);
        }

        public static IDomainServiceFactory CreateFor<TService>() where TService : IDomainService {
            return CreateFor(typeof(TService));
        }

        public static IDomainServiceFactory CreateFor(Type domainServiceType) {
            if(domainServiceType.IsAssignableTo(typeof(IDomainService)) == false)
                throw new ArgumentException($"Type {domainServiceType} is not assignable to type {nameof(IDomainService)}", nameof(domainServiceType));

            var repoCtor = domainServiceType.GetConstructor(new[] {typeof(IDomainContext), typeof(IUnitOfWorkContextFactory)});
            if(repoCtor == null)
                throw new ArgumentException($"Type {domainServiceType} doesnt have a valid constructor [.ctor({nameof(IDomainContext)}, {nameof(IUnitOfWorkContextFactory)})]", nameof(domainServiceType));
            var dynamicCtor = repoCtor.CreateDynamicCtor();
            if(dynamicCtor == null)
                throw new ArgumentException($"Can not create dynamic constructor for type {domainServiceType}");
            return new DynamicDomainServiceFactory(dynamicCtor);
        }
    }
}
