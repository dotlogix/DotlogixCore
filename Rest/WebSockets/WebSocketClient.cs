using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotLogix.Core.Rest.WebSockets {
    public class WebSocketClient : IDisposable{
        private readonly WebSocket _webSocket;
        public WebSocketClient(WebSocket webSocket) {
            _webSocket = webSocket;
        }

        public WebSocketState State => _webSocket.State;
        public WebSocketCloseStatus? CloseStatus => _webSocket.CloseStatus;
        public string CloseStatusDescription => _webSocket.CloseStatusDescription;

        public Encoding Encoding { get; set; }
        public int SenderBufferSize { get; set; }
        public int ReceiverBufferSize { get; set; }

        #region Send

        public virtual async Task SendMessageAsync(IWebSocketMessage message, CancellationToken token = default) {
            EnsureOpenWrite();
            try {
                switch (message.MessageType) {
                    case WebSocketMessageType.Text:
                        await SendTextMessage((IWebSocketMessage<string>)message, token);
                        break;
                    case WebSocketMessageType.Binary:
                        await SendBinaryMessage((IWebSocketMessage<ArraySegment<byte>>)message, token);
                        break;
                    case WebSocketMessageType.Close:
                        await SendCloseMessage((IWebSocketCloseMessage)message, token);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            } catch {
                Dispose();
                throw;
            }
        }

        protected virtual async Task SendCloseMessage(IWebSocketCloseMessage message, CancellationToken token) {
            await _webSocket.CloseAsync(message.CloseStatus ?? WebSocketCloseStatus.NormalClosure, message.CloseStatusDescription, token);
        }

        protected virtual Task SendBinaryMessage(IWebSocketMessage<ArraySegment<byte>> message, CancellationToken token) {
            return WriteDataAsync(WebSocketMessageType.Binary, message.Data, token);
        }

        protected virtual Task SendTextMessage(IWebSocketMessage<string> message, CancellationToken token) {
            var textBytes = new ArraySegment<byte>(Encoding.GetBytes(message.Data));
            return WriteDataAsync(WebSocketMessageType.Text, textBytes, token);
        }

        private async Task WriteDataAsync(WebSocketMessageType messageType, ArraySegment<byte> arraySegment, CancellationToken token) {
            if (arraySegment.Array == null)
                throw new InvalidDataException("Can not write empty data segment");

            for(var i = arraySegment.Offset; i < arraySegment.Count; i += ReceiverBufferSize) {
                EnsureOpenWrite();

                var segmentOffset = arraySegment.Offset + i;
                var segmentEnd = Math.Min(arraySegment.Count, segmentOffset + SenderBufferSize);
                var segmentLength = segmentEnd - segmentOffset;
                var isEndOfMessage = segmentEnd >= arraySegment.Count;
                var segment = new ArraySegment<byte>(arraySegment.Array, segmentOffset, segmentLength);

                await _webSocket.SendAsync(segment, messageType, isEndOfMessage, token);
            }
        }
        #endregion

        #region Receive
        public virtual async Task<IWebSocketMessage> ReceiveMessageAsync(CancellationToken token = default) {
            EnsureOpenRead();

            try {
                var (result, memoryStream) = await ReceiveDataAsync(token);
                var utcNow = DateTime.UtcNow;
                if (result.CloseStatus.HasValue || result.MessageType == WebSocketMessageType.Close) {
                    return ParseCloseMessage(result, utcNow);
                }

                switch (result.MessageType) {
                    case WebSocketMessageType.Text:
                        return ParseTextMessage(result, memoryStream, utcNow);
                    case WebSocketMessageType.Binary:
                        return ParseBinaryMessage(result, memoryStream, utcNow);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            } catch {
                Dispose();
                throw;
            }
        }

        private void EnsureOpenRead() {
            if(_webSocket.State != WebSocketState.Open)
                throw new InvalidOperationException($"Can not read message from a websocket in state {_webSocket.State}");
        }

        private void EnsureOpenWrite() {
            if(_webSocket.State != WebSocketState.Open)
                throw new InvalidOperationException($"Can not write message to a websocket in state {_webSocket.State}");
        }

        protected virtual IWebSocketMessage ParseCloseMessage(WebSocketReceiveResult result, DateTime utcNow) {
            return new WebSocketCloseMessage(utcNow, result.CloseStatus, result.CloseStatusDescription, result.MessageType);
        }

        protected virtual IWebSocketMessage ParseBinaryMessage(WebSocketReceiveResult result, MemoryStream memoryStream, DateTime utcNow) {
            var segment = new ArraySegment<byte>(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
            return new WebSocketBinaryMessage(utcNow, segment);
        }

        protected virtual IWebSocketMessage ParseTextMessage(WebSocketReceiveResult result, MemoryStream memoryStream, DateTime utcNow) {
            var text = Encoding.GetString(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
            return new WebSocketTextMessage(utcNow, text);
        }

        private async Task<(WebSocketReceiveResult result, MemoryStream memoryStream)> ReceiveDataAsync(CancellationToken token) {
            var segment = new ArraySegment<byte>(new byte[ReceiverBufferSize]);
            MemoryStream memoryStream = null;
            WebSocketReceiveResult result;
            bool continueRead;
            do {
                EnsureOpenRead();
                result = await _webSocket.ReceiveAsync(segment, token);
                continueRead = result.EndOfMessage == false && result.CloseStatus.HasValue == false;

                if(segment.Array == null || segment.Count == 0)
                    continue;

                memoryStream ??= new MemoryStream(ReceiverBufferSize);
                await memoryStream.WriteAsync(segment.Array, segment.Offset, segment.Count, token);
            } while(continueRead);

            return (result, memoryStream);
        }

        #endregion

        /// <inheritdoc />
        public void Dispose() {
            _webSocket?.Dispose();
        }
    }
}