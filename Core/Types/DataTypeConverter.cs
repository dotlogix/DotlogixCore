// ==================================================
// Copyright 2016(C) , DotLogix
// File:  DataTypeConverter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  09.09.2017
// LastEdited:  09.09.2017
// ==================================================

#region
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Core.Types {
    public class DataTypeConverter {
        private readonly ConcurrentDictionary<Type, DataType> _cachedDataTypes;

        public static DataTypeConverter Instance { get; } = new DataTypeConverter();

        private DataTypeConverter() {
            _cachedDataTypes = CreatePrimitiveTypes();
        }

        public DataType GetDataType(object instance) {
            return GetDataType(instance?.GetType());
        }

        public DataType GetDataType(Type type) {
            return type == null ? DataType.EmptyType : _cachedDataTypes.GetOrAdd(type, CreateDataType);
        }

        private DataType CreateDataType(Type type) {
            DataType dataType;
            if(type.IsEnumerable(out var elementType)) {
                dataType = new DataType(DataTypeFlags.Collection, type, elementType: elementType);
            } else {
                var underlyingType = Nullable.GetUnderlyingType(type);
                if(underlyingType == null) {
                    var flags = type.IsEnum
                                    ? DataTypeFlags.Primitive | DataTypeFlags.Enum
                                    : DataTypeFlags.Complex | DataTypeFlags.Object;
                    dataType = new DataType(flags, type);
                } else {
                    var underlayingDataType = GetDataType(underlyingType);
                    dataType = new DataType(DataTypeFlags.Nullable | underlayingDataType.Flags, type, underlyingType);
                }
            }
            return dataType;
        }

        private static ConcurrentDictionary<Type, DataType> CreatePrimitiveTypes() {
            var primitives = new ConcurrentDictionary<Type, DataType>();
            void AddPrimitiveType(Type type, DataTypeFlags flags)
            {
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
}
