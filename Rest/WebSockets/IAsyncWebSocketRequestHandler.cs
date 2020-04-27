using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotLogix.Core.Rest.WebSockets {
    public interface IAsyncWebSocketRequestHandler {
        IReadOnlyCollection<string> SupportedSubProtocols { get; }
        Task HandleRequestAsync(IAsyncWebSocketRequest webSocketRequest);
    }
}