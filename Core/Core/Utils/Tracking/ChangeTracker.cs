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
using DotLogix.Core.Utils.Tracking.Entries;
using DotLogix.Core.Utils.Tracking.Manager;
using DotLogix.Core.Utils.Tracking.Snapshots;
#endregion

namespace DotLogix.Core.Utils.Tracking {
    /// <inheritdoc />
    public class ChangeTracker : IChangeTracker {
        /// <summary>
        /// The inner dictionary to hold data
        /// </summary>
        protected readonly Dictionary<Type, IChangeTrackingEntryManager> EntryManagers = new();

        /// <inheritdoc />
        public IEnumerable<IChangeTrackingEntry> Entries => EntryManagers.Values.SelectMany(em => em.Entries);

        /// <inheritdoc />
        public void MarkAsAdded(object target) {
            EnsureEntry(target).MarkAsAdded();
        }

        /// <inheritdoc />
        public void MarkAsDeleted(object target) {
            EnsureEntry(target).MarkAsDeleted();
        }

        /// <inheritdoc />
        public void MarkAsModified(object target) {
            EnsureEntry(target).MarkAsModified();
        }

        /// <inheritdoc />
        public void Attach(object target) {
            EnsureEntry(target);
        }

        /// <inheritdoc />
        public void Detach(object target) {
            var type = target.GetType();
            if(EntryManagers.TryGetValue(type, out var entryManager) && entryManager.TryGetEntry(target, out var entry))
                entryManager.Remove(entry);
        }

        /// <inheritdoc />
        public IChangeTrackingEntry Entry(object target) {
            return EnsureEntry(target, false);
        }

        /// <summary>
        /// Ensure an entry exists
        /// </summary>
        protected IChangeTrackingEntry EnsureEntry(object target, bool autoAttach = true) {
            var entryManager = EnsureEntryManager(target);
            return entryManager.EnsureEntry(target, autoAttach);
        }

        /// <summary>
        /// Ensure an entry manager exists
        /// </summary>
        protected IChangeTrackingEntryManager EnsureEntryManager(object target) {
            var type = target.GetType();
            if(EntryManagers.TryGetValue(type, out var entryManager))
                return entryManager;

            entryManager = CreateEntryManager(type);
            EntryManagers.Add(type, entryManager);
            return entryManager;
        }

        /// <summary>
        /// Create an entry manager
        /// </summary>
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

                if(keyAccessor is not null)
                    throw new InvalidOperationException("Multiple change tracking keys are set for this type of entity you have to choose only one");
                keyAccessor = dynamicProp;
            }

            if(keyAccessor == null)
                throw new InvalidOperationException("You have to select a key for change tracking");

            var snapshotFactory = CreateSnapshotFactory(type, trackedAccessors);
            return new ChangeTrackingEntryManager(snapshotFactory, keyAccessor);
        }

        /// <summary>
        /// Create a snapshot factory
        /// </summary>
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