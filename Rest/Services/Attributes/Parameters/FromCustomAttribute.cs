using DotLogix.Core.Rest.Services.Parameters;

namespace DotLogix.Core.Rest.Services.Attributes {
    public class FromCustomAttribute : ParameterSourceFilterAttribute {
        public FromCustomAttribute() : base(ParameterSources.Custom) { }
        public FromCustomAttribute(ParameterSources sources = ParameterSources.Any) : base(sources) { }
    }
}