// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DataTypeConverter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Core.Types; 

/// <summary>
///     A singleton DataTypeConverter
/// </summary>
public class DataTypeConverter {
    private readonly Dictionary<Type, DataType> _cachedDataTypes;
    private readonly object _lock = new();

    /// <summary>
    ///     The static singleton instance
    /// </summary>
    public static DataTypeConverter Instance { get; } = new();

    /// <summary>
    ///     Get a dictionary of primitive data types
    /// </summary>
    public IReadOnlyDictionary<Type, DataType> Primitives { get; }

    private DataTypeConverter() {
        var primitives = CreatePrimitiveTypes();
        _cachedDataTypes = new Dictionary<Type, DataType>(primitives);
        Primitives = primitives;
    }

    /// <summary>
    ///     Get a data type of a new instance
    /// </summary>
    public DataType GetDataType(object instance) {
        return GetDataType(instance?.GetType());
    }

    /// <summary>
    ///     Get a data type of a type
    /// </summary>
    public DataType GetDataType(Type type) {
        if(type == null)
            return DataType.EmptyType;

        lock(_lock) {
            if(_cachedDataTypes.TryGetValue(type, out var dataType))
                return dataType;

            dataType = CreateDataType(type);
            _cachedDataTypes[type] = dataType;
            return dataType;
        }
    }

    private DataType CreateDataType(Type type) {
        if(type.IsEnumerable(out var elementType))
            return new DataType(DataTypeFlags.Collection, type, elementType: elementType);

        var underlyingType = Nullable.GetUnderlyingType(type);
        if(underlyingType is not null) {
            var underlyingDataType = GetDataType(underlyingType);
            return new DataType(DataTypeFlags.Nullable | underlyingDataType.Flags, type, underlyingType);
        }

        DataTypeFlags flags;
        if(type.IsEnum) {
            flags = DataTypeFlags.Primitive | DataTypeFlags.Enum;
            underlyingType = Enum.GetUnderlyingType(type);
        } else
            flags = DataTypeFlags.Complex | DataTypeFlags.Object;

        return new DataType(flags, type, underlyingType);
    }

    private static Dictionary<Type, DataType> CreatePrimitiveTypes() {
        var primitives = new Dictionary<Type, DataType>();

        void AddPrimitiveType(Type type, DataTypeFlags flags) {
            primitives.TryAdd(type, new DataType(flags, type));
        }

        AddPrimitiveType(typeof(Guid), DataTypeFlags.Guid | DataTypeFlags.Primitive);
        AddPrimitiveType(typeof(bool), DataTypeFlags.Bool | DataTypeFlags.Primitive);
        AddPrimitiveType(typeof(char), DataTypeFlags.Char | DataTypeFlags.Primitive);

        AddPrimitiveType(typeof(sbyte), DataTypeFlags.SByte | DataTypeFlags.Primitive);
        AddPrimitiveType(typeof(byte), DataTypeFlags.Byte | DataTypeFlags.Primitive);
        AddPrimitiveType(typeof(short), DataTypeFlags.Short | DataTypeFlags.Primitive);
        AddPrimitiveType(typeof(ushort), DataTypeFlags.UShort | DataTypeFlags.Primitive);
        AddPrimitiveType(typeof(int), DataTypeFlags.Int | DataTypeFlags.Primitive);
        AddPrimitiveType(typeof(uint), DataTypeFlags.UInt | DataTypeFlags.Primitive);
        AddPrimitiveType(typeof(long), DataTypeFlags.Long | DataTypeFlags.Primitive);
        AddPrimitiveType(typeof(ulong), DataTypeFlags.ULong | DataTypeFlags.Primitive);
        AddPrimitiveType(typeof(float), DataTypeFlags.Float | DataTypeFlags.Primitive);
        AddPrimitiveType(typeof(double), DataTypeFlags.Double | DataTypeFlags.Primitive);
        AddPrimitiveType(typeof(decimal), DataTypeFlags.Decimal | DataTypeFlags.Primitive);

        AddPrimitiveType(typeof(DateTime), DataTypeFlags.DateTime | DataTypeFlags.Primitive);
        AddPrimitiveType(typeof(DateTimeOffset), DataTypeFlags.DateTimeOffset | DataTypeFlags.Primitive);
        AddPrimitiveType(typeof(TimeSpan), DataTypeFlags.TimeSpan | DataTypeFlags.Primitive);

        AddPrimitiveType(typeof(string), DataTypeFlags.String | DataTypeFlags.Primitive);
        AddPrimitiveType(typeof(object), DataTypeFlags.Object | DataTypeFlags.Complex);

        return primitives;
    }
}