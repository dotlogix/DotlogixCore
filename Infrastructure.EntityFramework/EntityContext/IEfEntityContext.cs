// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IEfEntityContext.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using DotLogix.Architecture.Infrastructure.EntityContext;
using Microsoft.EntityFrameworkCore;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityFramework.EntityContext {
    public interface IEfEntityContext : IEntityContext {
        DbContext DbContext { get; }
    }
}
