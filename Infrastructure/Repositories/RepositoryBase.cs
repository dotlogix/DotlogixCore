// ==================================================
// Copyright 2016(C) , DotLogix
// File:  InstantiateRepo.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.08.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.EntityContext;
#endregion

namespace DotLogix.Architecture.Infrastructure.Repositories {
    public abstract class RepositoryBase<TEntityContext> : IRepository where TEntityContext:IEntityContext {
        protected TEntityContext EntityContext { get; }

        protected RepositoryBase(TEntityContext entityContext) {
            EntityContext = entityContext;
        }
    }
}
