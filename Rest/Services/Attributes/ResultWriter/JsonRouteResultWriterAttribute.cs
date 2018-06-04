using DotLogix.Core.Rest.Services.Writer;

namespace DotLogix.Core.Rest.Services.Attributes.ResultWriter {
    public class JsonRouteResultWriterAttribute : RouteResultWriterAttribute {
        public override IAsyncWebRequestResultWriter CreateResultWriter() {
            return WebRequestResultJsonWriter.Instance;
        }
    }
}