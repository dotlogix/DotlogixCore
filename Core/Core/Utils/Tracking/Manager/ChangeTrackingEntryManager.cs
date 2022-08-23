// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ChangeTrackingEntryManager.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Utils.Tracking.Entries;
using DotLogix.Core.Utils.Tracking.Snapshots;
#endregion

namespace DotLogix.Core.Utils.Tracking.Manager; 

/// <inheritdoc />
public class ChangeTrackingEntryManager : IChangeTrackingEntryManager {
    private readonly Dictionary<object, IChangeTrackingEntry> _entryDict = new();
    private readonly DynamicAccessor _keyAccessors;
    private readonly ISnapshotFactory _snapshotFactory;

    /// <summary>
    /// Creates a new instance of <see cref="ChangeTrackingEntryManager"/>
    /// </summary>
    public ChangeTrackingEntryManager(ISnapshotFactory snapshotFactory, DynamicAccessor keyAccessors) {
        _snapshotFactory = snapshotFactory;
        _keyAccessors = keyAccessors;
    }

    /// <inheritdoc />
    public IEnumerable<IChangeTrackingEntry> Entries => _entryDict.Values;

    /// <inheritdoc />
    public IChangeTrackingEntry GetEntry(object key) {
        return TryGetEntry(key, out var entry) ? entry : null;
    }

    /// <inheritdoc />
    public bool TryGetEntry(object target, out IChangeTrackingEntry entry) {
        var key = CalculateKey(target);
        return _entryDict.TryGetValue(key, out entry);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public void Add(IChangeTrackingEntry changeTrackingEntry) {
        _entryDict.Add(changeTrackingEntry.Key, changeTrackingEntry);
    }

    /// <inheritdoc />
    public bool Remove(IChangeTrackingEntry changeTrackingEntry) {
        return _entryDict.Remove(changeTrackingEntry.Key);
    }

    private object CalculateKey(object target) {
        return _keyAccessors.GetValue(target);
    }
}