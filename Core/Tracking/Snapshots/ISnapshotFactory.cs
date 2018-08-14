// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ISnapshotFactory.cs
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
    public interface ISnapshotFactory {
        ISnapshot CreateSnapshot(object target);
    }

    public class DiffSnapshotFactory : ISnapshotFactory {
        private readonly DynamicAccessor[] _accessors;

        public DiffSnapshotFactory(IEnumerable<DynamicAccessor> accessors) {
            _accessors = accessors.ToArray();
        }

        public ISnapshot CreateSnapshot(object target) {
            return new DiffSnapshot(target, _accessors);
        }
    }

    public class PropertyChangedSnapshotFactory : ISnapshotFactory {
        private readonly DynamicAccessor[] _accessors;

        public PropertyChangedSnapshotFactory(IEnumerable<DynamicAccessor> accessors) {
            _accessors = accessors.ToArray();
        }

        public ISnapshot CreateSnapshot(object target) {
            var npc = (INotifyPropertyChanged)target;
            return new PropertyChangedSnapshot(npc, _accessors);
        }
    }
}
