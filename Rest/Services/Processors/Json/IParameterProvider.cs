using System.Collections.Generic;
using System.Reflection;
using DotLogix.Core.Rest.Server.Http.Context;
using DotLogix.Core.Rest.Services.Context;

namespace DotLogix.Core.Rest.Services.Processors.Json {
    public interface IParameterProvider {
        string Name { get; }
        ParameterSources Source { get; }
        IEnumerable<KeyValuePair<string, object>> EnumerateValues(WebRequestContext context);
        bool TryResolve(WebRequestContext context, ParameterInfo parameter, out object paramValue);
    }
}