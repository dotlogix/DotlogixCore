using System;
using System.Net.WebSockets;

namespace DotLogix.Core.Rest.WebSockets {
    public class WebSocketBinaryMessage : IWebSocketMessage<ArraySegment<byte>> {
        public WebSocketBinaryMessage(ArraySegment<byte> data) {
            TimeStamp = DateTime.UtcNow;
            Data = data;
        }

        public WebSocketBinaryMessage(byte[] data) {
            TimeStamp = DateTime.UtcNow;
            Data = new ArraySegment<byte>(data);
        }

        public WebSocketBinaryMessage(DateTime timeStamp, ArraySegment<byte> data) {
            TimeStamp = timeStamp;
            Data = data;
        }

        public WebSocketBinaryMessage(DateTime timeStamp, byte[] data) {
            TimeStamp = timeStamp;
            Data = new ArraySegment<byte>(data);
        }

        public DateTime TimeStamp { get; }
        public WebSocketMessageType MessageType => WebSocketMessageType.Binary;
        object IWebSocketMessage.Data => Data;
        public ArraySegment<byte> Data { get; }
    }
}