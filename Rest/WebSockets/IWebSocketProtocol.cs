using System.Threading;
using System.Threading.Tasks;

namespace DotLogix.Core.Rest.Server.Http.WebSockets {
    public interface IWebSocketProtocol<TMessage> {
        string Name { get; }

        Task SendMessageAsync(WebSocketClient client, TMessage message, CancellationToken token = default);
        Task<TMessage> ReceiveMessageAsync(WebSocketClient client, CancellationToken token = default);
    }
}