using System;
using System.Net.WebSockets;

namespace DotLogix.Core.Rest.Server.Http.WebSockets {
    public class WebSocketTextMessage : IWebSocketMessage<string> {
        public WebSocketTextMessage(DateTime timeStamp, string data) {
            TimeStamp = timeStamp;
            Data = data;
        }

        public WebSocketTextMessage(string data) {
            TimeStamp = DateTime.UtcNow;
            Data = data;
        }

        public DateTime TimeStamp { get; }
        public WebSocketMessageType MessageType => WebSocketMessageType.Text;
        object IWebSocketMessage.Data => Data;
        public string Data { get; }
    }
}