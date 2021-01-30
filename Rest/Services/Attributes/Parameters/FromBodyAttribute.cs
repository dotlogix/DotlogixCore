using DotLogix.Core.Rest.Services.Parameters;

namespace DotLogix.Core.Rest.Services.Attributes {
    public class FromBodyAttribute : ParameterSourceFilterAttribute {
        public FromBodyAttribute() : base(ParameterSources.Body) { }
    }
}