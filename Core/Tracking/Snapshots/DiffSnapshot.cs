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

namespace DotLogix.Core.Tracking.Snapshots {
    public class DiffSnapshot : ISnapshot {
        protected readonly TrackedValue[] TrackedValues;


        public DiffSnapshot(object target, IEnumerable<DynamicAccessor> accessors) {
            Target = target;
            TrackedValues = accessors.Select(a => new TrackedValue(target, a)).ToArray();
        }

        public object Target { get; }
        public IReadOnlyDictionary<string, object> OldValues => TrackedValues.ToDictionary(v => v.Name, v => v.OldValue);
        public IReadOnlyDictionary<string, object> CurrentValues => TrackedValues.ToDictionary(v => v.Name, v => v.NewValue);
        public IReadOnlyDictionary<string, object> ChangedValues => TrackedValues.Where(v => v.HasChanged).ToDictionary(v => v.Name, v => v.NewValue);


        public virtual bool DetectChanges() {
            return TrackedValues.Any(tv => tv.HasChanged);
        }

        public virtual void AcceptChanges() {
            foreach(var trackedValue in TrackedValues)
                trackedValue.AcceptChanges();
        }

        public virtual void RevertChanges() {
            foreach(var trackedValue in TrackedValues)
                trackedValue.RevertChanges();
        }
    }
}
