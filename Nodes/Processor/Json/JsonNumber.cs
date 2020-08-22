using System;
using System.Globalization;
using DotLogix.Core.Types;

namespace DotLogix.Core.Nodes.Processor {
    public class JsonNumber : JsonConvertible, IJsonPrimitive
    {
        public JsonNumber(string value = null)
        {
            Value = value;
        }
        
        public JsonPrimitiveType Type => JsonPrimitiveType.Number;
        
        public void AppendJson(CharBuffer buffer)
        {
            buffer.Append(Value);
        }

        public string ToJson()
        {
            return Value;
        }

        protected override char ToChar(IReadOnlyConverterSettings settings)
        {
            return (char) JsonStrings.ParseUInt64(Value);
        }

        protected override double ToDouble(IReadOnlyConverterSettings settings)
        {
            return JsonStrings.ParseDouble(Value);
        }

        protected override float ToSingle(IReadOnlyConverterSettings settings)
        {
            return (float)JsonStrings.ParseDouble(Value);
        }

        protected override ulong ToUInt64(IReadOnlyConverterSettings settings)
        {
            return JsonStrings.ParseUInt64(Value);
        }

        protected override long ToInt64(IReadOnlyConverterSettings settings)
        {
            return JsonStrings.ParseInt64(Value);
        }

        protected override uint ToUInt32(IReadOnlyConverterSettings settings)
        {
            return (uint)JsonStrings.ParseUInt64(Value);
        }

        protected override int ToInt32(IReadOnlyConverterSettings settings)
        {
            return (int)JsonStrings.ParseInt64(Value);
        }

        protected override ushort ToUInt16(IReadOnlyConverterSettings settings)
        {
            return (ushort)JsonStrings.ParseUInt64(Value);
        }

        protected override short ToInt16(IReadOnlyConverterSettings settings)
        {
            return (short)JsonStrings.ParseInt64(Value);
        }

        protected override byte ToByte(IReadOnlyConverterSettings settings)
        {
            return (byte)JsonStrings.ParseUInt64(Value);
        }

        protected override sbyte ToSByte(IReadOnlyConverterSettings settings)
        {
            return (sbyte)JsonStrings.ParseInt64(Value);
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
}