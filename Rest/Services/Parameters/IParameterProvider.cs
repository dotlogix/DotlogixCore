using System.Collections.Generic;
using System.Reflection;
using DotLogix.Core.Rest.Services.Descriptors;

namespace DotLogix.Core.Rest.Services.Parameters {
    public interface IParameterProvider {
        string Name { get; }
        ParameterSources Source { get; }
        IEnumerable<KeyValuePair<string, object>> EnumerateValues(WebServiceContext context);
        bool TryResolve(WebServiceContext context, ParameterDescriptor parameterDescriptor, out object paramValue);
    }
}