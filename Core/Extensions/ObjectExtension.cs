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

            if(value is string str && str.TryParseTo(targetType, out target))
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

        public static DataType GetDataType(this object instance)
        {
            return DataTypeConverter.Instance.GetDataType(instance);
        }
    }
}
