// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ISnapshotFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
#endregion

namespace DotLogix.Core.Utils.Tracking.Snapshots {
    /// <summary>
    /// A factory to create snapshots
    /// </summary>
    public interface ISnapshotFactory {
        /// <summary>
        /// Create a snapshot
        /// </summary>
        ISnapshot CreateSnapshot(object target);
    }
}
