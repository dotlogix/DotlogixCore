// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DynamicAccessor.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Reflection;
#endregion

namespace DotLogix.Core.Reflection.Dynamics {
    /// <summary>
    /// A representation of a value accessor
    /// </summary>
    public abstract class DynamicAccessor {
        /// <summary>
        /// The name
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// The declaring type
        /// </summary>
        public Type DeclaringType { get; }
        /// <summary>
        /// The reflected type
        /// </summary>
        public Type ReflectedType { get; }
        /// <summary>
        /// The value type
        /// </summary>
        public Type ValueType { get; }
        /// <summary>
        /// The original member info
        /// </summary>
        public MemberInfo MemberInfo { get; }
        /// <summary>
        /// The accessor type
        /// </summary>
        public AccessorTypes AccessorType { get; }
        /// <summary>
        /// The access modes
        /// </summary>
        public ValueAccessModes ValueAccessMode { get; }
        /// <summary>
        /// The setter delegate
        /// </summary>
        public DynamicSetter Setter { get; }
        /// <summary>
        /// The getter delegate
        /// </summary>
        public DynamicGetter Getter { get; }
        /// <summary>
        /// Check if the accessor is readable
        /// </summary>
        public bool CanRead => (ValueAccessMode & ValueAccessModes.Read) != 0;
        /// <summary>
        /// Check if the accessor is writable
        /// </summary>
        public bool CanWrite => (ValueAccessMode & ValueAccessModes.Write) != 0;


        /// <summary>
        /// Creates a new instance of <see cref="DynamicAccessor"/>
        /// </summary>
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

        /// <summary>
        /// Set the value of the accessor<br></br>
        /// The accessor must be static
        /// </summary>
        public void SetValue(object value) {
            if((ValueAccessMode & ValueAccessModes.Write) == 0)
                throw new InvalidOperationException("You can not write to this accessor");
            Setter.SetValue(value);
        }

        /// <summary>
        /// Set the value of the accessor
        /// </summary>
        public void SetValue(object instance, object value) {
            if((ValueAccessMode & ValueAccessModes.Write) == 0)
                throw new InvalidOperationException("You can not write to this accessor");
            Setter.SetValue(instance, value);
        }

        /// <summary>
        /// Get the value of the accessor<br></br>
        /// The accessor must be static
        /// </summary>

        public object GetValue() {
            if((ValueAccessMode & ValueAccessModes.Read) == 0)
                throw new InvalidOperationException("You can not read from this accessor");
            return Getter.GetValue();
        }

        /// <summary>
        /// Get the value of the accessor<br></br>
        /// </summary>

        public object GetValue(object instance) {
            if((ValueAccessMode & ValueAccessModes.Read) == 0)
                throw new InvalidOperationException("You can not read from this accessor");
            return Getter.GetValue(instance);
        }

        /// <summary>
        /// Returns a representation of the accessor including access modes
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
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
