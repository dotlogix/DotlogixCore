// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IDomainServiceProvider.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  07.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using DotLogix.Architecture.Domain.Context;
using DotLogix.Architecture.Domain.UoW;
#endregion

namespace DotLogix.Architecture.Domain.Services.Providers {
    public interface IDomainServiceProvider {
        TService Create<TService>(IDomainContext domainContext, IUnitOfWorkContextFactory uowContextFactory) where TService : IDomainService;
    }
}
