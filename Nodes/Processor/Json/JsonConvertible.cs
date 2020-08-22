using System;
using System.Globalization;
using DotLogix.Core.Nodes.Schema;
using DotLogix.Core.Types;

namespace DotLogix.Core.Nodes.Processor {
    public class JsonConvertible
    {
        public string Value { get; set; }

        public virtual object ToObject(DataType targetType, IReadOnlyConverterSettings settings) {
            var flags = targetType.Flags & DataTypeFlags.PrimitiveMask;
            if (flags == 0)
                throw new ArgumentException("Value has to be a primitive value");

            return flags switch
                   {
                   DataTypeFlags.Enum => ToEnum(targetType.Type),
                   DataTypeFlags.SByte => ToSByte(settings),
                   DataTypeFlags.Byte => ToByte(settings),
                   DataTypeFlags.Short => ToInt16(settings),
                   DataTypeFlags.UShort => ToUInt16(settings),
                   DataTypeFlags.Int => ToInt32(settings),
                   DataTypeFlags.UInt => ToUInt32(settings),
                   DataTypeFlags.Long => ToInt64(settings),
                   DataTypeFlags.ULong => ToUInt64(settings),
                   DataTypeFlags.Float => ToSingle(settings),
                   DataTypeFlags.Double => ToDouble(settings),
                   DataTypeFlags.Decimal => ToDecimal(settings),
                   DataTypeFlags.DateTime => ToDateTime(settings),
                   DataTypeFlags.DateTimeOffset => ToDateTimeOffset(settings),
                   DataTypeFlags.TimeSpan => ToTimeSpan(settings),
                   DataTypeFlags.String => ToString(settings),
                   DataTypeFlags.Char => ToChar(settings),
                   _ => throw new NotSupportedException()
                   };
        }

        protected virtual object ToTimeSpan(IReadOnlyConverterSettings settings)
        {
            if (TimeSpan.TryParse(Value, CultureInfo.InvariantCulture, out var value))
                return value;
            throw new FormatException(
                                      $"Invalid format while parsing decimal \"{Value}\" with format \"{settings.NumberFormat}\"");
        }

        protected virtual object ToDateTimeOffset(IReadOnlyConverterSettings settings)
        {
            var styles = settings.TimeFormat == "u"
                         ? DateTimeStyles.AssumeUniversal
                         : DateTimeStyles.AssumeLocal;

            if (DateTimeOffset.TryParse(Value, CultureInfo.InvariantCulture, styles, out var value))
                return value;
            throw new FormatException(
                                      $"Invalid format while parsing decimal \"{Value}\" with format \"{settings.NumberFormat}\"");
        }

        protected virtual object ToDateTime(IReadOnlyConverterSettings settings)
        {
            var styles = settings.TimeFormat == "u"
                         ? DateTimeStyles.AssumeUniversal
                         : DateTimeStyles.AssumeLocal;

            if (DateTime.TryParse(Value, CultureInfo.InvariantCulture, styles, out var value))
                return value;
            throw new FormatException(
                                      $"Invalid format while parsing decimal \"{Value}\" with format \"{settings.NumberFormat}\"");
        }

        protected virtual object ToDecimal(IReadOnlyConverterSettings settings)
        {
            var currentValue = Value;
            var numberStyles = JsonNumber.ToNumberStyles(settings.NumberFormat, ref currentValue);
            if (decimal.TryParse(currentValue, numberStyles, CultureInfo.InvariantCulture, out var value))
                return value;
            throw new FormatException(
                                      $"Invalid format while parsing decimal \"{currentValue}\" with format \"{settings.NumberFormat}\"");
        }

        protected virtual double ToDouble(IReadOnlyConverterSettings settings)
        {
            var currentValue = Value;
            if (JsonNumber.IsNativeFloatFormat(settings.NumberFormat))
                return JsonStrings.ParseDouble(currentValue);

            var numberStyles = JsonNumber.ToNumberStyles(settings.NumberFormat, ref currentValue);
            if (double.TryParse(currentValue, numberStyles, CultureInfo.InvariantCulture, out var value))
                return value;
            throw new FormatException(
                                      $"Invalid format while parsing double \"{currentValue}\" with format \"{settings.NumberFormat}\"");
        }

        protected virtual float ToSingle(IReadOnlyConverterSettings settings)
        {
            var currentValue = Value;
            if (JsonNumber.IsNativeFloatFormat(settings.NumberFormat))
                return (float) JsonStrings.ParseDouble(currentValue);

            var numberStyles = JsonNumber.ToNumberStyles(settings.NumberFormat, ref currentValue);
            if (float.TryParse(currentValue, numberStyles, CultureInfo.InvariantCulture, out var value))
                return value;
            throw new FormatException(
                                      $"Invalid format while parsing float \"{currentValue}\" with format \"{settings.NumberFormat}\"");
        }

        protected virtual ulong ToUInt64(IReadOnlyConverterSettings settings)
        {
            var currentValue = Value;
            if (JsonNumber.IsNativeIntFormat(settings.NumberFormat))
                return JsonStrings.ParseUInt64(currentValue);

            var numberStyles = JsonNumber.ToNumberStyles(settings.NumberFormat, ref currentValue);
            if (ulong.TryParse(currentValue, numberStyles, CultureInfo.InvariantCulture, out var value))
                return value;
            throw new FormatException(
                                      $"Invalid format while parsing ulong \"{currentValue}\" with format \"{settings.NumberFormat}\"");
        }

