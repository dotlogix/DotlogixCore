// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DomainServiceProvider.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using DotLogix.Architecture.Domain.Context;
using DotLogix.Architecture.Domain.Services.Factories;
using DotLogix.Architecture.Domain.UoW;
#endregion

namespace DotLogix.Architecture.Domain.Services.Providers {
    public class DomainServiceProvider : IDomainServiceProvider {
        protected Dictionary<Type, IDomainServiceFactory> ServiceFactories { get; } = new Dictionary<Type, IDomainServiceFactory>();


        public virtual TService Create<TService>(IDomainContext domainContext, IUnitOfWorkContextFactory uowContextFactory) where TService : IDomainService {
            if(ServiceFactories.TryGetValue(typeof(TService), out var factory) == false)
                throw new ArgumentException($"Type {typeof(TService)} is not registered for this provider", nameof(TService));
            return (TService)factory.Create(domainContext, uowContextFactory);
        }

        public virtual void RegisterFactory(Type domainServiceType, IDomainServiceFactory factory) {
            ServiceFactories.Add(domainServiceType, factory);
        }

        public virtual void RegisterFactory(Type domainServiceType) {
            var factory = DynamicDomainServiceFactory.CreateFor(domainServiceType);
            RegisterFactory(domainServiceType, factory);
        }
    }
}
