// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ISnapshot.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Collections.Generic;
#endregion

namespace DotLogix.Core.Tracking.Snapshots {
    public interface ISnapshot {
        object Target { get; }
        IReadOnlyDictionary<string, object> OldValues { get; }
        IReadOnlyDictionary<string, object> CurrentValues { get; }
        IReadOnlyDictionary<string, object> ChangedValues { get; }
        bool DetectChanges();
        void AcceptChanges();
        void RevertChanges();
    }
}
