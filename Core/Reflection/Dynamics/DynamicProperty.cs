// ==================================================
// Copyright 2016(C) , DotLogix
// File:  DynamicProperty.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.09.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System.Reflection;
#endregion

namespace DotLogix.Core.Reflection.Dynamics {
    public sealed class DynamicProperty : DynamicAccessor {
        public PropertyInfo PropertyInfo { get; }

        internal DynamicProperty(PropertyInfo propertyInfo, DynamicGetter getter, DynamicSetter setter) :
            base(propertyInfo, setter, getter, propertyInfo.PropertyType, AccessorTypes.Field) {
            PropertyInfo = propertyInfo;
        }
    }
}
