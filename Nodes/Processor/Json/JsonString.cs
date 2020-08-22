namespace DotLogix.Core.Nodes.Processor {
    public class JsonString : JsonConvertible, IJsonPrimitive
    {
        public bool EscapeString { get; set; }
        public JsonPrimitiveType Type => JsonPrimitiveType.String;

        public JsonString(string value = null, bool escapeString = true)
        {
            Value = value;
            EscapeString = escapeString;
        }
        
        public void AppendJson(CharBuffer buffer)
        {
            if (EscapeString)
                JsonStrings.AppendJsonString(buffer, Value, true);
            else
                buffer.Append(Value);
        }

        public string ToJson()
        {
            return EscapeString ? JsonStrings.EscapeJsonString(Value, true) : Value;
        }
    }
}