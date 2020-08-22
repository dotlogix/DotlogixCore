using DotLogix.Core.Extensions;
using DotLogix.Core.Types;

namespace DotLogix.Core.Nodes.Processor {
    public class JsonNull : IJsonPrimitive
    {
        public static string Json {get; } = "null";
        public static char[] JsonChars { get; } = Json.ToCharArray();

        private JsonNull()
        {
        }

        public static IJsonPrimitive Instance { get; } = new JsonNull();
        public JsonPrimitiveType Type => JsonPrimitiveType.Null;

        public object ToObject(DataType targetType, IReadOnlyConverterSettings settings)
        {
            return targetType.Type.GetDefaultValue();
        }

        public void AppendJson(CharBuffer buffer)
        {
            buffer.Append(JsonChars);
        }

        public string ToJson()
        {
            return Json;
        }
    }
}