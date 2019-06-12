using System.Collections.Generic;
using System.Collections.Specialized;

namespace DotLogix.Core.Rest.Server.Http.Parameters {
    public class ExtendedParameterParser : ParameterParserBase {
        public IParameterParser FallBackParser { get; }
        public Dictionary<string, IParameterParser> SpecializedParsers { get; } = new Dictionary<string, IParameterParser>();
        public static ExtendedParameterParser Default => CreateDefaultParser();

        public ExtendedParameterParser(IParameterParser fallBackParser = null) {
            FallBackParser = fallBackParser ?? PrimitiveParameterParser.Instance;
        }

        private static ExtendedParameterParser CreateDefaultParser() {
            return new ExtendedParameterParser();
        }

        public override void DeserializeValue(string name, string[] values, IDictionary<string, object> parameters) {
            if(SpecializedParsers.TryGetValue(name, out var parameterParser) == false)
                parameterParser = FallBackParser;
            parameterParser.DeserializeValue(name, values, parameters);
        }

        public override void SerializeValue(string name, object value, NameValueCollection collection) {
            if(SpecializedParsers.TryGetValue(name, out var parameterParser) == false)
                parameterParser = FallBackParser;
            parameterParser.SerializeValue(name, value, collection);
        }
    }
}