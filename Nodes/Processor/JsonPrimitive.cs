using System;
using System.Globalization;
using DotLogix.Core.Extensions;
using DotLogix.Core.Types;

namespace DotLogix.Core.Nodes.Processor {
    public struct JsonPrimitive {
        public readonly JsonPrimitiveType Type;
        public readonly string Json;

        public JsonPrimitive(JsonPrimitiveType type, string json) {
            Type = type;
            Json = json;
        }


        public object ToObject(Type targetType, IReadOnlyConverterSettings settings) {
            var currentValue = Json;
            if(currentValue == null || currentValue == "null")
                return targetType.GetDefaultValue();

            var dataType = targetType.ToDataType();
            var flags = dataType.Flags & DataTypeFlags.PrimitiveMask;

            if (flags == 0)
                throw new ArgumentException("Value has to be a primitive value");

            switch (flags) {
                case DataTypeFlags.Bool:
                    return currentValue switch
                    {
                        "true" => true,
                        "false" => false,
                        _ => throw new FormatException($"Invalid format while parsing boolean \"{currentValue}\" with format \"{settings.GuidFormat}\"")
                    };

                case DataTypeFlags.Guid:
                    Guid guid;
                    if(settings.GuidFormat == null) {
                        if(Guid.TryParse(currentValue, out guid))
                            return guid;
                    } else if (Guid.TryParseExact(currentValue, settings.GuidFormat, out guid)) {
                        return guid;
                    }
                    throw new FormatException($"Invalid format while parsing guid \"{currentValue}\" with format \"{settings.GuidFormat}\"");
                case DataTypeFlags.Enum:
                    try {
                        return Enum.Parse(targetType, currentValue);
                    } catch(Exception e) {
                        throw new FormatException($"Invalid format while parsing enum \"{currentValue}\" to target type {targetType.Name}", e);
                    }
                case DataTypeFlags.Char:
                    if(currentValue.Length != 1)
                        throw new FormatException($"Invalid format while parsing char \"{currentValue}\" there should be exactly one character");
                    return currentValue[0];
                case DataTypeFlags.SByte: {
                    var numberStyles = ToNumberStyles(settings.NumberFormat, ref currentValue);
                    if(sbyte.TryParse(currentValue, numberStyles, settings.FormatProvider, out var value))
                        return value;
                    throw new FormatException($"Invalid format while parsing sbyte \"{currentValue}\" with format \"{settings.NumberFormat}\"");
                }
                case DataTypeFlags.Byte: {
                    var numberStyles = ToNumberStyles(settings.NumberFormat, ref currentValue);
                    if (byte.TryParse(currentValue, numberStyles, settings.FormatProvider, out var value))
                        return value;
                    throw new FormatException($"Invalid format while parsing byte \"{currentValue}\" with format \"{settings.NumberFormat}\"");
                }
                case DataTypeFlags.Short: {
                    var numberStyles = ToNumberStyles(settings.NumberFormat, ref currentValue);
                    if(short.TryParse(currentValue, numberStyles, settings.FormatProvider, out var value))
                        return value;
                    throw new FormatException($"Invalid format while parsing short \"{currentValue}\" with format \"{settings.NumberFormat}\"");
                }
                case DataTypeFlags.UShort: {
                    var numberStyles = ToNumberStyles(settings.NumberFormat, ref currentValue);
                    if(ushort.TryParse(currentValue, numberStyles, settings.FormatProvider, out var value))
                        return value;
                    throw new FormatException($"Invalid format while parsing ushort \"{currentValue}\" with format \"{settings.NumberFormat}\"");
                }
                case DataTypeFlags.Int: {
                    var numberStyles = ToNumberStyles(settings.NumberFormat, ref currentValue);
                    if(int.TryParse(currentValue, numberStyles, settings.FormatProvider, out var value))
                        return value;
                    throw new FormatException($"Invalid format while parsing int \"{currentValue}\" with format \"{settings.NumberFormat}\"");
                }
                case DataTypeFlags.UInt: {
                    var numberStyles = ToNumberStyles(settings.NumberFormat, ref currentValue);
                    if(uint.TryParse(currentValue, numberStyles, settings.FormatProvider, out var value))
                        return value;
                    throw new FormatException($"Invalid format while parsing uint \"{currentValue}\" with format \"{settings.NumberFormat}\"");
                }
                case DataTypeFlags.Long: {
                    var numberStyles = ToNumberStyles(settings.NumberFormat, ref currentValue);
                    if(long.TryParse(currentValue, numberStyles, settings.FormatProvider, out var value))
                        return value;
                    throw new FormatException($"Invalid format while parsing long \"{currentValue}\" with format \"{settings.NumberFormat}\"");
                }
                case DataTypeFlags.ULong: {
                    var numberStyles = ToNumberStyles(settings.NumberFormat, ref currentValue);
                    if(ulong.TryParse(currentValue, numberStyles, settings.FormatProvider, out var value))
                        return value;
                    throw new FormatException($"Invalid format while parsing ulong \"{currentValue}\" with format \"{settings.NumberFormat}\"");
                }
                case DataTypeFlags.Float: {
                    var numberStyles = ToNumberStyles(settings.NumberFormat, ref currentValue);
                    if(float.TryParse(currentValue, numberStyles, settings.FormatProvider, out var value))
                        return value;
                    throw new FormatException($"Invalid format while parsing float \"{currentValue}\" with format \"{settings.NumberFormat}\"");
                }
                case DataTypeFlags.Double: {
                    var numberStyles = ToNumberStyles(settings.NumberFormat, ref currentValue);
                    if(double.TryParse(currentValue, numberStyles, settings.FormatProvider, out var value))
                        return value;
                    throw new FormatException($"Invalid format while parsing double \"{currentValue}\" with format \"{settings.NumberFormat}\"");
                }
                case DataTypeFlags.Decimal: {
                    var numberStyles = ToNumberStyles(settings.NumberFormat, ref currentValue);
                    if(decimal.TryParse(currentValue, numberStyles, settings.FormatProvider, out var value))
                        return value;
                    throw new FormatException($"Invalid format while parsing decimal \"{currentValue}\" with format \"{settings.NumberFormat}\"");
                }
                case DataTypeFlags.DateTime: {
                    var timeStyles = settings.TimeFormat[0] == 'u'
                                     ? DateTimeStyles.AssumeUniversal
                                     : DateTimeStyles.AssumeLocal;

                    if(DateTime.TryParseExact(currentValue, settings.TimeFormat, settings.FormatProvider, timeStyles, out var value))
                        return value;
                    throw new FormatException($"Invalid format while parsing dateTime \"{currentValue}\" with format \"{settings.TimeFormat}\"");
                }
                case DataTypeFlags.DateTimeOffset: {
                    var timeStyles = settings.TimeFormat[0] == 'u'
                                     ? DateTimeStyles.AssumeUniversal
                                     : DateTimeStyles.AssumeLocal;

                    if(DateTimeOffset.TryParseExact(currentValue, settings.TimeFormat, settings.FormatProvider, timeStyles, out var value))
                        return value;
                    throw new FormatException($"Invalid format while parsing dateTimeOffset \"{currentValue}\" with format \"{settings.TimeFormat}\"");
                }

                case DataTypeFlags.TimeSpan: {
                    if (TimeSpan.TryParseExact(currentValue, settings.TimeFormat, settings.FormatProvider, TimeSpanStyles.None, out var value))
                        return value;
                    throw new FormatException($"Invalid format while parsing timeSpan \"{currentValue}\" with format \"{settings.TimeFormat}\"");
                }
                case DataTypeFlags.String:
                    return currentValue;
                default:
                    throw new ArgumentOutOfRangeException();
            }


        }

