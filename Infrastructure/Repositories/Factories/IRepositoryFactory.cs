// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IRepositoryFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Architecture.Infrastructure.EntityContext;
#endregion

namespace DotLogix.Architecture.Infrastructure.Repositories.Factories {
    /// <summary>
    /// An interface to represent a factory creating <see cref="IRepository"/>
    /// </summary>
    public interface IRepositoryFactory {
        /// <summary>
        /// Get or create an instance of <see cref="IRepository"/>
        /// </summary>
        IRepository Create(IEntityContext entityContext);
    }
}
