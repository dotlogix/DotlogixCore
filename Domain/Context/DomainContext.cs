// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DomainContext.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using DotLogix.Architecture.Domain.Services;
using DotLogix.Architecture.Domain.Services.Providers;
using DotLogix.Architecture.Domain.UoW;
#endregion

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
