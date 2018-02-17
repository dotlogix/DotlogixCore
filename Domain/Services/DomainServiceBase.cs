// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DomainServiceBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using DotLogix.Architecture.Domain.Context;
using DotLogix.Architecture.Domain.UoW;
#endregion

namespace DotLogix.Architecture.Domain.Services {
    public abstract class DomainServiceBase : IDomainService {
        protected IDomainContext DomainContext { get; }
        protected IUnitOfWorkContextFactory UowContextFactory { get; }

        protected DomainServiceBase(IDomainContext domainContext, IUnitOfWorkContextFactory uowContextFactory) {
            DomainContext = domainContext;
            UowContextFactory = uowContextFactory;
        }

        protected IUnitOfWorkContext BeginContext() {
            return UowContextFactory.BeginContext();
        }

        protected TService UseService<TService>(IUnitOfWorkContextFactory uowContextFactory) where TService : class, IDomainService {
            return DomainContext.UseService<TService>(uowContextFactory);
        }
    }
}
