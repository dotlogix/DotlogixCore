using System;
using System.Collections.Generic;
using System.Reflection;
using DotLogix.Core.Extensions;
using DotLogix.Core.Rest.Server.Http.Context;
using DotLogix.Core.Rest.Services.Context;

namespace DotLogix.Core.Rest.Services.Processors.Json {
    public class DictionaryParameterProvider : IParameterProvider {
        private readonly Func<WebRequestContext, IDictionary<string, object>> GetSourceFunc;

        public DictionaryParameterProvider(string name, ParameterSources source, Func<WebRequestContext, IDictionary<string, object>> getSourceFunc) {
            GetSourceFunc = getSourceFunc;
            Name = name;
            Source = source;
        }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public ParameterSources Source { get; }

        /// <inheritdoc />
        public IEnumerable<KeyValuePair<string, object>> EnumerateValues(WebRequestContext context) {
            return GetSourceFunc(context);
        }

        public bool TryResolve(WebRequestContext context, ParameterInfo parameter, out object paramValue) {
            var source = GetSourceFunc(context);
            if(source != null)
                return source.TryGetValueAs(parameter.Name, parameter.ParameterType, out paramValue);

            paramValue = null;
            return false;
        }
    }
}