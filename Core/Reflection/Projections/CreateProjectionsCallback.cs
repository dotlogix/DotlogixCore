// ==================================================
// Copyright 2018(C) , DotLogix
// File:  CreateProjectionsCallback.cs
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
    /// A delegate to create projections from two types
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public delegate IEnumerable<IProjection> CreateProjectionsCallback(Type left, Type right);
}
