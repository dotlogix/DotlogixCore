using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotLogix.Core.Rest.Events {
    public interface IHubEventListener {
        Guid Guid { get; }
        Task<bool> InvokeAsync(IHubMessage message, CancellationToken cancellationToken = default);
    }
}