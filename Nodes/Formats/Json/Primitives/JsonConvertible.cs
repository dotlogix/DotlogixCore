#region
using System;
using System.Globalization;
using DotLogix.Core.Nodes.Schema;
using DotLogix.Core.Nodes.Utils;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes.Formats.Json {
    public abstract class JsonConvertible : IJsonPrimitive {
        public JsonPrimitiveType Type { get; }
        public string Value { get; }

        protected JsonConvertible(JsonPrimitiveType type, string value) {
            Value = value ?? throw new ArgumentNullException(nameof(value));
            Type = type;
        }


        public virtual void AppendJson(CharBuffer buffer) {
            buffer.Append(Value);
        }

        public virtual string ToJson() {
            return Value;
        }

        public virtual object ToObject(DataType targetType, IReadOnlyConverterSettings settings) {
            var flags = targetType.Flags & DataTypeFlags.PrimitiveMask;
            if(flags == 0)
                throw new ArgumentException("Value has to be a primitive value");

            return flags switch {
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
                DataTypeFlags.Guid => ToGuid(settings),
                DataTypeFlags.Bool => ToBool(settings),
                _ => throw new NotSupportedException()
            };
        }

        protected virtual object ToBool(IReadOnlyConverterSettings settings) {
            if(bool.TryParse(Value, out var value))
                return value;
            throw new FormatException($"Invalid format while parsing bool \"{Value}\"");
        }

        protected virtual object ToGuid(IReadOnlyConverterSettings settings) {
            if(Guid.TryParseExact(Value, settings.GuidFormat, out var value))
                return value;
            throw new FormatException($"Invalid format while parsing guid \"{Value}\" with format \"{settings.GuidFormat}\"");
        }

        protected virtual object ToTimeSpan(IReadOnlyConverterSettings settings) {
            if(TimeSpan.TryParse(Value, settings.FormatProvider, out var value))
                return value;
            throw new FormatException($"Invalid format while parsing decimal \"{Value}\" with format \"{settings.NumberFormat}\"");
        }

        protected virtual object ToDateTimeOffset(IReadOnlyConverterSettings settings) {
            var styles = settings.TimeFormat == "u"
                             ? DateTimeStyles.AssumeUniversal
                             : DateTimeStyles.AssumeLocal;

            if(DateTimeOffset.TryParse(Value, settings.FormatProvider, styles, out var value))
                return value;
            throw new FormatException(
                                      $"Invalid format while parsing decimal \"{Value}\" with format \"{settings.NumberFormat}\""
                                     );
        }

        protected virtual object ToDateTime(IReadOnlyConverterSettings settings) {
            var styles = settings.TimeFormat == "u"
                             ? DateTimeStyles.AssumeUniversal
                             : DateTimeStyles.AssumeLocal;
            if(DateTime.TryParse(Value, settings.FormatProvider, styles, out var value))
                return value;
            throw new FormatException(
                                      $"Invalid format while parsing decimal \"{Value}\" with format \"{settings.NumberFormat}\""
                                     );
        }

        protected virtual object ToDecimal(IReadOnlyConverterSettings settings) {
            var currentValue = Value;
            var numberStyles = JsonNumber.ToNumberStyles(settings.NumberFormat, ref currentValue);
            if(decimal.TryParse(currentValue, numberStyles, settings.FormatProvider, out var value))
                return value;
            throw new FormatException(
                                      $"Invalid format while parsing decimal \"{currentValue}\" with format \"{settings.NumberFormat}\""
                                     );
        }

        protected virtual double ToDouble(IReadOnlyConverterSettings settings) {
            var currentValue = Value;
            if(JsonNumber.IsNativeFloatFormat(settings.NumberFormat, settings.FormatProvider))
                return JsonStrings.ParseDouble(currentValue);

            var numberStyles = JsonNumber.ToNumberStyles(settings.NumberFormat, ref currentValue);
            if(double.TryParse(currentValue, numberStyles, settings.FormatProvider, out var value))
                return value;
            throw new FormatException(
                                      $"Invalid format while parsing double \"{currentValue}\" with format \"{settings.NumberFormat}\""
                                     );
        }

        protected virtual float ToSingle(IReadOnlyConverterSettings settings) {
            var currentValue = Value;
            if(JsonNumber.IsNativeFloatFormat(settings.NumberFormat, settings.FormatProvider))
                return (float)JsonStrings.ParseDouble(currentValue);

            var numberStyles = JsonNumber.ToNumberStyles(settings.NumberFormat, ref currentValue);
            if(float.TryParse(currentValue, numberStyles, settings.FormatProvider, out var value))
                return value;
            throw new FormatException(
                                      $"Invalid format while parsing float \"{currentValue}\" with format \"{settings.NumberFormat}\""
                                     );
        }

        protected virtual ulong ToUInt64(IReadOnlyConverterSettings settings) {
            var currentValue = Value;
            if(JsonNumber.IsNativeIntFormat(settings.NumberFormat, settings.FormatProvider))
                return JsonStrings.ParseUInt64(currentValue);

            var numberStyles = JsonNumber.ToNumberStyles(settings.NumberFormat, ref currentValue);
            if(ulong.TryParse(currentValue, numberStyles, settings.FormatProvider, out var value))
                return value;
            throw new FormatException(
                                      $"Invalid format while parsing ulong \"{currentValue}\" with format \"{settings.NumberFormat}\""
                                     );
        }

        protected virtual long ToInt64(IReadOnlyConverterSettings settings) {
            var currentValue = Value;
            if(JsonNumber.IsNativeIntFormat(settings.NumberFormat, settings.FormatProvider))
                return JsonStrings.ParseInt64(currentValue);

            var numberStyles = JsonNumber.ToNumberStyles(settings.NumberFormat, ref currentValue);
            if(long.TryParse(currentValue, numberStyles, settings.FormatProvider, out var value))
                return value;
            throw new FormatException(
                                      $"Invalid format while parsing long \"{currentValue}\" with format \"{settings.NumberFormat}\""
                                     );
        }

        protected virtual uint ToUInt32(IReadOnlyConverterSettings settings) {
            var currentValue = Value;
            if(JsonNumber.IsNativeIntFormat(settings.NumberFormat, settings.FormatProvider))
                return (uint)JsonStrings.ParseUInt64(currentValue);

            var numberStyles = JsonNumber.ToNumberStyles(settings.NumberFormat, ref currentValue);
            if(uint.TryParse(currentValue, numberStyles, settings.FormatProvider, out var value))
                return value;
            throw new FormatException(
                                      $"Invalid format while parsing uint \"{currentValue}\" with format \"{settings.NumberFormat}\""
                                     );
        }

        protected virtual int ToInt32(IReadOnlyConverterSettings settings) {
            var currentValue = Value;
            if(JsonNumber.IsNativeIntFormat(settings.NumberFormat, settings.FormatProvider))
                return (int)JsonStrings.ParseInt64(currentValue);

            var numberStyles = JsonNumber.ToNumberStyles(settings.NumberFormat, ref currentValue);
            if(int.TryParse(currentValue, numberStyles, settings.FormatProvider, out var value))
                return value;
            throw new FormatException(
                                      $"Invalid format while parsing int \"{currentValue}\" with format \"{settings.NumberFormat}\""
                                     );
        }

        protected virtual ushort ToUInt16(IReadOnlyConverterSettings settings) {
            var currentValue = Value;
            if(JsonNumber.IsNativeIntFormat(settings.NumberFormat, settings.FormatProvider))
                return (ushort)JsonStrings.ParseUInt64(currentValue);

            var numberStyles = JsonNumber.ToNumberStyles(settings.NumberFormat, ref currentValue);
            if(ushort.TryParse(currentValue, numberStyles, settings.FormatProvider, out var value))
                return value;
            throw new FormatException(
                                      $"Invalid format while parsing ushort \"{currentValue}\" with format \"{settings.NumberFormat}\""
                                     );
        }

        protected virtual short ToInt16(IReadOnlyConverterSettings settings) {
            var currentValue = Value;
            if(JsonNumber.IsNativeIntFormat(settings.NumberFormat, settings.FormatProvider))
                return (short)JsonStrings.ParseInt64(currentValue);

            var numberStyles = JsonNumber.ToNumberStyles(settings.NumberFormat, ref currentValue);
            if(short.TryParse(currentValue, numberStyles, settings.FormatProvider, out var value))
                return value;
            throw new FormatException(
                                      $"Invalid format while parsing short \"{currentValue}\" with format \"{settings.NumberFormat}\""
                                     );
        }

        protected virtual byte ToByte(IReadOnlyConverterSettings settings) {
            var currentValue = Value;
            if(JsonNumber.IsNativeIntFormat(settings.NumberFormat, settings.FormatProvider))
                return (byte)JsonStrings.ParseUInt64(currentValue);

            var numberStyles = JsonNumber.ToNumberStyles(settings.NumberFormat, ref currentValue);
            if(byte.TryParse(currentValue, numberStyles, settings.FormatProvider, out var value))
                return value;
            throw new FormatException(
                                      $"Invalid format while parsing byte \"{currentValue}\" with format \"{settings.NumberFormat}\""
                                     );
        }

        protected virtual sbyte ToSByte(IReadOnlyConverterSettings settings) {
            var currentValue = Value;
            if(JsonNumber.IsNativeIntFormat(settings.NumberFormat, settings.FormatProvider))
                return (sbyte)JsonStrings.ParseInt64(currentValue);

            var numberStyles = JsonNumber.ToNumberStyles(settings.NumberFormat, ref currentValue);
            if(sbyte.TryParse(currentValue, numberStyles, settings.FormatProvider, out var value))
                return value;

            throw new FormatException(
                                      $"Invalid format while parsing sbyte \"{currentValue}\" with format \"{settings.NumberFormat}\""
                                     );
        }

        protected virtual object ToEnum(Type targetType) {
            try {
                return Enum.Parse(targetType, Value);
            } catch(Exception e) {
                throw new FormatException($"Invalid format while parsing enum \"{Value}\" to target type {targetType.Name}", e);
            }
        }

        protected virtual string ToString(IReadOnlyConverterSettings settings) {
            return Value;
        }

        protected virtual char ToChar(IReadOnlyConverterSettings settings) {
            if(Value.Length != 1)
                throw new FormatException($"Invalid format while parsing char \"{Value}\". Expected a single character but was {Value.Length}");
            return Value[0];
        }
    }
}
