// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DomainContextFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using DotLogix.Architecture.Domain.Services.Providers;
#endregion

namespace DotLogix.Architecture.Domain.Context.Factories {
    public class DomainContextFactory : IDomainContextFactory {
        protected IDomainServiceProvider DomainServiceProvider { get; }

        public DomainContextFactory(IDomainServiceProvider domainServiceProvider) {
            DomainServiceProvider = domainServiceProvider;
        }

        public virtual IDomainContext Create() {
            return new DomainContext(DomainServiceProvider);
        }
    }
}
