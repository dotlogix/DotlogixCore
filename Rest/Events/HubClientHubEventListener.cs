using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotLogix.Core.Rest.Events {
    public class HubClientHubEventListener : IHubEventListener {
        public Guid Guid { get; }
        public IHubClient Client { get; }
        public ValueTask<bool> InvokeAsync(IHubMessage message, CancellationToken cancellationToken = default) {
            //Client.SendMessageAsync()
            return new ValueTask<bool>(true);
        }
    }
}