using System.Text;
using DotLogix.Core.Extensions;
using DotLogix.Core.Types;

namespace DotLogix.Core.Nodes.Processor {
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
}