using DotLogix.Core.Nodes.Schema;
using DotLogix.Core.Types;

namespace DotLogix.Core.Nodes.Processor {
    public interface IJsonPrimitive
    {
        JsonPrimitiveType Type { get; }
        object ToObject(DataType targetType, IReadOnlyConverterSettings settings);
        void AppendJson(CharBuffer buffer);
        string ToJson();
    }
}