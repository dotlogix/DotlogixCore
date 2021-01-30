using System;
using System.Collections.Generic;
using System.Reflection;
using DotLogix.Core.Nodes;
using DotLogix.Core.Nodes.Formats.Json;
using DotLogix.Core.Rest.Services;
using DotLogix.Core.Rest.Services.Descriptors;
using DotLogix.Core.Rest.Services.Parameters;

namespace DotLogix.Core.Rest.Json {
    public class JsonNodesParameterProvider : IParameterProvider {
        /// <inheritdoc />
        public string Name { get; } = "JsonBody";

        /// <inheritdoc />
        public ParameterSources Source { get; } = ParameterSources.Body;

        public Node JsonRoot { get; }
        public JsonConverterSettings ConverterSettings { get; }

        /// <inheritdoc />
        public JsonNodesParameterProvider(Node jsonRoot, JsonConverterSettings converterSettings) {
            JsonRoot = jsonRoot;
            ConverterSettings = converterSettings;
        }

        /// <inheritdoc />
        public IEnumerable<KeyValuePair<string, object>> EnumerateValues(WebServiceContext context) {
            var values = new Dictionary<string, object>();
            if(JsonRoot is NodeMap nodeMap) {
                foreach(var property in nodeMap.Properties()) {
                    values.Add(property.Key, property.Value);
                }
            }
            values.Add(WebServiceJsonSettings.JsonDataParamName, JsonRoot);
            return values;
        }

        public bool TryResolve(WebServiceContext context, ParameterDescriptor parameterDescriptor, out object paramValue) {
            var node = JsonRoot;

            var parameterName = ConverterSettings.NamingStrategy.Rewrite(parameterDescriptor.Name);
            if(node is NodeMap nodeMap && nodeMap.TryGetChild(parameterName, out var childNode)) {
                node = childNode;
            }

            if (node == null) {
                paramValue = null;
                return false;
            }

            try {
                paramValue = node.ToObject(parameterDescriptor.ParameterType, ConverterSettings);
                return true;
            } catch (Exception e) {
                context.LogSource.Warn(e);
                // Not convertible exception ignored in a try method
                paramValue = null;
                return false;
            }
        }

    }
}