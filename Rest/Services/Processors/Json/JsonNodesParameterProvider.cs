using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotLogix.Core.Diagnostics;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes;
using DotLogix.Core.Rest.Server.Http.Context;
using DotLogix.Core.Rest.Services.Context;
using DotLogix.Core.Rest.Services.Writer;

namespace DotLogix.Core.Rest.Services.Processors.Json {
    public class JsonNodesParameterProvider : IParameterProvider {
        public const string JsonDataParamName = "$jsonData";
        public const string JsonRawParamName = "$jsonRaw";

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
        public IEnumerable<KeyValuePair<string, object>> EnumerateValues(WebRequestContext context) {
            var values = new Dictionary<string, object>();
            if(JsonRoot is NodeMap nodeMap) {
                foreach(var property in nodeMap.Properties()) {
                    values.Add(property.Key, property.Value);
                }
            }
            values.Add(JsonDataParamName, JsonRoot);
            return values;
        }

        public bool TryResolve(WebRequestContext context, ParameterInfo parameter, out object paramValue) {
            Node child = null;

            if(parameter.IsDefined(typeof(JsonBodyAttribute)))
                child = JsonRoot;
            else if(JsonRoot is NodeMap nodeMap)
                child = nodeMap.GetChild(parameter.Name);

            if (child == null) {
                paramValue = null;
                return false;
            }

            try {
                paramValue = child.ToObject(parameter.ParameterType, FormatterSettings);
                return true;
            } catch (Exception e) {
                Log.Warn(e);
                // Not convertible exception ignored in a try method
                paramValue = null;
                return false;
            }
        }

    }
}