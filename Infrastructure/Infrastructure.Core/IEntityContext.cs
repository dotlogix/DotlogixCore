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
using DotLogix.Core.Diagnostics;
#endregion

namespace DotLogix.Infrastructure {
    /// <summary>
    /// An interface to represent an entity context
    /// </summary>
    public interface IEntityContext : IEntitySetProvider, IDisposable {
        /// <summary>
        ///     The log source of the entity context
        /// </summary>
        ILogSource LogSource { get; }
        
        /// <summary>
        ///     The log source provider of the entity context
        /// </summary>
        ILogSourceProvider LogSourceProvider { get; }
        
        /// <summary>
        /// Complete the underlying unit of work and commit all changes to entities
        /// </summary>
        /// <returns></returns>
        Task<int> CompleteAsync();
    }
}
