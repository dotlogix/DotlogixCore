// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ApplicationContext.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Architecture.Domain.Context;
using DotLogix.Architecture.Domain.Services;
using DotLogix.Architecture.Domain.UoW;
#endregion

namespace DotLogix.Architecture.Application.Context {
    /// <summary>
    /// An implementation of the <see cref="IApplicationContext"/> interface
    /// </summary>
    public class ApplicationContext : IApplicationContext {
        /// <summary>
        /// The internal domain context
        /// </summary>
        protected IDomainContext DomainContext { get; }
        /// <summary>
        /// The internal unit of work
        /// </summary>
        protected IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// Creates a new instance of <see cref="ApplicationContext"/>
        /// </summary>
        public ApplicationContext(IDomainContext domainContext, IUnitOfWork unitOfWork) {
            DomainContext = domainContext;
            UnitOfWork = unitOfWork;
        }

        /// <inheritdoc />
        public virtual TService UseService<TService>() where TService : class, IDomainService {
            return DomainContext.UseService<TService>(UnitOfWork);
        }
    }
}
