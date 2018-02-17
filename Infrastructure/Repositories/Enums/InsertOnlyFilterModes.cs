// ==================================================
// Copyright 2018(C) , DotLogix
// File:  InsertOnlyFilterModes.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

namespace DotLogix.Architecture.Infrastructure.Repositories.Enums {
    public enum InsertOnlyFilterModes {
        Active = 1 << 0,
        InActive = 1 << 1,
        All = Active | InActive
    }
}
