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
    /// <summary>
    /// An implementation of the <see cref="IDomainServiceFactory"/> using reflection and il code to create instances
    /// </summary>
    public class DynamicDomainServiceFactory : IDomainServiceFactory {
        private readonly DynamicCtor _serviceCtor;
        /// <summary>
        /// Creates a new instance of <see cref="DynamicDomainServiceFactory"/>
        /// </summary>
        public DynamicDomainServiceFactory(DynamicCtor serviceCtor) {
            _serviceCtor = serviceCtor;
        }

        /// <inheritdoc />
        public IDomainService Create(IDomainContext domainContext, IUnitOfWorkContextFactory uowContextFactory) {
            return (IDomainService)_serviceCtor.Invoke(domainContext, uowContextFactory);
        }

        /// <summary>
        /// Create a new <see cref="DynamicDomainServiceFactory"/> using a constructor with the shape .ctor(<see cref="IDomainContext"/>, <see cref="IUnitOfWorkContextFactory"/>)
        /// </summary>
        public static IDomainServiceFactory CreateFor<TService>() where TService : IDomainService {
            return CreateFor(typeof(TService));
        }

        /// <summary>
        /// Create a new <see cref="DynamicDomainServiceFactory"/> using a constructor with the shape .ctor(<see cref="IDomainContext"/>, <see cref="IUnitOfWorkContextFactory"/>)
        /// </summary>
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
