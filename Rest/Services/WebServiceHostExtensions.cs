using System;
using DotLogix.Core.Nodes;
using DotLogix.Core.Rest.Services.Processors.Json;
using DotLogix.Core.Rest.Services.Writer;

namespace DotLogix.Core.Rest.Services {
    public static class WebServiceHostExtensions {
        public static WebServiceHost UseJson(this WebServiceHost host, JsonFormatterSettings settings, bool setAsDefault = true) {
            host.Settings.Set(WebServiceSettings.JsonNodesFormatterSettings, settings);
            host.GlobalPreProcessors.Add(JsonNodesParseBodyPreProcessor.Instance);

            if(setAsDefault) {
                host.Router.DefaultResultWriter = JsonNodesWebRequestResultWriter.Instance;
            }

            return host;
        }
    }
}