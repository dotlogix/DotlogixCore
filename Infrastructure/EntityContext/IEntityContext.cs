// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IEntityContext.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  19.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Threading.Tasks;
using DotLogix.Core.Utils;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityContext {
    /// <summary>
    /// An interface to represent an entity context
    /// </summary>
    public interface IEntityContext : IEntitySetProvider, IDisposable {
        /// <summary>
        /// Complete the underlying unit of work and commit all changes to entities
        /// </summary>
        /// <returns></returns>
        Task CompleteAsync();
    }
}
