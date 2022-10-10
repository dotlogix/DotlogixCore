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
    /// <summary>
    /// An implementation of the <see cref="IDomainContext"/> interface
    /// </summary>
    public class DomainContext : IDomainContext {
        /// <summary>
        /// The internal domains service provider
        /// </summary>
        protected IDomainServiceProvider DomainServiceProvider { get; }

        /// <inheritdoc />
        public IDictionary<string, object> Variables { get; } = new Dictionary<string, object>();

        /// <summary>
        /// Creates a new instance of <see cref="DomainContext"/>
        /// </summary>
        public DomainContext(IDomainServiceProvider domainServiceProvider) {
            DomainServiceProvider = domainServiceProvider;
        }

        /// <inheritdoc />
        public virtual TService UseService<TService>(IUnitOfWorkContextFactory contextFactory) where TService : class, IDomainService {
            return DomainServiceProvider.Create<TService>(this, contextFactory);
        }
    }
}
