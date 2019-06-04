// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IDomainContext.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
using DotLogix.Architecture.Domain.Services;
using DotLogix.Architecture.Domain.UoW;
#endregion

namespace DotLogix.Architecture.Domain.Context {
    public interface IDomainContext {
        IDictionary<string, object> Variables { get; }
        TService UseService<TService>(IUnitOfWorkContextFactory contextFactory) where TService : class, IDomainService;
    }
}
