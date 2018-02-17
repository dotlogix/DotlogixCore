// ==================================================
// Copyright 2018(C) , DotLogix
// File:  RepositoryBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using DotLogix.Architecture.Infrastructure.EntityContext;
#endregion

namespace DotLogix.Architecture.Infrastructure.Repositories {
    public abstract class RepositoryBase<TEntityContext> : IRepository where TEntityContext : IEntityContext {
        protected TEntityContext EntityContext { get; }

        protected RepositoryBase(TEntityContext entityContext) {
            EntityContext = entityContext;
        }
    }
}
