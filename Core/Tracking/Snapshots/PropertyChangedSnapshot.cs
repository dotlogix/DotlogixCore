﻿// ==================================================
// Copyright 2018(C) , DotLogix
// File:  PropertyChangedSnapshot.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DotLogix.Core.Reflection.Dynamics;
#endregion

namespace DotLogix.Core.Tracking.Snapshots {
    public class PropertyChangedSnapshot : ISnapshot {
        private readonly Dictionary<string, TrackedValue> _changedValues = new Dictionary<string, TrackedValue>();
        private readonly Dictionary<string, TrackedValue> _trackedValues;

        public INotifyPropertyChanged Target { get; }

        /// <summary>Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.</summary>
        public PropertyChangedSnapshot(INotifyPropertyChanged target, IEnumerable<DynamicAccessor> accessors) {
            Target = target;
            _trackedValues = accessors.ToDictionary(a => a.Name, a => new TrackedValue(target, a));

            target.PropertyChanged += Target_PropertyChanged;
        }

        object ISnapshot.Target => Target;
        public IReadOnlyDictionary<string, object> OldValues => _trackedValues.ToDictionary(kv => kv.Key, kv => kv.Value.OldValue);
        public IReadOnlyDictionary<string, object> CurrentValues => _trackedValues.ToDictionary(kv => kv.Key, kv => kv.Value.NewValue);
        public IReadOnlyDictionary<string, object> ChangedValues => _changedValues.ToDictionary(kv => kv.Key, kv => kv.Value.NewValue);


        public bool DetectChanges() {
            return _changedValues.Count > 0;
        }

        public void AcceptChanges() {
            foreach(var value in _changedValues.Values)
                value.AcceptChanges();
            _changedValues.Clear();
        }

        public void RevertChanges() {
            Target.PropertyChanged -= Target_PropertyChanged;

            foreach(var value in _changedValues.Values)
                value.RevertChanges();
            _changedValues.Clear();

            Target.PropertyChanged += Target_PropertyChanged;
        }

        private void Target_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            var name = e.PropertyName;
            if(_trackedValues.TryGetValue(name, out var value) == false)
                return;

            if(value.HasChanged)
                _changedValues.Add(name, value);
            else
                _changedValues.Remove(name);
        }
    }
}
