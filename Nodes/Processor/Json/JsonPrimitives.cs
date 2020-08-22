using System;
using System.Globalization;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes.Schema;
using DotLogix.Core.Types;

namespace DotLogix.Core.Nodes.Processor
{
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