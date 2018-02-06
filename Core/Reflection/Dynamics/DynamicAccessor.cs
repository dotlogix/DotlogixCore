// ==================================================
// Copyright 2016(C) , DotLogix
// File:  DynamicAccessor.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.09.2017
// LastEdited:  11.10.2017
// ==================================================

#region
using System;
using System.Reflection;
#endregion

namespace DotLogix.Core.Reflection.Dynamics {
    public abstract class DynamicAccessor {
        public string Name { get; }
        public Type DeclaringType { get; }
        public Type ReflectedType { get; }
        public Type ValueType { get; }
        public MemberInfo MemberInfo { get; }
        public AccessorTypes AccessorType { get; }
        public ValueAccessModes ValueAccessMode { get; }
        public DynamicSetter Setter { get; }
        public DynamicGetter Getter { get; }
        public bool CanRead => (ValueAccessMode & ValueAccessModes.Read) != 0;
        public bool CanWrite => (ValueAccessMode & ValueAccessModes.Write) != 0;


        protected DynamicAccessor(MemberInfo memberInfo, DynamicSetter setter, DynamicGetter getter, Type valueType, AccessorTypes accessorType) {
            MemberInfo = memberInfo ?? throw new ArgumentNullException(nameof(memberInfo));
            ValueType = valueType ?? throw new ArgumentNullException(nameof(valueType));
            Name = MemberInfo.Name;
            DeclaringType = memberInfo.DeclaringType;
            ReflectedType = memberInfo.ReflectedType;
            AccessorType = accessorType;
            var accessMode = ValueAccessModes.None;
            if(getter != null) {
                Getter = getter;
                accessMode |= ValueAccessModes.Read;
            }
            if(setter != null) {
                Setter = setter;
                accessMode |= ValueAccessModes.Write;
            }
            ValueAccessMode = accessMode;
        }


        public void SetValue(object value) {
            if((ValueAccessMode & ValueAccessModes.Write) == 0)
                throw new InvalidOperationException("You can not write to this accessor");
            Setter.SetValue(value);
        }

        public void SetValue(object instance, object value) {
            if((ValueAccessMode & ValueAccessModes.Write) == 0)
                throw new InvalidOperationException("You can not write to this accessor");
            Setter.SetValue(instance, value);
        }

        public object GetValue() {
            if((ValueAccessMode & ValueAccessModes.Read) == 0)
                throw new InvalidOperationException("You can not read from this accessor");
            return Getter.GetValue();
        }

        public object GetValue(object instance) {
            if((ValueAccessMode & ValueAccessModes.Read) == 0)
                throw new InvalidOperationException("You can not read from this accessor");
            return Getter.GetValue(instance);
        }

        public override string ToString() {
            switch(ValueAccessMode) {
                case ValueAccessModes.None:
                    return $"{DeclaringType.Name}.{Name}";
                case ValueAccessModes.Read:
                    return $"{DeclaringType.Name}.{Name}{{get;}}";
                case ValueAccessModes.Write:
                    return $"{DeclaringType.Name}.{Name}{{set;}}";
                case ValueAccessModes.ReadWrite:
                    return $"{DeclaringType.Name}.{Name}{{get; set;}}";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
