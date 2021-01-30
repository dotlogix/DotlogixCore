using System;
using System.Reflection;
using DotLogix.Core.Extensions;
using DotLogix.Core.Rest.Json;
using DotLogix.Core.Rest.Services.Attributes;
using DotLogix.Core.Rest.Services.Parameters;

namespace DotLogix.Core.Rest.Services.Descriptors {
    public class ParameterDescriptor {
        public ParameterDescriptor(ParameterInfo parameterInfo) {
            ParameterInfo = parameterInfo;
            var attribute = parameterInfo.GetCustomAttribute<ParameterSourceFilterAttribute>();
            if(attribute != null) {
                ProviderFilter = attribute.MatchesProvider;
                Sources = attribute.Sources;
            }
        }

        public string Name => ParameterInfo.Name;
        public ParameterSources Sources { get; } = ParameterSources.Any;
        public ParameterInfo ParameterInfo { get; }
        public Type ParameterType => ParameterInfo.ParameterType;
        public bool IsOptional => ParameterInfo.HasDefaultValue;
        public object DefaultValue => ParameterType.GetDefaultValue();
        public Func<IParameterProvider, bool> ProviderFilter { get; }
    }
}
