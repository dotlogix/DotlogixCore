using DotLogix.Core.Rest.Services.Writer;

namespace DotLogix.Core.Rest.Services.Attributes.Routes {
    public class StreamRouteResultWriterAttribute : RouteResultWriterAttribute {
        public override IAsyncWebRequestResultWriter CreateResultWriter() {
            return WebRequestResultStreamWriter.Instance;
        }
    }
}