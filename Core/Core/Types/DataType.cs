// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DataType.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  03.07.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Core.Types; 

/// <summary>
///     A more advanced type information object
/// </summary>
public class DataType {
    /// <summary>
    ///     The static empty type
    /// </summary>
    public static DataType EmptyType { get; } = new(DataTypeFlags.None, null);

    /// <summary>
    ///     Flags of the data type
    /// </summary>
    public DataTypeFlags Flags { get; }

    /// <summary>
    ///     The type
    /// </summary>
    public Type Type { get; }

    /// <summary>
    ///     The underlying type (enums)
    /// </summary>
    public Type UnderlyingType { get; }

    /// <summary>
    ///     The element type (collections)
    /// </summary>
    public Type ElementType { get; }

    /// <summary>
    ///     The underlying data type (enums)
    /// </summary>
    public DataType UnderlyingDataType => UnderlyingType?.ToDataType();

    /// <summary>
    ///     The element data type (collections)
    /// </summary>
    public DataType ElementDataType => ElementType?.ToDataType();

    /// <summary>
    ///     Create a new instance of <see cref="DataType" />
    /// </summary>
    public DataType(DataTypeFlags flags, Type type, Type underlyingType = null, Type elementType = null) {
        Flags = flags;
        Type = type;
        UnderlyingType = underlyingType;
        ElementType = elementType;
    }

    /// <summary>
    ///     Check if the type equals another
    /// </summary>
    protected bool Equals(DataType other) {
        return Type == other.Type;
    }

    /// <inheritdoc />
    public override bool Equals(object obj) {
        if(ReferenceEquals(null, obj))
            return false;
        if(ReferenceEquals(this, obj))
            return true;
        if(obj.GetType() != GetType())
            return false;
        return Equals((DataType)obj);
    }

    /// <inheritdoc />
    public override int GetHashCode() {
        return Type is not null ? Type.GetHashCode() : 0;
    }
}