// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DurationFilterModes.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Architecture.Infrastructure.Repositories.Enums {
    [Flags]
    public enum DurationFilterModes {
        Current = 1 << 0,
        InFuture = 1 << 1,
        Outdated = 1 << 2,
        All = Current | InFuture | Outdated
    }
}
