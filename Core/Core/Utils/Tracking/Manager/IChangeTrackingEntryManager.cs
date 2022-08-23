// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IChangeTrackingEntryManager.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
using DotLogix.Core.Utils.Tracking.Entries;
#endregion

namespace DotLogix.Core.Utils.Tracking.Manager {
    /// <summary>
    /// An interface for change tracking managers
    /// </summary>
    public interface IChangeTrackingEntryManager {
        /// <summary>
        /// The entries
        /// </summary>
        IEnumerable<IChangeTrackingEntry> Entries { get; }

        /// <summary>
        /// Get a entry of a target value
        /// </summary>
        IChangeTrackingEntry GetEntry(object target);
        /// <summary>
        /// Ensure a entry for a target value
        /// </summary>
        IChangeTrackingEntry EnsureEntry(object target, bool autoAttach);
        /// <summary>
        /// Tries to get a entry of a target value
        /// </summary>
        bool TryGetEntry(object target, out IChangeTrackingEntry entry);

        /// <summary>
        /// Add a entry to the manager
        /// </summary>
        void Add(IChangeTrackingEntry changeTrackingEntry);
        /// <summary>
        /// Remove a entry to the manager
        /// </summary>
        bool Remove(IChangeTrackingEntry changeTrackingEntry);
    }
}