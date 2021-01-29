using DotLogix.Core.Nodes.Utils;

namespace DotLogix.Core.Nodes.Formats.Json {
    public class JsonString : JsonConvertible {
        public bool EscapeString { get; }

        public JsonString(string value = null, bool escapeString = true) : base(JsonPrimitiveType.String, value) {
            EscapeString = escapeString;
        }

        public override void AppendJson(CharBuffer buffer) {
            if(EscapeString)
                JsonStrings.AppendJsonString(buffer, Value, true);
            else
                buffer.Append(Value);
        }

        public override string ToJson() {
            return EscapeString ? JsonStrings.EscapeJsonString(Value, true) : Value;
        }
    }
}
