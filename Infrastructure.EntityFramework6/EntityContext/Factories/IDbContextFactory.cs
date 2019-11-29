// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IDbContextFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
#endregion

#region

using System.Data.Entity;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityFramework.EntityContext.Factories {
    /// <summary>
    ///     An interface to represent a factory to create a <see cref="DbContext" />
    /// </summary>
    public interface IDbContextFactory {
        /// <summary>
        ///     Creates a new instance of <see cref="DbContext" />
        /// </summary>
        DbContext Create();
    }
}
