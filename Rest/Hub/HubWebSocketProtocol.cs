using System.IO;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Core.Nodes;
using DotLogix.Core.Rest.Events;

namespace DotLogix.Core.Rest.Server.Http.WebSockets {
    public class HubWebSocketProtocol : IWebSocketProtocol<IHubMessage> {
        /// <inheritdoc />
        public string Name => "Hub";

        /// <inheritdoc />
        public Task SendMessageAsync(WebSocketClient client, IHubMessage message, CancellationToken token = default) {
            var nodeMap = new NodeMap();
            nodeMap.CreateValue("uid", message.Guid);
            nodeMap.CreateValue("t", message.Type);
            nodeMap.CreateValue("ts", message.TimestampUtc);

            nodeMap.AddChild("d", Nodes.Nodes.ToNode(message.Payload));

            var json = nodeMap.ToJson();
            return client.SendMessageAsync(new WebSocketTextMessage(message.TimestampUtc, json), token);
        }

        /// <inheritdoc />
        public async Task<IHubMessage> ReceiveMessageAsync(WebSocketClient client, CancellationToken token = default) {
            var message = await client.ReceiveMessageAsync(token);
            if(message.MessageType != WebSocketMessageType.Text)
                throw new InvalidDataException("Can not use binary data when communicating with the hub");

            var node = JsonNodes.ToNode((string)message.Data);
            
        }
    }
}