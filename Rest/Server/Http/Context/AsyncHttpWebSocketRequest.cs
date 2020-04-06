using System.Collections.Generic;
using System.Net.WebSockets;
using DotLogix.Core.Rest.Server.Http.Parameters;

namespace DotLogix.Core.Rest.Server.Http.Context {
    public class AsyncHttpWebSocketRequest : IAsyncHttpWebSocketRequest {
        private AsyncHttpWebSocketRequest(HttpListenerWebSocketContext originalContext, IDictionary<string, object> headerParameters) {
            OriginalRequest = originalContext;
            HeaderParameters = headerParameters;

            WebSocket = originalContext.WebSocket;
            IsLocal = originalContext.IsLocal;
            OriginalRequest = originalContext;
        }

        public IDictionary<string, object> HeaderParameters { get; }

        /// <inheritdoc />
        public bool IsLocal { get; }
        public WebSocket WebSocket { get; }

        object IAsyncHttpWebSocketRequest.OriginalRequest => OriginalRequest;
        public HttpListenerWebSocketContext OriginalRequest { get; }
        
        /// <inheritdoc />
        public void Dispose() {
            WebSocket?.Dispose();
        }

        public static AsyncHttpWebSocketRequest Create(HttpListenerWebSocketContext originalContext, IParameterParser parameterParser) {
            var headerParameters = parameterParser.Deserialize(originalContext.Headers);
            return new AsyncHttpWebSocketRequest(originalContext, headerParameters);
        }
    }
}