using System;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Core.Extensions;

namespace DotLogix.Core.Rest.Server.Http.WebSockets {
    public class WebSocketProtocol<TMessage> : IWebSocketProtocol<TMessage> {
        /// <inheritdoc />
        public string Name { get; }

        private readonly Func<TMessage, IWebSocketMessage> _serializeFunc;
        private readonly Func<IWebSocketMessage, TMessage> _deserializeFunc;

        /// <inheritdoc />
        public WebSocketProtocol(string name, Func<TMessage, IWebSocketMessage> serializeFunc, Func<IWebSocketMessage, TMessage> deserializeFunc) {
            Name = name;
            _serializeFunc = serializeFunc;
            _deserializeFunc = deserializeFunc;
        }


        /// <inheritdoc />
        public Task SendMessageAsync(WebSocketClient client, TMessage message, CancellationToken token = default) {
            return client.SendMessageAsync(_serializeFunc.Invoke(message), token);
        }

        /// <inheritdoc />
        public Task<TMessage> ReceiveMessageAsync(WebSocketClient client, CancellationToken token = default) {
            return client.ReceiveMessageAsync(token).ConvertResult(m=> _deserializeFunc.Invoke(m.Result));
        }
    }
}