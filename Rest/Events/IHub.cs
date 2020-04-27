using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DotLogix.Core.Rest.Events {
    public interface IHub {
        IEnumerable<IHubMessage> Messages { get; }
        IReadOnlyDictionary<Guid, IHubClient> Clients { get; }
        IReadOnlyDictionary<string, IHubEvent> Events { get; }

        bool RegisterEvent(IHubEvent hubEvent);
        bool UnregisterEvent(IHubEvent hubEvent);

        ValueTask<bool> DispatchAsync(IHubMessage message, CancellationToken token = default);
        bool Subscribe(string eventName, IHubClient client, object payload);
        bool Unsubscribe(string eventName, IHubClient client, object payload);


        void OnClientConnect(IHubClient client);
        void OnClientDisconnect(IHubClient client);
        void OnClientError(IHubClient client, Exception exception);
        void OnError(Exception exception);
    }
}
