using System;
using System.Text;
using DotLogix.Core.Nodes.Schema;
using DotLogix.Core.Types;

namespace DotLogix.Core.Nodes.Processor {
    public class JsonBool : IJsonPrimitive
    {
        public static string JsonTrue { get; } = "true";
        public static char[] JsonTrueChars { get; } = JsonTrue.ToCharArray();
        public static string JsonFalse { get; } = "false";
        public static char[] JsonFalseChars { get; } = JsonFalse.ToCharArray();

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
                return ToJson();

            throw new NotSupportedException(
                                            $"Primitive {Type} can not be converted to target type {targetType.Type.Name}");
        }

        public void AppendJson(CharBuffer buffer)
        {
            buffer.Append(Value ? JsonTrueChars : JsonFalseChars);
        }

        public string ToJson()
        {
            return Value ? JsonTrue : JsonFalse;
        }

        public JsonPrimitiveType Type => JsonPrimitiveType.Boolean;
    }
}