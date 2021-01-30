using DotLogix.Core.Rest.Services.Parameters;

namespace DotLogix.Core.Rest.Services.Attributes {
    public class FromHeaderAttribute : ParameterSourceFilterAttribute {
        public FromHeaderAttribute() : base(ParameterSources.Header) { }
    }
}