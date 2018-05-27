using DotLogix.Core.Rest.Services.Writer;

namespace DotLogix.Core.Rest.Services.Attributes.Routes {
    public class JsonRouteResultWriterAttribute : RouteResultWriterAttribute {
        public override IAsyncWebRequestResultWriter CreateResultWriter() {
            return WebRequestResultJsonWriter.Instance;
        }
    }
}