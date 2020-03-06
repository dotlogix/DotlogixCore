// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IChangeTrackingEntry.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
#endregion

namespace DotLogix.Core.Tracking.Entries {
    /// <summary>
    /// A change tracking entry
    /// </summary>
    public interface IChangeTrackingEntry {
        /// <summary>
        /// The old values
        /// </summary>
        IReadOnlyDictionary<string, object> OldValues { get; }
        /// <summary>
        /// The changed values
        /// </summary>
        IReadOnlyDictionary<string, object> ChangedValues { get; }
        /// <summary>
        /// The current values
        /// </summary>
        IReadOnlyDictionary<string, object> CurrentValues { get; }
        /// <summary>
        /// The current state
        /// </summary>
        TrackedState CurrentState { get; }
        /// <summary>
        /// The key
        /// </summary>
        object Key { get; }
        /// <summary>
        /// The target
        /// </summary>
        object Target { get; }

        /// <summary>
        /// Mark as added
        /// </summary>
        void MarkAsAdded();
        /// <summary>
        /// Mark as deleted
        /// </summary>
        void MarkAsDeleted();
        /// <summary>
        /// Mark as modified
        /// </summary>
        void MarkAsModified();
        /// <summary>
        /// Detach self from entry manager
        /// </summary>
        void Attach();
        /// <summary>
        /// Detach self from entry manager
        /// </summary>
        void Detach();
        /// <summary>
        /// Accept all changes and return to unmodified state
        /// </summary>
        void Reset();
        /// <summary>
        /// Revert all changes
        /// </summary>
        void RevertChanges();
    }
}
