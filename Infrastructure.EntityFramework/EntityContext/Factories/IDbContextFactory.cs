// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IDbContextFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
#endregion

#region
using Microsoft.EntityFrameworkCore;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityFramework.EntityContext.Factories {
    public interface IDbContextFactory {
        DbContext Create();
    }
}
