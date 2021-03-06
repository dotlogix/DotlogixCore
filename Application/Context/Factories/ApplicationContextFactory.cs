﻿// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ApplicationContextFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Architecture.Domain.Context.Factories;
using DotLogix.Architecture.Domain.UoW;
#endregion

namespace DotLogix.Architecture.Application.Context.Factories {
    /// <summary>
    /// An implementation of the <see cref="IApplicationContextFactory"/> interface
    /// </summary>
    public class ApplicationContextFactory : IApplicationContextFactory {
        private readonly IDomainContextFactory _domainContextFactory;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>
        /// Creates a new instance of <see cref="ApplicationContextFactory"/>
        /// </summary>
        public ApplicationContextFactory(IDomainContextFactory domainContextFactory, IUnitOfWorkFactory unitOfWorkFactory) {
            _domainContextFactory = domainContextFactory;
            _unitOfWorkFactory = unitOfWorkFactory;
        }
        ///<inheritdoc/>
        public IApplicationContext Create() {
            return new ApplicationContext(_domainContextFactory.Create(), _unitOfWorkFactory.Create());
        }
    }
}
