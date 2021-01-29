#region
using System;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes.Schema;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes.Formats.Json {
    public static class JsonPrimitives {
        public static IJsonPrimitive True { get; } = JsonBool.True;
        public static IJsonPrimitive False { get; } = JsonBool.False;

        public static IJsonPrimitive FromObject(object value, IReadOnlyConverterSettings settings, DataType dataType = null) {
            if(value == null)
                return null;

            dataType ??= value.GetDataType();
            var flags = dataType.Flags & DataTypeFlags.PrimitiveMask;

            if(flags == 0)
                throw new ArgumentException("Value has to be a primitive value");


            string json;
            switch(flags) {
                case DataTypeFlags.Bool:
                    return (bool)value ? True : False;
                case DataTypeFlags.Guid:
                    json = ((IFormattable)value).ToString(settings.GuidFormat, settings.FormatProvider);
                    return new JsonString(json);
                case DataTypeFlags.Enum:
                    json = ((IFormattable)value).ToString(settings.EnumFormat, settings.FormatProvider);
                    return JsonNumber.IsNativeIntFormat(settings.EnumFormat, settings.FormatProvider) ? (IJsonPrimitive)new JsonNumber(json) : new JsonString(json);
                case DataTypeFlags.Char:
                    return new JsonString(new string((char)value, 1));
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
                    json = ((IFormattable)value).ToString(settings.NumberFormat, settings.FormatProvider);
                    return JsonNumber.IsNativeNumberFormat(settings.NumberFormat, settings.FormatProvider) ? (IJsonPrimitive)new JsonNumber(json) : new JsonString(json);
                case DataTypeFlags.DateTime:
                case DataTypeFlags.DateTimeOffset:
                case DataTypeFlags.TimeSpan:
                    json = ((IFormattable)value).ToString(settings.TimeFormat, settings.FormatProvider);
                    return new JsonString(json);
                case DataTypeFlags.String:
                    json = (string)value;
                    return new JsonString(json);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
