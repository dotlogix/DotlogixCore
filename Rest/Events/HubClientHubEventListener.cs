using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotLogix.Core.Rest.Events {
    public class HubClientHubEventListener : IHubEventListener {
        public Guid Guid { get; }
        public IHubClient Client { get; }
        public Task<bool> InvokeAsync(IHubMessage message, CancellationToken cancellationToken = default) {
            return Client.SendMessageAsync(message, cancellationToken);
        }
    }
}