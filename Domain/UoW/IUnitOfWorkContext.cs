// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IUnitOfWorkContext.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Architecture.Infrastructure.Repositories;
#endregion

namespace DotLogix.Architecture.Domain.UoW {
    /// <summary>
    /// An interface to represent a unit of work context
    /// </summary>
    public interface IUnitOfWorkContext : IUnitOfWorkContextFactory, IDisposable {
        /// <summary>
        /// The entity context
        /// </summary>
        IEntityContext EntityContext { get; }
        
        /// <summary>
        /// Get or create an instance of <see cref="IRepository"/>
        /// </summary>
        TRepo UseRepository<TRepo>() where TRepo : IRepository;

        /// <summary>
        /// Complete this unit of work (in nested contexts this method will do nothing)
        /// </summary>
        Task CompleteAsync();
    }
}
