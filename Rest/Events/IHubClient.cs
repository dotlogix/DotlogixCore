using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotLogix.Core.Rest.Events {
    public interface IHubClient {
        Guid Guid { get; }
        HubClientStatus Status { get; }

        Task<bool> SendMessageAsync(IHubMessage message, CancellationToken token = default);
        Task<IHubMessage> ReceiveMessageAsync(CancellationToken token = default);
    }
}