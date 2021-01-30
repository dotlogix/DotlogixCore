using DotLogix.Core.Rest.Services.Parameters;

namespace DotLogix.Core.Rest.Services.Attributes {
    public class FromUrlAttribute : ParameterSourceFilterAttribute {
        public FromUrlAttribute() : base(ParameterSources.Url) { }
    }
}