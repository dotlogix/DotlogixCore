// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IProjectionFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
#endregion

namespace DotLogix.Core.Reflection.Projections {
    public interface IProjectionFactory {
        IEnumerable<IProjection> CreateProjections(Type left, Type right);
    }
}
