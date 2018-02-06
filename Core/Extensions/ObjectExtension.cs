// ==================================================
// Copyright 2016(C) , DotLogix
// File:  ObjectExtension.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.09.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.ComponentModel;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Extensions {
    public static class ObjectExtension {
        public static T ExchangeIfDefault<T>(this T source, T exchangeValue = default(T)) {
            return Equals(source, default(T)) ? exchangeValue : source;
        }

        public static T Cast<T>(this object obj) {
            return (T)obj;
        }

        public static T SafeCast<T>(this object obj) where T : class {
            return obj as T;
        }

        public static T[] ToSingleElementArray<T>(this T value) {
            return new[] {value};
        }

        public static IEnumerable<T> ToSingleElementEnumerable<T>(this T value) {
            yield return value;
        }

        #region Convert

        public static object ConvertTo(this object value, Type targetType)
        {
            if (TryConvertTo(value, targetType, out var convertedValue) == false)
                throw new
                NotSupportedException($"Conversion between {value.GetType()} and {targetType} is not supported");
            return convertedValue;
        }

        public static bool TryConvertTo(this object value, Type targetType, out object target)
        {
            if (value == null)
            {
                if (targetType.IsValueType)
                    throw new ArgumentNullException(nameof(value), "Can not convert null to value type");
                target = default(object);
                return true;
            }

            var sourceType = value.GetType();
            if (targetType.IsAssignableFrom(sourceType))
            {
                target = value;
                return true;
            }

            if(value is string str && TryParseTo(str, targetType, out target))
                return true;

            var sourceTypeConverter = TypeDescriptor.GetConverter(sourceType);

            if (sourceTypeConverter.CanConvertTo(targetType))
            {
                target = sourceTypeConverter.ConvertTo(value, targetType);
                return true;
            }

            var targetTypeConverter = TypeDescriptor.GetConverter(targetType);
            if (targetTypeConverter.CanConvertFrom(sourceType))
            {
                target = targetTypeConverter.ConvertFrom(value);
                return true;
            }
            target = default(object);
            return false;
        }

        public static TTarget ConvertTo<TTarget>(this object value)
        {
            var targetType = typeof(TTarget);
            return (TTarget)ConvertTo(value, targetType);
        }

        public static bool TryConvertTo<TTarget>(this object value, out TTarget target)
        {
            var targetType = typeof(TTarget);
            if (TryConvertTo(value, targetType, out var convertedValue))
            {
                target = (TTarget)convertedValue;
                return true;
            }
            target = default(TTarget);
            return false;
        }

        #endregion

        #region Parse

        public static object ParseTo(this string value, Type targetType)
        {
            if (TryParseTo(value, targetType, out var convertedValue) == false)
                throw new
                NotSupportedException($"Conversion between {value.GetType()} and {targetType} is not supported");
            return convertedValue;
        }

        public static bool TryParseTo(this string value, Type targetType, out object target) {
            target = null;
            var dataType = targetType.ToDataType();
            if((dataType.Flags & DataTypeFlags.Primitive) == 0)
                return false;

            bool result;
            switch(dataType.Flags & DataTypeFlags.PrimitiveMask) {
                case DataTypeFlags.Guid:
                    result = Guid.TryParse(value, out var guid);
                    target = guid;
                    break;
                case DataTypeFlags.Bool:
                    result = bool.TryParse(value, out var bo);
                    target = bo;
                    break;
                case DataTypeFlags.Char:
                    result = char.TryParse(value, out var c);
                    target = c;
                    break;
                case DataTypeFlags.Enum:
                    target = Enum.Parse(targetType, value);
                    result = true;
                    break;
                case DataTypeFlags.SByte:
                    result = sbyte.TryParse(value, out var sb);
                    target = sb;
                    break;
                case DataTypeFlags.Byte:
                    result = byte.TryParse(value, out var b);
                    target = b;
                    break;
                case DataTypeFlags.Short:
                    result = short.TryParse(value, out var s);
                    target = s;
                    break;
                case DataTypeFlags.UShort:
                    result = ushort.TryParse(value, out var us);
                    target = us;
                    break;
                case DataTypeFlags.Int:
                    result = int.TryParse(value, out var i);
                    target = i;
                    break;
                case DataTypeFlags.UInt:
                    result = uint.TryParse(value, out var ui);
                    target = ui;
                    break;
                case DataTypeFlags.Long:
                    result = long.TryParse(value, out var l);
                    target = l;
                    break;
                case DataTypeFlags.ULong:
                    result = ulong.TryParse(value, out var ul);
                    target = ul;
                    break;
                case DataTypeFlags.Float:
                    result = float.TryParse(value, out var f);
                    target = f;
                    break;
                case DataTypeFlags.Double:
                    result = double.TryParse(value, out var d);
                    target = d;
                    break;
                case DataTypeFlags.Decimal:
                    result = decimal.TryParse(value, out var dec);
                    target = dec;
                    break;
                case DataTypeFlags.DateTime:
                    result = DateTime.TryParse(value, out var dt);
                    target = dt;
                    break;
                case DataTypeFlags.DateTimeOffset:
                    result = DateTimeOffset.TryParse(value, out var dto);
                    target = dto;
                    break;
                case DataTypeFlags.TimeSpan:
                    result = TimeSpan.TryParse(value, out var ts);
                    target = ts;
                    break;
                case DataTypeFlags.String:
                    target = value;
                    result = true;
                    break;
                default:
                    return false;
            }
            return result;
        }

        public static TTarget ParseTo<TTarget>(this string value)
        {
            var targetType = typeof(TTarget);
            return (TTarget)ParseTo(value, targetType);
        }

        public static bool TryParseTo<TTarget>(this string value, out TTarget target)
        {
            var targetType = typeof(TTarget);
            if (TryParseTo(value, targetType, out var convertedValue))
            {
                target = (TTarget)convertedValue;
                return true;
            }
            target = default(TTarget);
            return false;
        }

        #endregion


        public static DataType GetDataType(this object instance)
        {
            return DataTypeConverter.Instance.GetDataType(instance);
        }
    }
}
