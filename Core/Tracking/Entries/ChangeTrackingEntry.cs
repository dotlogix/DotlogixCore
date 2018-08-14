// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ChangeTrackingEntry.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using DotLogix.Core.Tracking.Manager;
using DotLogix.Core.Tracking.Snapshots;
#endregion

namespace DotLogix.Core.Tracking.Entries {
    public class ChangeTrackingEntry : IChangeTrackingEntry {
        private readonly IChangeTrackingEntryManager _entryManager;
        private readonly ISnapshot _snapshot;

        private TrackedState _currentState;
        private bool _forceModified;

        public ChangeTrackingEntry(object key, IChangeTrackingEntryManager entryManager, ISnapshot snapshot, TrackedState initialState = TrackedState.Detached) {
            _entryManager = entryManager;
            Key = key;
            _snapshot = snapshot;
            _currentState = initialState;
        }

        public IReadOnlyDictionary<string, object> OldValues => _snapshot.OldValues;
        public IReadOnlyDictionary<string, object> CurrentValues => _snapshot.CurrentValues;
        public IReadOnlyDictionary<string, object> ChangedValues => _forceModified ? _snapshot.CurrentValues : _snapshot.ChangedValues;

        public object Key { get; }
        public object Target => _snapshot.Target;

        public TrackedState CurrentState {
            get {
                switch(_currentState) {
                    case TrackedState.Added:
                    case TrackedState.Deleted:
                    case TrackedState.Detached:
                        return _currentState;
                    case TrackedState.Unchanged:
                        if(_snapshot.DetectChanges())
                            _currentState = TrackedState.Modified;
                        return _currentState;
                    case TrackedState.Modified:
                        if((_forceModified || _snapshot.DetectChanges()) == false)
                            _currentState = TrackedState.Unchanged;
                        return _currentState;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void MarkAsAdded() {
            if(_currentState == TrackedState.Detached)
                Attach();
            _currentState = TrackedState.Added;
            _forceModified = false;
        }

        public void MarkAsDeleted() {
            switch(_currentState) {
                case TrackedState.Detached:
                    Attach();
                    break;
                case TrackedState.Added:
                    Detach();
                    return;
            }

            _currentState = TrackedState.Deleted;
            _forceModified = false;
        }

        public void MarkAsModified() {
            if(_currentState == TrackedState.Detached)
                Attach();

            _currentState = TrackedState.Modified;
            _forceModified = true;
        }

        public void Attach() {
            _entryManager.Add(this);
            if(_currentState == TrackedState.Detached)
                _currentState = TrackedState.Unchanged;
        }

        public void Detach() {
            _entryManager.Remove(this);
            _currentState = TrackedState.Detached;
            _forceModified = false;
        }

        public void RevertChanges() {
            _snapshot.RevertChanges();
            if(_currentState == TrackedState.Modified)
                _currentState = TrackedState.Unchanged;
            _forceModified = false;
        }

        public void Reset() {
            _snapshot.AcceptChanges();
            _currentState = TrackedState.Unchanged;
            _forceModified = false;
        }
    }
}
