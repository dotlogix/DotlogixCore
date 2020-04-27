using System;
using System.Data;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Core.Nodes;
using DotLogix.Core.Rest.Events;

namespace DotLogix.Core.Rest.Server.Http.WebSockets {
    public class JsonHubClient : IHubClient {
        private WebSocket _webSocket;

        public Guid Guid { get; } = Guid.NewGuid();

        /// <inheritdoc />
        public HubClientStatus Status => (HubClientStatus)_webSocket.State;

        public int SenderBufferSize { get; set; }
        public int ReceiverBufferSize { get; set; }

        /// <inheritdoc />
        public JsonHubClient(WebSocket webSocket) {
            _webSocket = webSocket;
        }

        /// <inheritdoc />
        public async ValueTask<bool> SendMessageAsync(IHubMessage message, CancellationToken token = default) {
            if (Status != HubClientStatus.Open) {
                throw new InvalidOperationException("Client is not connected");
            }

            //_webSocket.
            //var node = SerializeHubMessage(message);
            //var json = Encoding.UTF8.GetBytes(node.ToJson());
            //_webSocket.SendAsync(new ArraySegment<byte>(json), WebSocketMessageType.Text,)
            return false;
        }

        /// <inheritdoc />
        public async ValueTask<IHubMessage> ReceiveMessageAsync(CancellationToken token = default) {

            var (result, memoryStream) = await ReceiveWebSocketMessageAsync(token);
            var timeStampUtc = DateTime.UtcNow;

            if (result.CloseStatus.HasValue)
                return new HubMessage(HubMessageType.Disconnect, timeStampUtc);

            var isEmpty = memoryStream == null || memoryStream.Length == 0;
            switch (result.MessageType) {
                case WebSocketMessageType.Close:
                    return new HubMessage(HubMessageType.Disconnect, timeStampUtc);
                case WebSocketMessageType.Binary:
                    return ParseBinaryWebSocketMessage(isEmpty, timeStampUtc, memoryStream);
                case WebSocketMessageType.Text:
                    return ParseWebSocketTextMessage(memoryStream, timeStampUtc);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #region Helper

        private async Task<(WebSocketReceiveResult result, MemoryStream memoryStream)> ReceiveWebSocketMessageAsync(CancellationToken token) {
            var segment = new ArraySegment<byte>(new byte[ReceiverBufferSize]);
            MemoryStream memoryStream = null;
            WebSocketReceiveResult result;
            bool continueRead;
            do {
                result = await _webSocket.ReceiveAsync(segment, token);
                continueRead = result.EndOfMessage == false && result.CloseStatus.HasValue == false;
                if (continueRead == false || segment.Array == null)
                    continue;

                if (memoryStream == null)
                    memoryStream = new MemoryStream(ReceiverBufferSize);
                memoryStream.Write(segment.Array, segment.Offset, segment.Count);
            } while (continueRead);

            return (result, memoryStream);
        }

        private async Task<bool> SendWebSocketMessageAsync(IHubMessage message, CancellationToken token) {
            switch (message.Type) {
                case HubMessageType.Empty:
                    break;
                case HubMessageType.Connect:
                    break;
                case HubMessageType.Disconnect:
                    break;
                case HubMessageType.Subscribe:
                    break;
                case HubMessageType.Unsubscribe:
                    break;
                case HubMessageType.Text:
                    break;
                case HubMessageType.Binary:
                    break;
                case HubMessageType.Event:
                    break;
                case HubMessageType.Custom:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            var segment = new ArraySegment<byte>(new byte[ReceiverBufferSize]);
            MemoryStream memoryStream = null;
            WebSocketReceiveResult result;
            bool continueRead;
            do {
                result = await _webSocket.ReceiveAsync(segment, token);
                continueRead = result.EndOfMessage == false && result.CloseStatus.HasValue == false;
                if (continueRead == false || segment.Array == null)
                    continue;

                if (memoryStream == null)
                    memoryStream = new MemoryStream(ReceiverBufferSize);
                memoryStream.Write(segment.Array, segment.Offset, segment.Count);
            } while (continueRead);

            return (result, memoryStream);
        }

        #endregion

        #region ParseWebSocket

        private static IHubMessage ParseWebSocketTextMessage(MemoryStream memoryStream, DateTime timeStampUtc) {
            string text = null;
            if (memoryStream != null && memoryStream.TryGetBuffer(out var segment) && segment.Array != null) {
                text = Encoding.UTF8.GetString(segment.Array, segment.Offset, segment.Count);
            }

            if (text == null)
                return new HubMessage(HubMessageType.Empty, timeStampUtc);

            var nodeMap = JsonNodes.ToNode<NodeMap>(text);

            var typeNode = nodeMap.GetChild<NodeValue>("type");
            if (typeNode == null || typeNode.TryGetValue(out HubMessageType messageType) == false) {
                messageType = HubMessageType.Custom;
            }

            if (nodeMap.TryGetChildValue("timestampUtc", out DateTime timestampNodeValue))
                timeStampUtc = timestampNodeValue;

            if (nodeMap.TryGetChildValue("guid", out Guid guid) == false)
                guid = Guid.NewGuid();

            return DeserializeHubMessage(guid, messageType, timeStampUtc, nodeMap);
        }

        private static IHubMessage ParseBinaryWebSocketMessage(bool isEmpty, DateTime timeStampUtc, MemoryStream memoryStream) {
            if (isEmpty)
                return new HubMessage(HubMessageType.Empty, timeStampUtc);

            if (memoryStream.TryGetBuffer(out var segment))
                return new HubBinaryMessage(timeStampUtc, segment);
            throw new DataException("Received invalid binary message");
        }

        #endregion

        #region ParseHubMessage
        private static Node SerializeHubMessage(IHubMessage message) {
            string messageType = null;
            Node payload = null;
            switch (message.Type) {
                case HubMessageType.Empty:
                case HubMessageType.Connect:
                case HubMessageType.Disconnect:
                    payload = null;
                    break;
                case HubMessageType.Binary:
                    payload = SerializeHubBinaryMessage(message);
                    break;
                case HubMessageType.Text:
                    payload = new NodeValue((message as HubTextMessage)?.Payload);
                    break;
                case HubMessageType.Event:
                    payload = (message as HubEventMessage)?.Payload;
                    break;
                case HubMessageType.Custom:
                    var customMessage = (message as HubCustomMessage);
                    messageType = customMessage?.MessageType;
                    payload = customMessage?.Payload;
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Message type {message.Type} is invalid");
            }

            if (messageType == null) {
                messageType = message.Type.ToString().ToLower();
            }

            var nodeMap = new NodeMap();
            nodeMap.CreateValue("guid", message.Guid);
            nodeMap.CreateValue("type", messageType);
            nodeMap.CreateValue("timestampUtc", message.TimestampUtc);
            if (payload != null) {
                nodeMap.AddChild("payload", payload);
            }

            return nodeMap;
        }

        private static Node SerializeHubBinaryMessage(IHubMessage message) {
            if (!(message is HubBinaryMessage binary))
                return null;
            var segment = binary.Payload;
            if (segment.Array == null)
                return null;

            return new NodeValue(Convert.ToBase64String(segment.Array, segment.Offset, segment.Count));
        }

        private static IHubMessage DeserializeHubMessage(Guid guid, HubMessageType messageType, DateTime timeStampUtc, NodeMap nodeMap) {
            var payloadNode = nodeMap.GetChild("payload");
            switch (messageType) {
                case HubMessageType.Empty:
                case HubMessageType.Connect:
                case HubMessageType.Disconnect:
                    return new HubMessage(messageType, timeStampUtc, guid: guid);
                case HubMessageType.Subscribe:
                    return new HubSubscribeMessage(timeStampUtc, payloadNode, guid);
                case HubMessageType.Unsubscribe:
                    return new HubUnsubscribeMessage(timeStampUtc, payloadNode, guid);
                case HubMessageType.Text:
                    return DeserializeHubTextMessage(guid, timeStampUtc, payloadNode);
                case HubMessageType.Binary:
                    return DeserializeHubBinaryMessage(guid, timeStampUtc, payloadNode);
                case HubMessageType.Event:
                    return new HubEventMessage(timeStampUtc, payloadNode, guid);
                case HubMessageType.Custom:
                    return new HubCustomMessage(timeStampUtc, payloadNode, guid);
                default:
                    throw new DataException("Received invalid text message");
            }
        }
        private static IHubMessage DeserializeHubBinaryMessage(Guid guid, DateTime timeStampUtc, Node payloadNode) {
            if (payloadNode is NodeValue binaryValue) {
                var bytes = Convert.FromBase64String(binaryValue.Value.ToString());
                return new HubBinaryMessage(timeStampUtc, new ArraySegment<byte>(bytes), guid);
            }
            throw new DataException("Can not convert payload to byte[]");
        }
        private static IHubMessage DeserializeHubTextMessage(Guid guid, DateTime timeStampUtc, Node payloadNode) {
            if (payloadNode is NodeValue textValue) {
                return new HubTextMessage(timeStampUtc, textValue.Value.ToString(), guid);
            }
            throw new DataException("Can not convert payload to text");
        }

        #endregion

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public Task<bool> SendMessageAsync(WebSocket webSocket, IHubMessage message) {
            return null;
        }
    }
}