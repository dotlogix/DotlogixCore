// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DynamicProperty.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Reflection;
#endregion

namespace DotLogix.Core.Reflection.Dynamics {
    /// <summary>
    /// A representation of a property
    /// </summary>
    public sealed class DynamicProperty : DynamicAccessor {
        /// <summary>
        /// The original property info
        /// </summary>
        public PropertyInfo PropertyInfo { get; }

        internal DynamicProperty(PropertyInfo propertyInfo, DynamicGetter getter, DynamicSetter setter) :
            base(propertyInfo, setter, getter, propertyInfo.PropertyType, AccessorTypes.Field) {
            PropertyInfo = propertyInfo;
        }
    }
}