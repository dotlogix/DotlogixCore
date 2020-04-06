using System.Collections.Generic;
using System.Threading.Tasks;
using DotLogix.Core.Rest.Server.Http.Context;

namespace DotLogix.Core.Rest.Server.Http {
    public interface IAsyncHttpWebSocketRequestHandler {
        IReadOnlyCollection<string> SupportedSubProtocols { get; }
        Task HandleRequestAsync(IAsyncHttpWebSocketRequest webSocketRequest);
    }
}