using System;
using System.Collections.Generic;
using System.Reflection;
using DotLogix.Core.Nodes;
using DotLogix.Core.Rest.Services;
using DotLogix.Core.Rest.Services.Parameters;

namespace DotLogix.Core.Rest.Json {
    public class JsonNodesParameterProvider : IParameterProvider {
        /// <inheritdoc />
        public string Name { get; } = "JsonBody";

        /// <inheritdoc />
        public ParameterSources Source { get; } = ParameterSources.Body;

        public Node JsonRoot { get; }
        public JsonFormatterSettings FormatterSettings { get; }

        /// <inheritdoc />
        public JsonNodesParameterProvider(Node jsonRoot, JsonFormatterSettings formatterSettings) {
            JsonRoot = jsonRoot;
            FormatterSettings = formatterSettings;
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

        public bool TryResolve(WebServiceContext context, ParameterInfo parameter, out object paramValue) {
            Node child = null;

            if(parameter.IsDefined(typeof(JsonBodyAttribute)))
                child = JsonRoot;
            else if(JsonRoot is NodeMap nodeMap) {
                child = nodeMap.GetChild(FormatterSettings.NamingStrategy.Rewrite(parameter.Name));
            }

            if (child == null) {
                paramValue = null;
                return false;
            }

            try {
                paramValue = child.ToObject(parameter.ParameterType, FormatterSettings);
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