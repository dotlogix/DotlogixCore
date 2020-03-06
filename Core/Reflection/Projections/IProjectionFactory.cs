// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IProjectionFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
#endregion

namespace DotLogix.Core.Reflection.Projections {
    /// <summary>
    /// An interface for a factory producing projections
    /// </summary>
    public interface IProjectionFactory {
        /// <summary>
        /// Create the projections to map the types
        /// </summary>
        IEnumerable<IProjection> CreateProjections(Type left, Type right);
    }
}
