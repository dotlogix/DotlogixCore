using DotLogix.Core.Nodes;
using DotLogix.Core.Nodes.Formats.Json;
using DotLogix.Core.Rest.Services;

namespace DotLogix.Core.Rest.Json {
    public static class WebServiceJsonExtensions {
        public static WebServiceSettings UseJson(this WebServiceSettings serviceSettings, JsonConverterSettings settings, bool useAsDefault = true) {
            serviceSettings.UseExtension(new JsonNodesExtension(settings, useAsDefault));
            return serviceSettings;
        }
    }
}