// ==================================================
// Copyright 2018(C) , DotLogix
// File:  TrackedValue.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Core.Reflection.Dynamics;
#endregion

namespace DotLogix.Core.Utils.Tracking.Snapshots; 

/// <summary>
/// A representation of a change tracked value accessor
/// </summary>
public class TrackedValue {
    /// <summary>
    /// The dynamic accessor
    /// </summary>
    public DynamicAccessor DynamicAccessor { get; }
    /// <summary>
    /// The object target
    /// </summary>
    public object Target { get; }
    /// <summary>
    /// The name
    /// </summary>
    public string Name => DynamicAccessor.Name;
    /// <summary>
    /// The old value
    /// </summary>
    public object OldValue { get; private set; }
    /// <summary>
    /// The current value
    /// </summary>
    public object NewValue => DynamicAccessor.GetValue(Target);
    /// <summary>
    /// Check if the value has changed
    /// </summary>
    public bool HasChanged => Equals(OldValue, NewValue) == false;

    /// <summary>
    /// Creates a new instance of <see cref="TrackedValue"/>
    /// </summary>
    public TrackedValue(object target, DynamicAccessor dynamicAccessor) {
        Target = target;
        DynamicAccessor = dynamicAccessor;
        OldValue = dynamicAccessor.GetValue(target);
    }

    /// <summary>
    /// Accept changes
    /// </summary>
    public void AcceptChanges() {
        OldValue = NewValue;
    }

    /// <summary>
    /// Revert changes
    /// </summary>
    public void RevertChanges() {
        DynamicAccessor.SetValue(Target, OldValue);
    }
}