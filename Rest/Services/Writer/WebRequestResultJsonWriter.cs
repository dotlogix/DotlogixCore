using System.Threading.Tasks;
using DotLogix.Core.Nodes;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Server.Http.Context;
using DotLogix.Core.Rest.Server.Http.Mime;
using DotLogix.Core.Rest.Server.Http.State;

namespace DotLogix.Core.Rest.Services.Writer {
    public class WebRequestResultJsonWriter : WebRequestResultWriterBase
    {
        public static IAsyncWebRequestResultWriter Instance { get; } = new WebRequestResultJsonWriter();
        private WebRequestResultJsonWriter() { }


        protected override Task WriteResultAsync(IAsyncHttpResponse asyncHttpResponse, WebRequestResult webRequestResult) {
            if (webRequestResult.ReturnValue == null)
            {
                asyncHttpResponse.StatusCode = HttpStatusCodes.Success.NoContent;
                return Task.CompletedTask;
            }

            asyncHttpResponse.ContentType = MimeTypes.Application.Json;
            return asyncHttpResponse.WriteToResponseStreamAsync(JsonNodes.ToJson(webRequestResult.ReturnValue));
        }
    }
}