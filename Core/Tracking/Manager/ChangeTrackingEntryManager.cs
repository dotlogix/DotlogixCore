using System.Collections.Generic;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Tracking.Entries;
using DotLogix.Core.Tracking.Snapshots;

namespace DotLogix.Core.Tracking.Manager {
    public class ChangeTrackingEntryManager : IChangeTrackingEntryManager {
        private readonly Dictionary<object, IChangeTrackingEntry> _entryDict=new Dictionary<object, IChangeTrackingEntry>();
        private readonly ISnapshotFactory _snapshotFactory;
        private readonly DynamicAccessor _keyAccessors;

        public ChangeTrackingEntryManager(ISnapshotFactory snapshotFactory, DynamicAccessor keyAccessors) {
            this._snapshotFactory = snapshotFactory;
            this._keyAccessors = keyAccessors;
        }
        public IEnumerable<IChangeTrackingEntry> Entries => _entryDict.Values;
        public IChangeTrackingEntry GetEntry(object key) {
            return TryGetEntry(key, out var entry) ? entry : null;
        }

        public bool TryGetEntry(object target, out IChangeTrackingEntry entry) {
            var key = CalculateKey(target);
            return _entryDict.TryGetValue(key, out entry);
        }

        public IChangeTrackingEntry EnsureEntry(object target, bool autoAttach) {
            var key = CalculateKey(target);
            if(_entryDict.TryGetValue(key, out var entry))
                return entry;

            var snapshot = _snapshotFactory.CreateSnapshot(target);
            entry = new ChangeTrackingEntry(key, this, snapshot, autoAttach ? TrackedState.Unchanged : TrackedState.Detached);
            if(autoAttach)
                _entryDict.Add(key, entry);

            return entry;
        }

        public void Add(IChangeTrackingEntry changeTrackingEntry) {
            _entryDict.Add(changeTrackingEntry.Key, changeTrackingEntry);
        }

        public bool Remove(IChangeTrackingEntry changeTrackingEntry) {
            return _entryDict.Remove(changeTrackingEntry.Key);
        }

        private object CalculateKey(object target) {
            return _keyAccessors.GetValue(target);
        }
    }
}