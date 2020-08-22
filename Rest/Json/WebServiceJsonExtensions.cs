using DotLogix.Core.Nodes;
using DotLogix.Core.Rest.Services;

namespace DotLogix.Core.Rest.Json {
    public static class WebServiceJsonExtensions {
        public static WebServiceSettings UseJson(this WebServiceSettings serviceSettings, JsonFormatterSettings settings, bool useAsDefault = true) {
            serviceSettings.UseExtension(new JsonNodesExtension(settings, useAsDefault));
            return serviceSettings;
        }
    }

    public class JsonNodesExtension : IWebServiceExtension {
        public const string JsonDataParamName = "nodes.json.jsonData";
        public const string JsonRawParamName = "nodes.json.jsonRaw";

        public string Name => nameof(JsonNodesExtension);
        public JsonFormatterSettings FormatterSettings { get; }
        public bool UseAsDefault { get; }

        public JsonNodesExtension(JsonFormatterSettings formatterSettings, bool useAsDefault) {
            FormatterSettings = formatterSettings;
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