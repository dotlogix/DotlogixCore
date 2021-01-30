using DotLogix.Core.Nodes;
using DotLogix.Core.Nodes.Formats.Json;
using DotLogix.Core.Rest.Services;

namespace DotLogix.Core.Rest.Json {
    public class JsonNodesExtension : IWebServiceExtension {
        public const string JsonDataParamName = "nodes.json.jsonData";
        public const string JsonRawParamName = "nodes.json.jsonRaw";

        public string Name => nameof(JsonNodesExtension);
        public JsonConverterSettings ConverterSettings { get; }
        public bool UseAsDefault { get; }

        public JsonNodesExtension(JsonConverterSettings converterSettings, bool useAsDefault) {
            ConverterSettings = converterSettings;
            UseAsDefault = useAsDefault;
        }

        /// <inheritdoc />
        public void Configure(WebServiceSettings settings) {
            settings.Router.PreProcessors.Add(JsonNodesParseBodyPreProcessor.Instance);

            if (UseAsDefault) {
                settings.Router.DefaultResultWriter = JsonNodesResultWriter.Instance;
            }
        }
    }
}