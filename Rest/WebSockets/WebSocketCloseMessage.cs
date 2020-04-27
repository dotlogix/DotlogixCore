using System;
using System.Net.WebSockets;

namespace DotLogix.Core.Rest.Server.Http.WebSockets {
    public class WebSocketCloseMessage : IWebSocketCloseMessage {
        public WebSocketCloseMessage(DateTime timeStamp, WebSocketCloseStatus? closeStatus = default, string closeStatusDescription = null, WebSocketMessageType originalMessageType = WebSocketMessageType.Close) {
            TimeStamp = timeStamp;
            OriginalMessageType = originalMessageType;
            CloseStatus = closeStatus;
            CloseStatusDescription = closeStatusDescription;
        }

        public DateTime TimeStamp { get; }
        public WebSocketMessageType MessageType => WebSocketMessageType.Close;
        public WebSocketMessageType OriginalMessageType { get; }
        public WebSocketCloseStatus? CloseStatus { get; }
        public string CloseStatusDescription { get; }
        object IWebSocketMessage.Data => null;
    }
}