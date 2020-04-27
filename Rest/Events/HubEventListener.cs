using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotLogix.Core.Rest.Events {
    public class HubEventListener : IHubEventListener {
        private readonly Func<IHubMessage, CancellationToken, ValueTask<bool>> _handler;
        private HubEventListener(Func<IHubMessage, CancellationToken, ValueTask<bool>> handler) {
            _handler = handler;
        }

        public Guid Guid { get; } = Guid.NewGuid();
        public ValueTask<bool> InvokeAsync(IHubMessage message, CancellationToken cancellationToken = default) {
            return _handler.Invoke(message, cancellationToken);
        }

        public static IHubEventListener Create(Func<IHubMessage, CancellationToken, ValueTask<bool>> handler) {
            return new HubEventListener(handler);
        }
        public static IHubEventListener Create(Func<IHubMessage, ValueTask<bool>> handler) {
            return new HubEventListener((ctx, token) => handler.Invoke(ctx));
        }

        public static IHubEventListener Create(Func<IHubMessage, bool> handler) {
            return new HubEventListener((ctx, token) => new ValueTask<bool>(handler.Invoke(ctx)));
        }

        public static IHubEventListener Create(Action<IHubMessage> handler) {
            return new HubEventListener((ctx, token) => {
                                                 handler.Invoke(ctx);
                                                 return new ValueTask<bool>(true);
                                             });
        }
    }
}