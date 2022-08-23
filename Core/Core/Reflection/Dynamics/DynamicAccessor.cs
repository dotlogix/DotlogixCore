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

namespace DotLogix.Core.Reflection.Dynamics; 

/// <summary>
/// A representation of a value accessor
/// </summary>
public abstract class DynamicAccessor : DynamicMember {
    /// <summary>
    /// The value type
    /// </summary>
    public Type ValueType { get; }

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
    protected DynamicAccessor(MemberInfo memberInfo, DynamicSetter setter, DynamicGetter getter, Type valueType, AccessorTypes accessorType) : base(memberInfo) {
        ValueType = valueType ?? throw new ArgumentNullException(nameof(valueType));
        AccessorType = accessorType;
        var accessMode = ValueAccessModes.None;
        if(getter is not null) {
            Getter = getter;
            accessMode |= ValueAccessModes.Read;
        }
        if(setter is not null) {
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
        return ValueAccessMode switch {
            ValueAccessModes.None => $"{DeclaringType.Name}.{Name}",
            ValueAccessModes.Read => $"{DeclaringType.Name}.{Name}{{get;}}",
            ValueAccessModes.Write => $"{DeclaringType.Name}.{Name}{{set;}}",
            ValueAccessModes.ReadWrite => $"{DeclaringType.Name}.{Name}{{get; set;}}",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}