using System;
using System.Net.WebSockets;

namespace DotLogix.Core.Rest.WebSockets {
    public interface IWebSocketMessage {
        DateTime TimeStamp { get; }
        WebSocketMessageType MessageType { get; }
        object Data { get; }
    }

    public interface IWebSocketMessage<TData> : IWebSocketMessage {
        new TData Data { get; }
    }
}