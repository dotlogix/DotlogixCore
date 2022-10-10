// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ISnapshot.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
#endregion

namespace DotLogix.Core.Tracking.Snapshots {
    /// <summary>
    /// A change tracking snapshot of an object
    /// </summary>
    public interface ISnapshot {
        /// <summary>
        /// The target value
        /// </summary>
        object Target { get; }

        /// <summary>
        /// The old values
        /// </summary>
        IReadOnlyDictionary<string, object> OldValues { get; }

        /// <summary>
        /// The current values
        /// </summary>
        IReadOnlyDictionary<string, object> CurrentValues { get; }

        /// <summary>
        /// The changed values
        /// </summary>
        IReadOnlyDictionary<string, object> ChangedValues { get; }

        /// <summary>
        /// Detect if a change has occured
        /// </summary>
        /// <returns></returns>
        bool DetectChanges();
        /// <summary>
        /// Accept changes
        /// </summary>
        void AcceptChanges();
        /// <summary>
        /// Revert changes
        /// </summary>
        void RevertChanges();
    }
}
