using System;
using System.Collections.Generic;
using System.Reflection;
using DotLogix.Core.Extensions;
using DotLogix.Core.Rest.Services.Descriptors;

namespace DotLogix.Core.Rest.Services.Parameters {
    public class DictionaryParameterProvider : IParameterProvider {
        private readonly Func<WebServiceContext, IDictionary<string, object>> GetSourceFunc;

        public DictionaryParameterProvider(string name, ParameterSources source, Func<WebServiceContext, IDictionary<string, object>> getSourceFunc) {
            GetSourceFunc = getSourceFunc;
            Name = name;
            Source = source;
        }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public ParameterSources Source { get; }

        /// <inheritdoc />
        public IEnumerable<KeyValuePair<string, object>> EnumerateValues(WebServiceContext context) {
            return GetSourceFunc(context);
        }

        public bool TryResolve(WebServiceContext context, ParameterDescriptor parameterDescriptor, out object paramValue) {
            var source = GetSourceFunc(context);
            if(source != null)
                return source.TryGetValueAs(parameterDescriptor.Name, parameterDescriptor.ParameterType, out paramValue);

            paramValue = null;
            return false;
        }
    }
}