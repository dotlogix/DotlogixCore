// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DiffSnapshot.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
using System.Linq;
using DotLogix.Core.Reflection.Dynamics;
#endregion

namespace DotLogix.Core.Utils.Tracking.Snapshots; 

/// <inheritdoc />
public class DiffSnapshot : ISnapshot {
    /// <summary>
    /// The tracked values
    /// </summary>
    protected readonly TrackedValue[] TrackedValues;

    /// <summary>
    /// Create a new instance of <see cref="DiffSnapshot"/>
    /// </summary>
    /// <param name="target"></param>
    /// <param name="accessors"></param>
    public DiffSnapshot(object target, IEnumerable<DynamicAccessor> accessors) {
        Target = target;
        TrackedValues = accessors.Select(a => new TrackedValue(target, a)).ToArray();
    }

    /// <inheritdoc />
    public object Target { get; }
    /// <inheritdoc />
    public IReadOnlyDictionary<string, object> OldValues => TrackedValues.ToDictionary(v => v.Name, v => v.OldValue);
    /// <inheritdoc />
    public IReadOnlyDictionary<string, object> CurrentValues => TrackedValues.ToDictionary(v => v.Name, v => v.NewValue);
    /// <inheritdoc />
    public IReadOnlyDictionary<string, object> ChangedValues => TrackedValues.Where(v => v.HasChanged).ToDictionary(v => v.Name, v => v.NewValue);


    /// <inheritdoc />
    public virtual bool DetectChanges() {
        return TrackedValues.Any(tv => tv.HasChanged);
    }

    /// <inheritdoc />
    public virtual void AcceptChanges() {
        foreach(var trackedValue in TrackedValues)
            trackedValue.AcceptChanges();
    }

    /// <inheritdoc />
    public virtual void RevertChanges() {
        foreach(var trackedValue in TrackedValues)
            trackedValue.RevertChanges();
    }
}