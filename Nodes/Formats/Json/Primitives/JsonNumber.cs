#region
using System;
using System.Globalization;
using DotLogix.Core.Nodes.Schema;
using DotLogix.Core.Nodes.Utils;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes.Formats.Json {
    public class JsonNumber : JsonConvertible {
        public JsonNumber(string value) : base(JsonPrimitiveType.Number, value) { }

        protected override char ToChar(IReadOnlyConverterSettings settings) {
            return (char)JsonStrings.ParseUInt64(Value);
        }

        protected override object ToBool(IReadOnlyConverterSettings settings) {
            return JsonStrings.ParseInt64(Value) > 0;
        }

        protected override double ToDouble(IReadOnlyConverterSettings settings) {
            return JsonStrings.ParseDouble(Value);
        }

        protected override float ToSingle(IReadOnlyConverterSettings settings) {
            return (float)JsonStrings.ParseDouble(Value);
        }

        protected override ulong ToUInt64(IReadOnlyConverterSettings settings) {
            return JsonStrings.ParseUInt64(Value);
        }

        protected override long ToInt64(IReadOnlyConverterSettings settings) {
            return JsonStrings.ParseInt64(Value);
        }

        protected override uint ToUInt32(IReadOnlyConverterSettings settings) {
            return (uint)JsonStrings.ParseUInt64(Value);
        }

        protected override int ToInt32(IReadOnlyConverterSettings settings) {
            return (int)JsonStrings.ParseInt64(Value);
        }

        protected override ushort ToUInt16(IReadOnlyConverterSettings settings) {
            return (ushort)JsonStrings.ParseUInt64(Value);
        }

        protected override short ToInt16(IReadOnlyConverterSettings settings) {
            return (short)JsonStrings.ParseInt64(Value);
        }

        protected override byte ToByte(IReadOnlyConverterSettings settings) {
            return (byte)JsonStrings.ParseUInt64(Value);
        }

        protected override sbyte ToSByte(IReadOnlyConverterSettings settings) {
            return (sbyte)JsonStrings.ParseInt64(Value);
        }

        private static object ToType(DataType targetType, IConvertible value) {
            return targetType.Type.IsInstanceOfType(value)
                       ? value
                       : value.ToType(targetType.Type, CultureInfo.InvariantCulture);
        }

        public static bool IsNativeNumberFormat(string format, IFormatProvider formatProvider) {
            return IsNativeIntFormat(format, formatProvider) || IsNativeFloatFormat(format, formatProvider);
        }

        public static bool IsNativeIntFormat(string format, IFormatProvider formatProvider) {
            if((format.Length != 1) || !Equals(formatProvider, CultureInfo.InvariantCulture))
                return false;

            switch(format[0]) {
                case 'D':
                case 'F':
                case 'G':
                    return Equals(formatProvider, CultureInfo.InvariantCulture);
                default:
                    return false;
            }
        }

        public static bool IsNativeFloatFormat(string format, IFormatProvider formatProvider) {
            if((format.Length != 1) || !Equals(formatProvider, CultureInfo.InvariantCulture))
                return false;

            switch(char.ToUpperInvariant(format[0])) {
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

        public static NumberStyles ToNumberStyles(string format, ref string currentValue, NumberStyles defaultStyle = NumberStyles.Any) {
            if(format.Length != 1)
                return defaultStyle;

            switch(char.ToUpperInvariant(format[0])) {
                case 'X':
                    return NumberStyles.HexNumber;
                case 'P':
                    currentValue = currentValue.Substring(0, currentValue.Length - 2);
                    return NumberStyles.Number;
                case 'C':
                    return NumberStyles.Currency;
            }

            return defaultStyle;
        }
    }
}
