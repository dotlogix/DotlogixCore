#region
using DotLogix.Core.Nodes.Schema;
using DotLogix.Core.Nodes.Utils;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes.Formats.Json {
    public interface IJsonPrimitive {
        JsonPrimitiveType Type { get; }
        object ToObject(DataType targetType, IReadOnlyConverterSettings settings);
        void AppendJson(CharBuffer buffer);
        string ToJson();
    }
}
