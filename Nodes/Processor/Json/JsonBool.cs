using System;
using System.Text;
using DotLogix.Core.Types;

namespace DotLogix.Core.Nodes.Processor {
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
}