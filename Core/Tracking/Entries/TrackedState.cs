// ==================================================
// Copyright 2018(C) , DotLogix
// File:  TrackedState.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

namespace DotLogix.Core.Tracking.Entries {
    public enum TrackedState {
        Detached,
        Unchanged,
        Modified,
        Added,
        Deleted
    }
}
