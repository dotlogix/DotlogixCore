// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IChangeTrackingEntryManager.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
using DotLogix.Core.Tracking.Entries;
#endregion

namespace DotLogix.Core.Tracking.Manager {
    public interface IChangeTrackingEntryManager {
        IEnumerable<IChangeTrackingEntry> Entries { get; }

        IChangeTrackingEntry GetEntry(object target);
        IChangeTrackingEntry EnsureEntry(object target, bool autoAttach);
        bool TryGetEntry(object target, out IChangeTrackingEntry entry);


        void Add(IChangeTrackingEntry changeTrackingEntry);
        bool Remove(IChangeTrackingEntry changeTrackingEntry);
    }
}
