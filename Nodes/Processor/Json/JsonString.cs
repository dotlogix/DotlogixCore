using System.Text;

namespace DotLogix.Core.Nodes.Processor {
    public class JsonString : JsonConvertible, IJsonPrimitive
    {
        public string Value { get; set; }
        public bool EscapeString { get; set; }
        public JsonPrimitiveType Type => JsonPrimitiveType.String;

        public JsonString(string value = null, bool escapeString = true)
        {
            Value = value;
            EscapeString = escapeString;
        }
        
        public void AppendJson(StringBuilder stringBuilder)
        {
            if (EscapeString)
                JsonStrings.AppendJsonString(stringBuilder, Value, true);
            else
                stringBuilder.Append(Value);
        }

        public string ToJson()
        {
            return EscapeString ? JsonStrings.EscapeJsonString(Value, true) : Value;
        }
    }
}