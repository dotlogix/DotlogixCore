// ==================================================
// Copyright 2017(C) , DotLogix
// File:  IDbContextFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  10.12.2017
// LastEdited:  10.12.2017
// ==================================================

#region
#endregion

using Microsoft.EntityFrameworkCore;

namespace DotLogix.Architecture.Infrastructure.EntityFramework.EntityContext.Factories {
    public interface IDbContextFactory {
        DbContext Create();
    }
}
