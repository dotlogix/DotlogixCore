using System;
using System.Collections.Generic;

namespace DotLogix.Core.Rest.Services.Sessions {
    public class SessionDiscardedEventArgs<TSession> : EventArgs {
        public IReadOnlyList<TSession> DiscardedSessions { get; }

        public SessionDiscardedEventArgs(IReadOnlyList<TSession> discardedSessions) {
            DiscardedSessions = discardedSessions;
        }
    }
}