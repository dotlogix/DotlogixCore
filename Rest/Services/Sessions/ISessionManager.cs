using System;

namespace DotLogix.Core.Rest.Services.Sessions {
    public interface ISessionManager<TSession> where TSession : ISession {
        TimeSpan CleanupInterval { get; }
        TimeSpan SessionTimeout { get; }

        void Store(TSession session);
        bool TryLoad(Guid token, out TSession session);
        bool TryLoadAndRenew(Guid token, out TSession session);
    }
}