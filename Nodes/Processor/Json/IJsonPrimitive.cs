using System.Text;
using DotLogix.Core.Types;

namespace DotLogix.Core.Nodes.Processor {
    public interface IJsonPrimitive
    {
        JsonPrimitiveType Type { get; }
        object ToObject(DataType targetType, IReadOnlyConverterSettings settings);
        void AppendJson(StringBuilder stringBuilder);
        string ToJson();
    }
}