using System;
using System.Collections.Generic;
using System.Net.WebSockets;

namespace DotLogix.Core.Rest.Server.Http.Context {
    public interface IAsyncHttpWebSocketRequest : IDisposable {
        IDictionary<string, object> HeaderParameters { get; }
        bool IsLocal { get; }
        WebSocket WebSocket { get; }
        object OriginalRequest { get; }
    }
}