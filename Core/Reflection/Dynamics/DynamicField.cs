// ==================================================
// Copyright 2016(C) , DotLogix
// File:  DynamicField.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.09.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System.Reflection;
#endregion

namespace DotLogix.Core.Reflection.Dynamics {
    public sealed class DynamicField : DynamicAccessor {
        public FieldInfo FieldInfo { get; }

        internal DynamicField(FieldInfo fieldInfo, DynamicGetter getter, DynamicSetter setter) :
            base(fieldInfo, setter, getter, fieldInfo.FieldType, AccessorTypes.Field) {
            FieldInfo = fieldInfo;
        }
    }
}
