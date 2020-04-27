using System;
using System.Collections.Generic;
using System.Net;
using System.Net.WebSockets;

namespace DotLogix.Core.Rest.WebSockets {
    public interface IAsyncWebSocketRequest : IDisposable {
        bool IsLocal { get; }
        Uri Url { get; }
        IDictionary<string, object> HeaderParameters { get; }
        CookieCollection Cookies { get; set; }
        WebSocket WebSocket { get; }
        object OriginalRequest { get; }
        void Dispose();
    }
}