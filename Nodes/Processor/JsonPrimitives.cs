using System;
using System.Globalization;
using System.Text;
using DotLogix.Core.Extensions;
using DotLogix.Core.Types;

namespace DotLogix.Core.Nodes.Processor
{
    public interface IJsonPrimitive
    {
        JsonPrimitiveType Type { get; }
        object ToObject(DataType targetType, IReadOnlyConverterSettings settings);
        void AppendJson(StringBuilder stringBuilder);
        string ToJson();
    }

    public class JsonNumber : IJsonPrimitive
    {
        private const DataTypeFlags UnsignedInteger = DataTypeFlags.ULong | DataTypeFlags.UInt | DataTypeFlags.UShort | DataTypeFlags.Byte;

        private const DataTypeFlags SignedInteger = DataTypeFlags.Long | DataTypeFlags.Int | DataTypeFlags.Short | DataTypeFlags.SByte;

        private const DataTypeFlags Floating = DataTypeFlags.Float | DataTypeFlags.Double | DataTypeFlags.Decimal;
        public string Value { get; set; }

        public JsonNumber(string value = null)
        {
            Value = value;
        }



        public JsonPrimitiveType Type => JsonPrimitiveType.Number;

        public object ToObject(DataType targetType, IReadOnlyConverterSettings settings)
        {
            return ToObject(Value, targetType, settings);
        }


        public void AppendJson(StringBuilder stringBuilder)
        {
            stringBuilder.Append(Value);
        }

        public string ToJson()
        {
            return Value;
        }

