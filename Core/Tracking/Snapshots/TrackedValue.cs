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

namespace DotLogix.Core.Tracking.Snapshots {
    public class TrackedValue {
        public DynamicAccessor DynamicAccessor { get; }
        public object Target { get; }
        public string Name => DynamicAccessor.Name;
        public object OldValue { get; private set; }
        public object NewValue => DynamicAccessor.GetValue(Target);
        public bool HasChanged => Equals(OldValue, NewValue) == false;

        public TrackedValue(object target, DynamicAccessor dynamicAccessor) {
            Target = target;
            DynamicAccessor = dynamicAccessor;
            OldValue = dynamicAccessor.GetValue(target);
        }

        public void AcceptChanges() {
            OldValue = NewValue;
        }

        public void RevertChanges() {
            DynamicAccessor.SetValue(Target, OldValue);
        }
    }
}
