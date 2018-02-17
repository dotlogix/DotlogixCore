// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IChangeTrackingEntry.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Collections.Generic;
#endregion

namespace DotLogix.Core.Tracking.Entries {
    public interface IChangeTrackingEntry {
        IReadOnlyDictionary<string, object> OldValues { get; }
        IReadOnlyDictionary<string, object> ChangedValues { get; }
        IReadOnlyDictionary<string, object> CurrentValues { get; }
        TrackedState CurrentState { get; }
        object Key { get; }
        object Target { get; }

        void MarkAsAdded();
        void MarkAsDeleted();
        void MarkAsModified();
        void Attach();
        void Detach();
        void Reset();
        void RevertChanges();
    }
}
