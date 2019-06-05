// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Snapshotter.cs
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
    /// A static class to create snapshots
    /// </summary>
    public static class Snapshotter {
        /// <summary>
        /// Create a snapshot
        /// </summary>
        public static ISnapshot CreateSnapshot(object target, AccessorTypes includedAccessors = AccessorTypes.Property) {
            var dynamicType = target.GetType().CreateDynamicType(includedAccessors.GetMemberTypes());
            var accessors = dynamicType.GetAccessors(includedAccessors).ToArray();
            return CreateSnapshot(target, accessors);
        }
        /// <summary>
        /// Create a snapshot
        /// </summary>
        public static ISnapshot CreateSnapshot(object target, IEnumerable<DynamicAccessor> accessors) {
            if(target is INotifyPropertyChanged npc)
                return new PropertyChangedSnapshot(npc, accessors);
            return new DiffSnapshot(target, accessors);
        }
    }
}
