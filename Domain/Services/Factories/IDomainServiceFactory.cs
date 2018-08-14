// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IDomainServiceFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Architecture.Domain.Context;
using DotLogix.Architecture.Domain.UoW;
#endregion

namespace DotLogix.Architecture.Domain.Services.Factories {
    public interface IDomainServiceFactory {
        IDomainService Create(IDomainContext domainContext, IUnitOfWorkContextFactory uowContextFactory);
    }
}
