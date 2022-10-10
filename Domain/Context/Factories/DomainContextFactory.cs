// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DomainContextFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Architecture.Domain.Services.Providers;
#endregion

namespace DotLogix.Architecture.Domain.Context.Factories {
    /// <summary>
    /// An implementation of the <see cref="IDomainContextFactory"/>
    /// </summary>
    public class DomainContextFactory : IDomainContextFactory {
        /// <summary>
        /// The internal domain context provider
        /// </summary>
        protected IDomainServiceProvider DomainServiceProvider { get; }

        /// <summary>
        /// Creates a new instance of <see cref="DomainContextFactory"/>
        /// </summary>
        public DomainContextFactory(IDomainServiceProvider domainServiceProvider) {
            DomainServiceProvider = domainServiceProvider;
        }


        /// <inheritdoc />
        public virtual IDomainContext Create() {
            return new DomainContext(DomainServiceProvider);
        }
    }
}
