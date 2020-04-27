using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotLogix.Core.Rest.Events {
    public interface IHubClient {
        Guid Guid { get; }
        HubClientStatus Status { get; }

        ValueTask<bool> SendMessageAsync(IHubMessage message, CancellationToken token = default);
        ValueTask<IHubMessage> ReceiveMessageAsync(CancellationToken token = default);
    }
}