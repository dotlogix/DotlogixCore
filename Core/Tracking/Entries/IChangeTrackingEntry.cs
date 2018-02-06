using System.Collections.Generic;

namespace DotLogix.Core.Tracking.Entries
{
    public interface IChangeTrackingEntry
    {
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