// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IChangeTracker.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Collections.Generic;
using DotLogix.Core.Tracking.Entries;
#endregion

namespace DotLogix.Core.Tracking {
    public interface IChangeTracker {
        IEnumerable<IChangeTrackingEntry> Entries { get; }
        void MarkAsAdded(object target);
        void MarkAsDeleted(object target);
        void MarkAsModified(object target);
        void Attach(object target);
        void Detach(object target);
        IChangeTrackingEntry Entry(object target);
    }
}