        public static object ToObject(string jsonValue, DataType targetType, IReadOnlyConverterSettings settings)
        {
            var flags = targetType.Flags & DataTypeFlags.NumericMask;
            if ((flags & UnsignedInteger) != 0)
            {
                var value = JsonStrings.ParseULong(jsonValue);
                switch (flags) {
                    case DataTypeFlags.ULong:
                        return value;
                    case DataTypeFlags.UInt:
                        return (uint)value;
                    case DataTypeFlags.UShort:
                        return (ushort)value;
                    case DataTypeFlags.Byte:
                        return (byte)value;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if ((flags & SignedInteger) != 0)
            {
                var value = JsonStrings.ParseLong(jsonValue);
                switch (flags)
                {
                    case DataTypeFlags.Long:
                        return value;
                    case DataTypeFlags.Int:
                        return (int)value;
                    case DataTypeFlags.Short:
                        return (short)value; 
                    case DataTypeFlags.SByte:
                        return (sbyte)value;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if ((flags & Floating) != 0)
            {
                var value = JsonStrings.ParseDouble(jsonValue);
                switch (flags) {
                    case DataTypeFlags.Decimal:
                        return decimal.Parse(jsonValue, ToNumberStyles(settings.NumberFormat, ref jsonValue), CultureInfo.InvariantCulture);
                    case DataTypeFlags.Double:
                        return value;
                    case DataTypeFlags.Float:
                        return (float)value;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if ((flags & DataTypeFlags.Enum) != 0)
                try
                {
                    return Enum.Parse(targetType.Type, jsonValue);
                }
                catch (Exception e)
                {
                    throw new FormatException(
                        $"Invalid format while parsing enum \"{jsonValue}\" to target type {targetType.Type.Name}", e);
                }

            throw new NotSupportedException();
        }

        private static object ToType(DataType targetType, IConvertible value)
        {
            return targetType.Type.IsInstanceOfType(value)
                ? value
                : value.ToType(targetType.Type, CultureInfo.InvariantCulture);
        }

        public static bool IsNativeNumberFormat(string format)
        {
            return IsNativeIntFormat(format) || IsNativeFloatFormat(format);
        }

        public static bool IsNativeIntFormat(string format)
        {
            switch (format[0]) {
                case 'D':
                case 'F':
                case 'G':
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsNativeFloatFormat(string format)
        {
            switch (format[0])
            {
                case 'D':
                case 'E':
                case 'F':
                case 'G':
                case 'R':
                    return true;
                default:
                    return false;
            }
        }

        public static NumberStyles ToNumberStyles(string currentFormat, ref string currentValue)
        {
            var numberStyles = NumberStyles.Any;
            if (currentFormat.Length > 1)
                switch (char.ToUpperInvariant(currentFormat[0]))
                {
                    case 'X':
                        numberStyles = NumberStyles.AllowHexSpecifier | NumberStyles.HexNumber;
                        break;
                    case 'P':
                        numberStyles = NumberStyles.AllowLeadingSign;
                        currentValue = currentValue.Substring(0, currentValue.Length - 2);
                        break;
                    case 'C':
                        numberStyles = NumberStyles.AllowCurrencySymbol | NumberStyles.AllowParentheses |
                                       NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign;
                        break;
                }

            return numberStyles;
        }
    }

    public class JsonString : IJsonPrimitive
    {
        public string Value { get; set; }
        public bool EscapeString { get; set; }
        public JsonPrimitiveType Type => JsonPrimitiveType.String;

        public JsonString(string value = null, bool escapeString = true)
        {
            Value = value;
            EscapeString = escapeString;
        }

        public object ToObject(DataType targetType, IReadOnlyConverterSettings settings)
        {
            var flags = targetType.Flags & DataTypeFlags.PrimitiveMask;
            if (flags == 0)
                throw new ArgumentException("Value has to be a primitive number value");

            var currentValue = Value;
            switch (flags)
            {
                case DataTypeFlags.Enum:
                    try
                    {
                        return Enum.Parse(targetType.Type, currentValue);
                    }
                    catch (Exception e)
                    {
                        throw new FormatException($"Invalid format while parsing enum \"{currentValue}\" to target type {targetType.Type.Name}", e);
                    }
                case DataTypeFlags.SByte:
                {
                    if (JsonNumber.IsNativeIntFormat(settings.NumberFormat))
                        return (sbyte)JsonStrings.ParseLong(currentValue);

                    var numberStyles = JsonNumber.ToNumberStyles(settings.NumberFormat, ref currentValue);
                    if (sbyte.TryParse(currentValue, numberStyles, CultureInfo.InvariantCulture, out var value))
                        return value;

                    throw new FormatException($"Invalid format while parsing sbyte \"{currentValue}\" with format \"{settings.NumberFormat}\"");
                }
                case DataTypeFlags.Byte:
                {
                    if (JsonNumber.IsNativeIntFormat(settings.NumberFormat))
                        return (byte) JsonStrings.ParseULong(currentValue);

                    var numberStyles = JsonNumber.ToNumberStyles(settings.NumberFormat, ref currentValue);
                    if (byte.TryParse(currentValue, numberStyles, CultureInfo.InvariantCulture, out var value))
                        return value;
                    throw new FormatException($"Invalid format while parsing byte \"{currentValue}\" with format \"{settings.NumberFormat}\"");
                }
                case DataTypeFlags.Short:
                {
                    if (JsonNumber.IsNativeIntFormat(settings.NumberFormat))
                        return (short) JsonStrings.ParseLong(currentValue);

                    var numberStyles = JsonNumber.ToNumberStyles(settings.NumberFormat, ref currentValue);
                    if (short.TryParse(currentValue, numberStyles, CultureInfo.InvariantCulture, out var value))
                        return value;
                    throw new FormatException($"Invalid format while parsing short \"{currentValue}\" with format \"{settings.NumberFormat}\"");
                }
                case DataTypeFlags.UShort:
                {
                    if (JsonNumber.IsNativeIntFormat(settings.NumberFormat))
                        return (ushort) JsonStrings.ParseULong(currentValue);

                    var numberStyles = JsonNumber.ToNumberStyles(settings.NumberFormat, ref currentValue);
                    if (ushort.TryParse(currentValue, numberStyles, CultureInfo.InvariantCulture, out var value))
                        return value;
                    throw new FormatException($"Invalid format while parsing ushort \"{currentValue}\" with format \"{settings.NumberFormat}\"");
                }
                case DataTypeFlags.Int:
                {
                    if (JsonNumber.IsNativeIntFormat(settings.NumberFormat))
                        return (int) JsonStrings.ParseLong(currentValue);

                    var numberStyles = JsonNumber.ToNumberStyles(settings.NumberFormat, ref currentValue);
                    if (int.TryParse(currentValue, numberStyles, CultureInfo.InvariantCulture, out var value))
                        return value;
                    throw new FormatException($"Invalid format while parsing int \"{currentValue}\" with format \"{settings.NumberFormat}\"");
                }
                case DataTypeFlags.UInt:
                {
                    if (JsonNumber.IsNativeIntFormat(settings.NumberFormat))
                        return (uint) JsonStrings.ParseULong(currentValue);

                    var numberStyles = JsonNumber.ToNumberStyles(settings.NumberFormat, ref currentValue);
                    if (uint.TryParse(currentValue, numberStyles, CultureInfo.InvariantCulture, out var value))
                        return value;
                    throw new FormatException($"Invalid format while parsing uint \"{currentValue}\" with format \"{settings.NumberFormat}\"");
                }
                case DataTypeFlags.Long:
                {
                    if (JsonNumber.IsNativeIntFormat(settings.NumberFormat))
                        return JsonStrings.ParseLong(currentValue);

                    var numberStyles = JsonNumber.ToNumberStyles(settings.NumberFormat, ref currentValue);
                    if (long.TryParse(currentValue, numberStyles, CultureInfo.InvariantCulture, out var value))
                        return value;
                    throw new FormatException($"Invalid format while parsing long \"{currentValue}\" with format \"{settings.NumberFormat}\"");
                }
                case DataTypeFlags.ULong:
                {
                    if (JsonNumber.IsNativeIntFormat(settings.NumberFormat))
                        return JsonStrings.ParseULong(currentValue);

                    var numberStyles = JsonNumber.ToNumberStyles(settings.NumberFormat, ref currentValue);
                    if (ulong.TryParse(currentValue, numberStyles, CultureInfo.InvariantCulture, out var value))
                        return value;
                    throw new FormatException($"Invalid format while parsing ulong \"{currentValue}\" with format \"{settings.NumberFormat}\"");
                }
                case DataTypeFlags.Float:
                {
                    if (JsonNumber.IsNativeFloatFormat(settings.NumberFormat))
                        return (float) JsonStrings.ParseDouble(currentValue);

                    var numberStyles = JsonNumber.ToNumberStyles(settings.NumberFormat, ref currentValue);
                    if (float.TryParse(currentValue, numberStyles, CultureInfo.InvariantCulture, out var value))
                        return value;
                    throw new FormatException($"Invalid format while parsing float \"{currentValue}\" with format \"{settings.NumberFormat}\"");
                }
                case DataTypeFlags.Double:
                {
                    if (JsonNumber.IsNativeFloatFormat(settings.NumberFormat))
                        return JsonStrings.ParseDouble(currentValue);

                    var numberStyles = JsonNumber.ToNumberStyles(settings.NumberFormat, ref currentValue);
                    if (double.TryParse(currentValue, numberStyles, CultureInfo.InvariantCulture, out var value))
                        return value;
                    throw new FormatException($"Invalid format while parsing double \"{currentValue}\" with format \"{settings.NumberFormat}\"");
                }
                case DataTypeFlags.Decimal:
                {
                    var numberStyles = JsonNumber.ToNumberStyles(settings.NumberFormat, ref currentValue);
                    if (decimal.TryParse(currentValue, numberStyles, CultureInfo.InvariantCulture, out var value))
                        return value;
                    throw new FormatException($"Invalid format while parsing decimal \"{currentValue}\" with format \"{settings.NumberFormat}\"");
                }
                case DataTypeFlags.DateTime:
                {
                    var styles = settings.TimeFormat == "u"
                        ? DateTimeStyles.AssumeUniversal
                        : DateTimeStyles.AssumeLocal;

                    if (DateTime.TryParse(currentValue, CultureInfo.InvariantCulture, styles, out var value))
                        return value;
                    throw new FormatException($"Invalid format while parsing decimal \"{currentValue}\" with format \"{settings.NumberFormat}\"");
                }
                case DataTypeFlags.DateTimeOffset:
                {
                    var styles = settings.TimeFormat == "u"
                        ? DateTimeStyles.AssumeUniversal
                        : DateTimeStyles.AssumeLocal;

                    if (DateTimeOffset.TryParse(currentValue, CultureInfo.InvariantCulture, styles, out var value))
                        return value;
                    throw new FormatException($"Invalid format while parsing decimal \"{currentValue}\" with format \"{settings.NumberFormat}\"");
                }
                case DataTypeFlags.TimeSpan:
                {
                    if (TimeSpan.TryParse(currentValue, CultureInfo.InvariantCulture, out var value))
                        return value;
                    throw new FormatException($"Invalid format while parsing decimal \"{currentValue}\" with format \"{settings.NumberFormat}\"");
                }
                case DataTypeFlags.String:
                {
                    return Value;
                }
                default:
                    throw new NotSupportedException();
            }
        }

        public void AppendJson(StringBuilder stringBuilder)
        {
            if (EscapeString)
                JsonStrings.AppendJsonString(stringBuilder, Value, true);
            else
                stringBuilder.Append(Value);
        }

        public string ToJson()
        {
            return EscapeString ? JsonStrings.EscapeJsonString(Value, true) : Value;
        }
    }

    public class JsonNull : IJsonPrimitive
    {
        private JsonNull()
        {
        }

        public static IJsonPrimitive Instance { get; } = new JsonNull();
        public JsonPrimitiveType Type => JsonPrimitiveType.Null;

        public object ToObject(DataType targetType, IReadOnlyConverterSettings settings)
        {
            return targetType.Type.GetDefaultValue();
        }

        public void AppendJson(StringBuilder stringBuilder)
        {
            stringBuilder.Append("null");
        }

        public string ToJson()
        {
            return "null";
        }
    }

    public class JsonBool : IJsonPrimitive
    {
        private JsonBool(bool value)
        {
            Value = value;
        }

        public static IJsonPrimitive True { get; } = new JsonBool(true);
        public static IJsonPrimitive False { get; } = new JsonBool(false);

        public bool Value { get; set; }

        public object ToObject(DataType targetType, IReadOnlyConverterSettings settings)
        {
            if ((targetType.Flags & DataTypeFlags.Bool) != 0)
                return Value;

            if ((targetType.Flags & DataTypeFlags.String) != 0)
                return Value.ToString();

            throw new NotSupportedException(
                $"Primitive {Type} can not be converted to target type {targetType.Type.Name}");
        }

        public void AppendJson(StringBuilder stringBuilder)
        {
            stringBuilder.Append(ToJson());
        }

        public string ToJson()
        {
            return Value ? "true" : "false";
        }

        public JsonPrimitiveType Type => JsonPrimitiveType.Boolean;
    }


    public static class JsonPrimitives
    {
        public static IJsonPrimitive FromObject(object value, IReadOnlyConverterSettings settings,
            DataType dataType = null)
        {
            if (value == null)
                return Null;

            dataType ??= value.GetDataType();
            var flags = dataType.Flags & DataTypeFlags.PrimitiveMask;

            if (flags == 0)
                throw new ArgumentException("Value has to be a primitive value");


            string json;
            switch (flags)
            {
                case DataTypeFlags.Bool:
                    return (bool) value ? True : False;
                case DataTypeFlags.Guid:
                    json = ((IFormattable) value).ToString(settings.GuidFormat, CultureInfo.InvariantCulture);
                    return new JsonString(json);
                case DataTypeFlags.Enum:
                    json = ((IFormattable) value).ToString(settings.EnumFormat, CultureInfo.InvariantCulture);
                    return JsonNumber.IsNativeIntFormat(settings.EnumFormat) ? (IJsonPrimitive)new JsonNumber(json) : new JsonString(json);
                case DataTypeFlags.Char:
                    return new JsonString(new string((char) value, 1));
                case DataTypeFlags.Byte:
                case DataTypeFlags.UShort:
                case DataTypeFlags.UInt:
                case DataTypeFlags.ULong:
                case DataTypeFlags.SByte:
                case DataTypeFlags.Short:
                case DataTypeFlags.Int:
                case DataTypeFlags.Long:
                case DataTypeFlags.Float:
                case DataTypeFlags.Double:
                case DataTypeFlags.Decimal:
                    json = ((IFormattable) value).ToString(settings.NumberFormat, CultureInfo.InvariantCulture);
                    return JsonNumber.IsNativeNumberFormat(settings.NumberFormat) ? (IJsonPrimitive)new JsonNumber(json) : new JsonString(json);
                case DataTypeFlags.DateTime:
                case DataTypeFlags.DateTimeOffset:
                case DataTypeFlags.TimeSpan:
                    json = ((IFormattable) value).ToString(settings.TimeFormat, CultureInfo.InvariantCulture);
                    return new JsonString(json);
                case DataTypeFlags.String:
                    json = (string) value;
                    return new JsonString(json);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        public static IJsonPrimitive Null { get; } = JsonNull.Instance;
        public static IJsonPrimitive True { get; } = JsonBool.True;
        public static IJsonPrimitive False { get; } = JsonBool.True;
    }
}