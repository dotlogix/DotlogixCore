#region
using System;
using DotLogix.Core.Nodes.Schema;
using DotLogix.Core.Nodes.Utils;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes.Formats.Json {
    public class JsonBool : IJsonPrimitive {
        public JsonPrimitiveType Type => JsonPrimitiveType.Boolean;
        public static IJsonPrimitive True { get; } = new JsonBool(true);
        public static IJsonPrimitive False { get; } = new JsonBool(false);
        public bool Value { get; }


        private JsonBool(bool value) {
            Value = value;
        }


        public object ToObject(DataType targetType, IReadOnlyConverterSettings settings) {
            if((targetType.Flags & DataTypeFlags.Bool) != 0)
                return Value;

            if((targetType.Flags & DataTypeFlags.String) != 0)
                return ToJson();

            throw new NotSupportedException(
                                            $"Primitive {Type} can not be converted to target type {targetType.Type.Name}"
                                           );
        }

        public void AppendJson(CharBuffer buffer) {
            buffer.Append(Value ? JsonStrings.JsonTrueChars : JsonStrings.JsonFalseChars);
        }

        public string ToJson() {
            return Value ? JsonStrings.JsonTrue : JsonStrings.JsonFalse;
        }
    }
}
