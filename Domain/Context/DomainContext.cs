// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DomainContext.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
using DotLogix.Architecture.Domain.Services;
using DotLogix.Architecture.Domain.Services.Providers;
using DotLogix.Architecture.Domain.UoW;
#endregion

namespace DotLogix.Architecture.Domain.Context {
    public class DomainContext : IDomainContext {
        protected IDomainServiceProvider DomainServiceProvider { get; }
        public IDictionary<string, object> Variables { get; } = new Dictionary<string, object>();
        public DomainContext(IDomainServiceProvider domainServiceProvider) {
            DomainServiceProvider = domainServiceProvider;
        }

        public virtual TService UseService<TService>(IUnitOfWorkContextFactory contextFactory) where TService : class, IDomainService {
            return DomainServiceProvider.Create<TService>(this, contextFactory);
        }
    }
}