        private static NumberStyles ToNumberStyles(string currentFormat, ref string currentValue) {
            var numberStyles = NumberStyles.Any;
            if(currentFormat != null) {
                switch(char.ToUpperInvariant(currentFormat[0])) {
                    case 'X':
                        numberStyles = NumberStyles.AllowHexSpecifier | NumberStyles.HexNumber;
                        break;
                    case 'P':
                        currentValue = currentValue.Substring(0, currentValue.Length - 2);
                        break;
                    case 'C':
                        numberStyles = NumberStyles.AllowCurrencySymbol | NumberStyles.AllowParentheses;
                        break;
                }
            }

            return numberStyles;
        }

        public static JsonPrimitive FromObject(object value, IReadOnlyConverterSettings settings) {
            if(value == null)
                return Null;

            var dataType = value.GetDataType();
            var flags = dataType.Flags & DataTypeFlags.PrimitiveMask;

            if(flags == 0)
                throw new ArgumentException("Value has to be a primitive value");


            string json;
            switch (flags) {
                case DataTypeFlags.Bool:
                    return (bool)value ? True : False;
                case DataTypeFlags.Guid:
                    json = ((IFormattable)value).ToString(settings.GuidFormat, settings.FormatProvider);
                    return new JsonPrimitive(JsonPrimitiveType.String, json);
                case DataTypeFlags.Enum:
                    json = ((IFormattable)value).ToString(settings.EnumFormat, settings.FormatProvider);
                    return settings.EnumFormat == "D"
                           ? new JsonPrimitive(JsonPrimitiveType.Number, json)
                           : new JsonPrimitive(JsonPrimitiveType.String, json);
                case DataTypeFlags.Char:
                    return new JsonPrimitive(JsonPrimitiveType.String, new string((char)value, 1));
                case DataTypeFlags.SByte:
                case DataTypeFlags.Byte:
                case DataTypeFlags.Short:
                case DataTypeFlags.UShort:
                case DataTypeFlags.Int:
                case DataTypeFlags.UInt:
                case DataTypeFlags.Long:
                case DataTypeFlags.ULong:
                case DataTypeFlags.Float:
                case DataTypeFlags.Double:
                case DataTypeFlags.Decimal:
                    json = ((IFormattable)value).ToString(settings.NumberFormat, settings.FormatProvider);
                    if (!Equals(settings.FormatProvider, CultureInfo.InvariantCulture))
                        return new JsonPrimitive(JsonPrimitiveType.String, json);

                    switch (settings.NumberFormat[0]) {
                        case 'G':  
                        case 'D':  
                        case 'E':  
                        case 'F':
                            return new JsonPrimitive(JsonPrimitiveType.Number, json);
                        default:
                            return new JsonPrimitive(JsonPrimitiveType.String, json);
                    }
                    break;
                case DataTypeFlags.DateTime:
                case DataTypeFlags.DateTimeOffset:
                case DataTypeFlags.TimeSpan:
                    json = ((IFormattable)value).ToString(settings.TimeFormat, settings.FormatProvider);
                    return new JsonPrimitive(JsonPrimitiveType.String, json);
                case DataTypeFlags.String:
                    json = (string)value;
                    return new JsonPrimitive(JsonPrimitiveType.String, json);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static JsonPrimitive Null { get; } = new JsonPrimitive(JsonPrimitiveType.Null, "null");
        public static JsonPrimitive True { get; } = new JsonPrimitive(JsonPrimitiveType.Boolean, "true");
        public static JsonPrimitive False { get; } = new JsonPrimitive(JsonPrimitiveType.Boolean, "false");
    }
}
