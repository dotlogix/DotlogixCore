// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IDomainContext.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  07.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using DotLogix.Architecture.Domain.Services;
using DotLogix.Architecture.Domain.UoW;
#endregion

namespace DotLogix.Architecture.Domain.Context {
    public interface IDomainContext {
        TService UseService<TService>(IUnitOfWorkContextFactory contextFactory) where TService : class, IDomainService;
    }
}
