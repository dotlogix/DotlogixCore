﻿// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ApplicationContext.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
using DotLogix.Architecture.Domain.Context;
using DotLogix.Architecture.Domain.Services;
using DotLogix.Architecture.Domain.UoW;
#endregion

namespace DotLogix.Architecture.Application.Context {
    public class ApplicationContext : IApplicationContext {
        protected IDomainContext DomainContext { get; }
        protected IUnitOfWork UnitOfWork { get; }
        public IDictionary<string, object> Variables { get; } = new Dictionary<string, object>();

        public ApplicationContext(IDomainContext domainContext, IUnitOfWork unitOfWork) {
            DomainContext = domainContext;
            UnitOfWork = unitOfWork;
        }

        public virtual TService UseService<TService>() where TService : class, IDomainService {
            return DomainContext.UseService<TService>(UnitOfWork);
        }
    }
}
