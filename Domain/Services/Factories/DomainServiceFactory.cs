// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DomainServiceFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Architecture.Domain.Context;
using DotLogix.Architecture.Domain.UoW;
#endregion

namespace DotLogix.Architecture.Domain.Services.Factories {
    /// <summary>
    /// An implementation of the <see cref="IDomainServiceFactory"/>
    /// </summary>
    public class DomainServiceFactory : IDomainServiceFactory {
        /// <summary>
        /// The internal callback to create the new instance
        /// </summary>
        protected Func<IDomainContext, IUnitOfWorkContextFactory, IDomainService> CreateDomainServiceFunc { get; }

        /// <summary>
        /// Creates a new instance of <see cref="DomainServiceFactory"/>
        /// </summary>
        public DomainServiceFactory(Func<IDomainContext, IUnitOfWorkContextFactory, IDomainService> createDomainServiceFunc) {
            CreateDomainServiceFunc = createDomainServiceFunc;
        }

        /// <inheritdoc />
        public IDomainService Create(IDomainContext domainContext, IUnitOfWorkContextFactory uowContextFactory) {
            return CreateDomainServiceFunc.Invoke(domainContext, uowContextFactory);
        }
    }
}
