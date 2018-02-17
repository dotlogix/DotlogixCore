// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ApplicationContextFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  07.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using DotLogix.Architecture.Domain.Context.Factories;
using DotLogix.Architecture.Domain.UoW;
#endregion

namespace DotLogix.Architecture.Application.Context.Factories {
    public class ApplicationContextFactory : IApplicationContextFactory {
        private readonly IDomainContextFactory _domainContextFactory;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public ApplicationContextFactory(IDomainContextFactory domainContextFactory, IUnitOfWorkFactory unitOfWorkFactory) {
            _domainContextFactory = domainContextFactory;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public IApplicationContext Create() {
            return new ApplicationContext(_domainContextFactory.Create(), _unitOfWorkFactory.Create());
        }
    }
}
