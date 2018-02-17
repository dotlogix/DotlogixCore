// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DomainServiceFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using DotLogix.Architecture.Domain.Context;
using DotLogix.Architecture.Domain.UoW;
#endregion

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
