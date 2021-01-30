using DotLogix.Core.Rest.Services.Parameters;

namespace DotLogix.Core.Rest.Services.Attributes {
    public class FromQueryAttribute : ParameterSourceFilterAttribute {
        public FromQueryAttribute() : base(ParameterSources.Query) { }
    }
}