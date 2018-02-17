// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DynamicProperty.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
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
