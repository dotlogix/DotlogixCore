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
    /// <summary>
    /// A factory to create snapshots
    /// </summary>
    public interface ISnapshotFactory {
        /// <summary>
        /// Create a snapshot
        /// </summary>
        ISnapshot CreateSnapshot(object target);
    }

    /// <inheritdoc />
    public class DiffSnapshotFactory : ISnapshotFactory {
        private readonly DynamicAccessor[] _accessors;

        /// <summary>
        /// Create a new instance of <see cref="DiffSnapshotFactory"/>
        /// </summary>
        /// <param name="accessors"></param>
        public DiffSnapshotFactory(IEnumerable<DynamicAccessor> accessors) {
            _accessors = accessors.ToArray();
        }

        /// <inheritdoc />
        public ISnapshot CreateSnapshot(object target) {
            return new DiffSnapshot(target, _accessors);
        }
    }

    /// <inheritdoc />
    public class PropertyChangedSnapshotFactory : ISnapshotFactory {
        private readonly DynamicAccessor[] _accessors;

        /// <summary>
        /// Create a new instance of <see cref="PropertyChangedSnapshotFactory"/>
        /// </summary>
        /// <param name="accessors"></param>
        public PropertyChangedSnapshotFactory(IEnumerable<DynamicAccessor> accessors) {
            _accessors = accessors.ToArray();
        }

        /// <inheritdoc />
        public ISnapshot CreateSnapshot(object target) {
            var npc = (INotifyPropertyChanged)target;
            return new PropertyChangedSnapshot(npc, _accessors);
        }
    }
}
