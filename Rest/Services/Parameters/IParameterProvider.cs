using System.Collections.Generic;
using System.Reflection;

namespace DotLogix.Core.Rest.Services.Parameters {
    public interface IParameterProvider {
        string Name { get; }
        ParameterSources Source { get; }
        IEnumerable<KeyValuePair<string, object>> EnumerateValues(WebServiceContext context);
        bool TryResolve(WebServiceContext context, ParameterInfo parameter, out object paramValue);
    }
}