// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IChangeTracker.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
using DotLogix.Core.Tracking.Entries;
#endregion

namespace DotLogix.Core.Tracking {
    /// <summary>
    /// An interface for change tracking
    /// </summary>
    public interface IChangeTracker {
        /// <summary>
        /// The entries
        /// </summary>
        IEnumerable<IChangeTrackingEntry> Entries { get; }
        /// <summary>
        /// Mark an object as added
        /// </summary>
        void MarkAsAdded(object target);
        /// <summary>
        /// Mark an object as deleted
        /// </summary>
        void MarkAsDeleted(object target);
        /// <summary>
        /// Mark an object as modified
        /// </summary>
        void MarkAsModified(object target);
        /// <summary>
        /// Attach an object
        /// </summary>
        void Attach(object target);
        /// <summary>
        /// Detach an object
        /// </summary>
        void Detach(object target);

        /// <summary>
        /// Get the entry
        /// </summary>
        IChangeTrackingEntry Entry(object target);
    }
}
