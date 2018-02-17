// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IRepositoryProvider.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using DotLogix.Architecture.Infrastructure.EntityContext;
#endregion

namespace DotLogix.Architecture.Infrastructure.Repositories.Provider {
    public interface IRepositoryProvider {
        TRepoInterface Create<TRepoInterface>(IEntityContext entityContext) where TRepoInterface : IRepository;
    }
}
