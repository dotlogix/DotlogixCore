// ==================================================
// Copyright 2018(C) , DotLogix
// File:  TrackedState.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Core.Utils.Tracking.Entries {
    /// <summary>
    /// The change tracking state of an entry
    /// </summary>
    public enum TrackedState {
        /// <summary>
        /// Detached
        /// </summary>
        Detached,
        /// <summary>
        /// Unchanged
        /// </summary>
        Unchanged,
        /// <summary>
        /// Modified
        /// </summary>
        Modified,
        /// <summary>
        /// Added
        /// </summary>
        Added,
        /// <summary>
        /// Deleted
        /// </summary>
        Deleted
    }
}