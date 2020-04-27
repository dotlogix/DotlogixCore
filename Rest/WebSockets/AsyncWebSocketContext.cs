using System;
using System.Collections.Generic;
using System.Net;
using System.Net.WebSockets;
using DotLogix.Core.Rest.Http.Parameters;
using DotLogix.Core.Rest.Server.Http.WebSockets;

namespace DotLogix.Core.Rest.WebSockets {
    public class AsyncWebSocketContext : IAsyncWebSocketRequest {
        private AsyncWebSocketContext(HttpListenerWebSocketContext originalContext, IDictionary<string, object> headerParameters) {
            OriginalRequest = originalContext;
            HeaderParameters = headerParameters;
            Cookies = originalContext.CookieCollection;

            WebSocket = originalContext.WebSocket;
            IsLocal = originalContext.IsLocal;
            OriginalRequest = originalContext;
        }

        /// <inheritdoc />
        public bool IsLocal { get; }

        public Uri Url => OriginalRequest.RequestUri;
        public IDictionary<string, object> HeaderParameters { get; }
        public CookieCollection Cookies { get; set; }

        public WebSocket WebSocket { get; }
        public WebSocketClient WebSocketClient => new WebSocketClient(WebSocket);

        object IAsyncWebSocketRequest.OriginalRequest => OriginalRequest;
        public HttpListenerWebSocketContext OriginalRequest { get; }
        
        /// <inheritdoc />
        public void Dispose() {
            WebSocket?.Dispose();
        }

        public static AsyncWebSocketContext Create(HttpListenerWebSocketContext originalContext, IParameterParser parameterParser) {
            var headerParameters = parameterParser.Deserialize(originalContext.Headers);
            return new AsyncWebSocketContext(originalContext, headerParameters);
        }
    }
}