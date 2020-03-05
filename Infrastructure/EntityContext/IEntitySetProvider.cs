// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IEntitySetProvider.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  04.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Architecture.Infrastructure.Entities;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityContext {
    /// <summary>
    /// An interface to represent a provider of <see cref="IEntitySet{TEntity}"/>
    /// </summary>
    public interface IEntitySetProvider {
        /// <summary>
        /// Get or create an <see cref="IEntitySet{TEntity}"/>
        /// </summary>
        IEntitySet<TEntity> UseSet<TEntity>() where TEntity : class, new();
    }
}
