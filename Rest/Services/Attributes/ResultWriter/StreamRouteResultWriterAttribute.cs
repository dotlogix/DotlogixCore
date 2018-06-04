using DotLogix.Core.Rest.Services.Writer;

namespace DotLogix.Core.Rest.Services.Attributes.ResultWriter {
    public class StreamRouteResultWriterAttribute : RouteResultWriterAttribute {
        public override IAsyncWebRequestResultWriter CreateResultWriter() {
            return WebRequestResultStreamWriter.Instance;
        }
    }
}