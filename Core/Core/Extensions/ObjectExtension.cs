// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ObjectExtension.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  03.07.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.ComponentModel;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Extensions; 

/// <summary>
///     A static class providing extension methods for <see cref="object" />
/// </summary>
public static class ObjectExtension {
    /// <summary>
    ///     Replaces null values with the provided value
    /// </summary>
    /// <param name="source">The object</param>
    /// <param name="convert">The callback to convert if can not be casted</param>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <returns></returns>
    public static T2 CastOrConvert<T1, T2>(this T1 source, Func<T1, T2> convert) {
        return source is T2 o ? o : convert.Invoke(source);
    }

    /// <summary>
    ///     Replaces null values with the provided value
    /// </summary>
    /// <param name="source">The object</param>
    /// <param name="exchangeValue">The default value</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ExchangeIfDefault<T>(this T source, T exchangeValue = default) {
        return Equals(source, default(T)) ? exchangeValue : source;
    }

    /// <summary>
    ///     Get the <see cref="DataType" /> of an object instance
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    public static DataType GetDataType(this object instance) {
        return DataTypeConverter.Instance.GetDataType(instance);
    }

    #region Convert
    /// <summary>
    ///     Converts a value to another type using type converters
    /// </summary>
    /// <param name="value">The value</param>
    /// <param name="targetType">The target type</param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public static object ConvertTo(this object value, Type targetType) {
        if(TryConvertTo(value, targetType, out var convertedValue) == false) {
            throw new
                NotSupportedException($"Conversion between {value.GetType()} and {targetType} is not supported");
        }

        return convertedValue;
    }

    /// <summary>
    ///     Tries to convert a value to another type using type converters
    /// </summary>
    /// <param name="value">The value</param>
    /// <param name="targetType">The target type</param>
    /// <param name="target">The target value</param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public static bool TryConvertTo(this object value, Type targetType, out object target) {
        if(value == null) {
            target = targetType.GetDefaultValue();
            return true;
        }

        var sourceType = value.GetType();
        if(targetType.IsAssignableFrom(sourceType)) {
            target = value;
            return true;
        }

        if(value is string str && str.TryParseTo(targetType, out target)) {
            return true;
        }

        if(targetType.IsEnum) {
            try {
                target = Enum.ToObject(targetType, value);
                return true;
            } catch {
                // ignored
            }
        }

        try {
            var sourceTypeConverter = TypeDescriptor.GetConverter(sourceType);

            if(sourceTypeConverter.CanConvertTo(targetType)) {
                target = sourceTypeConverter.ConvertTo(value, targetType);
                return true;
            }

            var targetTypeConverter = TypeDescriptor.GetConverter(targetType);
            if(targetTypeConverter.CanConvertFrom(sourceType)) {
                target = targetTypeConverter.ConvertFrom(value);
                return true;
            }
        } catch {
            // ignored
        }

        target = default;
        return false;
    }

    /// <summary>
    ///     Converts a value to another type using type converters
    /// </summary>
    /// <param name="value">The value</param>
    /// <typeparam name="TTarget">The target type</typeparam>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public static TTarget ConvertTo<TTarget>(this object value) {
        if(value is TTarget t) {
            return t;
        }

        var targetType = typeof(TTarget);
        return (TTarget)ConvertTo(value, targetType);
    }

    /// <summary>
    ///     Tries to convert a value to another type using type converters
    /// </summary>
    /// <param name="value">The value</param>
    /// <typeparam name="TTarget">The target type</typeparam>
    /// <param name="target">The target value</param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public static bool TryConvertTo<TTarget>(this object value, out TTarget target) {
        if(value is TTarget t) {
            target = t;
            return true;
        }

        var targetType = typeof(TTarget);
        if(TryConvertTo(value, targetType, out var convertedValue)) {
            target = (TTarget)convertedValue;
            return true;
        }

        target = default;
        return false;
    }
    #endregion
}