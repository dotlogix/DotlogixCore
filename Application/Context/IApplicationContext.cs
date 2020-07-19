// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IApplicationContext.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
using DotLogix.Architecture.Domain.Services;
#endregion

namespace DotLogix.Architecture.Application.Context {
    /// <summary>
    /// An interface to represent an application context
    /// </summary>
    public interface IApplicationContext {
        /// <summary>
        /// Get or create a service 
        /// </summary>
        TService UseService<TService>() where TService : class, IDomainService;
    }
}
