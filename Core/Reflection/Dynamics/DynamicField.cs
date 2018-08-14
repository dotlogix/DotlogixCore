// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DynamicField.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
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
