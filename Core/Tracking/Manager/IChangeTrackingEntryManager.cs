using System.Collections.Generic;
using DotLogix.Core.Tracking.Entries;

namespace DotLogix.Core.Tracking.Manager
{
    public interface IChangeTrackingEntryManager
    {
        IEnumerable<IChangeTrackingEntry> Entries { get; }

        IChangeTrackingEntry GetEntry(object target);
        IChangeTrackingEntry EnsureEntry(object target, bool autoAttach);
        bool TryGetEntry(object target, out IChangeTrackingEntry entry);


        void Add(IChangeTrackingEntry changeTrackingEntry);
        bool Remove(IChangeTrackingEntry changeTrackingEntry);
    }
}