// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IRepositoryFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using DotLogix.Architecture.Infrastructure.EntityContext;
#endregion

namespace DotLogix.Architecture.Infrastructure.Repositories.Factories {
    public interface IRepositoryFactory {
        IRepository Create(IEntitySetProvider entitySetProvider);
    }
}
