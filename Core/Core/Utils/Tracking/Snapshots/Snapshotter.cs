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

            IEnumerable<DynamicAccessor> accessors;
            switch (includedAccessors) {
                case AccessorTypes.None:
                    accessors = Enumerable.Empty<DynamicAccessor>();
                    break;
                case AccessorTypes.Property:
                    accessors = dynamicType.Properties;
                    break;
                case AccessorTypes.Field:
                    accessors = dynamicType.Fields;
                    break;
                case AccessorTypes.Any:
                    accessors = dynamicType.Accessors;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(includedAccessors), includedAccessors, null);
            }
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
