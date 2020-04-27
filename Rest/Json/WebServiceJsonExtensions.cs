using DotLogix.Core.Nodes;
using DotLogix.Core.Rest.Services;

namespace DotLogix.Core.Rest.Json {
    public static class WebServiceJsonExtensions {
        public static WebServiceHost UseJson(this WebServiceHost host, JsonFormatterSettings settings, bool setAsDefault = true) {
            host.Settings.Set(WebServiceJsonSettings.JsonNodesFormatterSettings, settings);
            host.GlobalPreProcessors.Add(JsonNodesParseBodyPreProcessor.Instance);

            if(setAsDefault) {
                host.Router.DefaultResultWriter = JsonNodesResultWriter.Instance;
            }

            return host;
        }
    }
}