        protected virtual long ToInt64(IReadOnlyConverterSettings settings)
        {
            var currentValue = Value;
            if (JsonNumber.IsNativeIntFormat(settings.NumberFormat))
                return JsonStrings.ParseInt64(currentValue);

            var numberStyles = JsonNumber.ToNumberStyles(settings.NumberFormat, ref currentValue);
            if (long.TryParse(currentValue, numberStyles, CultureInfo.InvariantCulture, out var value))
                return value;
            throw new FormatException(
                                      $"Invalid format while parsing long \"{currentValue}\" with format \"{settings.NumberFormat}\"");
        }

        protected virtual uint ToUInt32(IReadOnlyConverterSettings settings)
        {
            var currentValue = Value;
            if (JsonNumber.IsNativeIntFormat(settings.NumberFormat))
                return (uint) JsonStrings.ParseUInt64(currentValue);

            var numberStyles = JsonNumber.ToNumberStyles(settings.NumberFormat, ref currentValue);
            if (uint.TryParse(currentValue, numberStyles, CultureInfo.InvariantCulture, out var value))
                return value;
            throw new FormatException(
                                      $"Invalid format while parsing uint \"{currentValue}\" with format \"{settings.NumberFormat}\"");
        }

        protected virtual int ToInt32(IReadOnlyConverterSettings settings)
        {
            var currentValue = Value;
            if (JsonNumber.IsNativeIntFormat(settings.NumberFormat))
                return (int) JsonStrings.ParseInt64(currentValue);

            var numberStyles = JsonNumber.ToNumberStyles(settings.NumberFormat, ref currentValue);
            if (int.TryParse(currentValue, numberStyles, CultureInfo.InvariantCulture, out var value))
                return value;
            throw new FormatException(
                                      $"Invalid format while parsing int \"{currentValue}\" with format \"{settings.NumberFormat}\"");
        }

        protected virtual ushort ToUInt16(IReadOnlyConverterSettings settings)
        {
            var currentValue = Value;
            if (JsonNumber.IsNativeIntFormat(settings.NumberFormat))
                return (ushort) JsonStrings.ParseUInt64(currentValue);

            var numberStyles = JsonNumber.ToNumberStyles(settings.NumberFormat, ref currentValue);
            if (ushort.TryParse(currentValue, numberStyles, CultureInfo.InvariantCulture, out var value))
                return value;
            throw new FormatException(
                                      $"Invalid format while parsing ushort \"{currentValue}\" with format \"{settings.NumberFormat}\"");
        }

        protected virtual short ToInt16(IReadOnlyConverterSettings settings)
        {
            var currentValue = Value;
            if (JsonNumber.IsNativeIntFormat(settings.NumberFormat))
                return (short) JsonStrings.ParseInt64(currentValue);

            var numberStyles = JsonNumber.ToNumberStyles(settings.NumberFormat, ref currentValue);
            if (short.TryParse(currentValue, numberStyles, CultureInfo.InvariantCulture, out var value))
                return value;
            throw new FormatException(
                                      $"Invalid format while parsing short \"{currentValue}\" with format \"{settings.NumberFormat}\"");
        }

        protected virtual byte ToByte(IReadOnlyConverterSettings settings)
        {
            var currentValue = Value;
            if (JsonNumber.IsNativeIntFormat(settings.NumberFormat))
                return (byte) JsonStrings.ParseUInt64(currentValue);

            var numberStyles = JsonNumber.ToNumberStyles(settings.NumberFormat, ref currentValue);
            if (byte.TryParse(currentValue, numberStyles, CultureInfo.InvariantCulture, out var value))
                return value;
            throw new FormatException(
                                      $"Invalid format while parsing byte \"{currentValue}\" with format \"{settings.NumberFormat}\"");
        }

        protected virtual sbyte ToSByte(IReadOnlyConverterSettings settings)
        {
            var currentValue = Value;
            if (JsonNumber.IsNativeIntFormat(settings.NumberFormat))
                return (sbyte) JsonStrings.ParseInt64(currentValue);

            var numberStyles = JsonNumber.ToNumberStyles(settings.NumberFormat, ref currentValue);
            if (sbyte.TryParse(currentValue, numberStyles, CultureInfo.InvariantCulture, out var value))
                return value;

            throw new FormatException(
                                      $"Invalid format while parsing sbyte \"{currentValue}\" with format \"{settings.NumberFormat}\"");
        }

        protected virtual object ToEnum(Type targetType)
        {
            try
            {
                return Enum.Parse(targetType, Value);
            }
            catch (Exception e)
            {
                throw new FormatException($"Invalid format while parsing enum \"{Value}\" to target type {targetType.Name}", e);
            }
        }

        protected virtual string ToString(IReadOnlyConverterSettings settings)
        {
            return Value;
        }

        protected virtual char ToChar(IReadOnlyConverterSettings settings)
        {
            if(Value.Length != 1)
                throw new FormatException($"Invalid format while parsing char \"{Value}\". Expected a single character but was {Value.Length}");
            return Value[0];
        }
    }
}