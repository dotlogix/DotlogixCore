using System;
using System.Net.WebSockets;

namespace DotLogix.Core.Rest.Server.Http.WebSockets {
    public interface IWebSocketCloseMessage : IWebSocketMessage {
        DateTime TimeStamp { get; }
        WebSocketMessageType MessageType { get; }
        WebSocketMessageType OriginalMessageType { get; }
        WebSocketCloseStatus? CloseStatus { get; }
        string CloseStatusDescription { get; }
    }
}