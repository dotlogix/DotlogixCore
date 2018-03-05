using System;
using DotLogix.Core.Caching;

namespace DotLogix.Core.Rest.Services.Sessions {
    public class SessionCachePolicy : ICachePolicy {
        private readonly ISession _session;

        public SessionCachePolicy(ISession session) {
            _session = session;
        }

        public bool HasExpired(DateTime timeStampUtc) {
            return timeStampUtc > _session.ValidUntilUtc;
        }
    }
}