// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Snapshotter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DotLogix.Core.Extensions;
using DotLogix.Core.Reflection.Dynamics;
#endregion

namespace DotLogix.Core.Utils.Tracking.Snapshots {
    /// <summary>
    /// A static class to create snapshots
    /// </summary>
    public static class Snapshotter {
        /// <summary>
        /// Create a snapshot
        /// </summary>
        public static ISnapshot CreateSnapshot(object target, AccessorTypes includedAccessors = AccessorTypes.Property) {
            var dynamicType = target.GetType().CreateDynamicType();

            var accessors = includedAccessors switch {
                AccessorTypes.None => Enumerable.Empty<DynamicAccessor>(),
                AccessorTypes.Property => dynamicType.Properties,
                AccessorTypes.Field => dynamicType.Fields,
                AccessorTypes.Any => dynamicType.Accessors,
                _ => throw new ArgumentOutOfRangeException(nameof(includedAccessors), includedAccessors, null)
            };
            return CreateSnapshot(target, accessors.AsArray());
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