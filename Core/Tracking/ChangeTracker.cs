// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ChangeTracker.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using DotLogix.Core.Extensions;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Tracking.Entries;
using DotLogix.Core.Tracking.Manager;
using DotLogix.Core.Tracking.Snapshots;
#endregion

namespace DotLogix.Core.Tracking {
    public class ChangeTracker : IChangeTracker {
        protected readonly Dictionary<Type, IChangeTrackingEntryManager> EntryManagers = new Dictionary<Type, IChangeTrackingEntryManager>();

        public IEnumerable<IChangeTrackingEntry> Entries => EntryManagers.Values.SelectMany(em => em.Entries);

        public void MarkAsAdded(object target) {
            EnsureEntry(target).MarkAsAdded();
        }

        public void MarkAsDeleted(object target) {
            EnsureEntry(target).MarkAsDeleted();
        }

        public void MarkAsModified(object target) {
            EnsureEntry(target).MarkAsModified();
        }

        public void Attach(object target) {
            EnsureEntry(target);
        }

        public void Detach(object target) {
            var type = target.GetType();
            if(EntryManagers.TryGetValue(type, out var entryManager) && entryManager.TryGetEntry(target, out var entry))
                entryManager.Remove(entry);
        }

        public IChangeTrackingEntry Entry(object target) {
            return EnsureEntry(target, false);
        }

        protected IChangeTrackingEntry EnsureEntry(object target, bool autoAttach = true) {
            var entryManager = EnsureEntryManager(target);
            return entryManager.EnsureEntry(target, autoAttach);
        }

        protected IChangeTrackingEntryManager EnsureEntryManager(object target) {
            var type = target.GetType();
            if(EntryManagers.TryGetValue(type, out var entryManager))
                return entryManager;

            entryManager = CreateEntryManager(type);
            EntryManagers.Add(type, entryManager);
            return entryManager;
        }

        protected virtual IChangeTrackingEntryManager CreateEntryManager(Type type) {
            var trackedAccessors = new List<DynamicAccessor>();
            DynamicAccessor keyAccessor = null;

            foreach(var property in type.GetProperties()) {
                if(property.IsDefined(typeof(IgnoreChangesAttribute)))
                    continue;

                var dynamicProp = property.CreateDynamicProperty();
                trackedAccessors.Add(dynamicProp);

                if(property.IsDefined(typeof(ChangeTrackingKeyAttribute)) == false)
                    continue;

                if(keyAccessor != null)
                    throw new InvalidOperationException("Multiple change tracking keys are set for this type of entity you have to choose only one");
                keyAccessor = dynamicProp;
            }

            if(keyAccessor == null)
                throw new InvalidOperationException("You have to select a key for change tracking");

            var snapshotFactory = CreateSnapshotFactory(type, trackedAccessors);
            return new ChangeTrackingEntryManager(snapshotFactory, keyAccessor);
        }

        protected virtual ISnapshotFactory CreateSnapshotFactory(Type type, List<DynamicAccessor> trackedAccessors) {
            ISnapshotFactory snapshotFactory;
            if(type.IsAssignableTo(typeof(INotifyPropertyChanged)))
                snapshotFactory = new PropertyChangedSnapshotFactory(trackedAccessors);
            else
                snapshotFactory = new DiffSnapshotFactory(trackedAccessors);
            return snapshotFactory;
        }
    }
}
