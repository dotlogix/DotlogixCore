using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DotLogix.Core.Rest.Events {
    public interface IHubEvent {
        string Name { get; }
        IEnumerable<IHubEventListener> Listeners { get; }

        Task<bool> DispatchAsync(IHubMessage message, CancellationToken token = default);

        bool Subscribe(IHubEventListener listener, object payload);
        bool Unsubscribe(IHubEventListener listener, object payload);
    }
}