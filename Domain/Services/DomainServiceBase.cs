// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DomainServiceBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Architecture.Domain.Context;
using DotLogix.Architecture.Domain.UoW;
#endregion

namespace DotLogix.Architecture.Domain.Services {
    /// <summary>
    /// An implementation of the <see cref="IDomainService"/> interface
    /// </summary>
    public abstract class DomainServiceBase : IDomainService {
        /// <summary>
        /// The internal domain context
        /// </summary>
        protected IDomainContext DomainContext { get; }

        /// <summary>
        /// The internal unit of work factory
        /// </summary>
        protected IUnitOfWorkContextFactory UowContextFactory { get; }

        /// <summary>
        /// Creates a new instance of <see cref="DomainServiceBase"/>
        /// </summary>
        protected DomainServiceBase(IDomainContext domainContext, IUnitOfWorkContextFactory uowContextFactory) {
            DomainContext = domainContext;
            UowContextFactory = uowContextFactory;
        }

        /// <summary>
        /// Begin a new unit of work context
        /// </summary>
        protected IUnitOfWorkContext BeginContext() {
            return UowContextFactory.BeginContext();
        }

        /// <summary>
        /// Get another service running in a nested unit of work context
        /// </summary>
        protected TService UseService<TService>(IUnitOfWorkContextFactory uowContextFactory) where TService : class, IDomainService {
            return DomainContext.UseService<TService>(uowContextFactory);
        }
    }
}
