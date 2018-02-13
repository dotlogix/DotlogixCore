// ==================================================
// Copyright 2018(C) , DotLogix
// File:  SessionManager.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  09.02.2018
// LastEdited:  10.02.2018
// ==================================================

#region
using System;
using System.Linq;
using DotLogix.Core.Caching;
#endregion

namespace DotLogix.Core.Rest.Services.Sessions {
    public class SessionManager<TSession> : ISessionManager<TSession> where TSession : ISession {
        private readonly Cache<Guid, TSession> _activeSessions;

        public SessionManager(TimeSpan cleanupInterval, TimeSpan sessionTimeout) {
            CleanupInterval = cleanupInterval;
            SessionTimeout = sessionTimeout;
            _activeSessions = new Cache<Guid, TSession>(cleanupInterval);
            _activeSessions.ItemsDiscarded += _activeSessions_ItemsDiscarded;
        }

        public TimeSpan CleanupInterval { get; }
        public TimeSpan SessionTimeout { get; }

        public void Store(TSession session) {
            _activeSessions.Store(session.Token, session, new SessionCachePolicy(session));
        }

        public bool TryLoad(Guid token, out TSession session) {
            return _activeSessions.TryRetrieve(token, out session);
        }

        public bool TryLoadAndRenew(Guid token, out TSession session) {
            if(_activeSessions.TryRetrieve(token, out session) == false)
                return false;
            RenewSession(session);
            return true;
        }

        public void StoreAndRenew(TSession session) {
            RenewSession(session);
            _activeSessions.Store(session.Token, session, new SessionCachePolicy(session));
        }

        public void RenewSession(TSession session) {
            session.Renew(DateTime.UtcNow + SessionTimeout);
        }

        public event EventHandler<SessionDiscardedEventArgs<TSession>> SessionsDiscarded;

        private void _activeSessions_ItemsDiscarded(object sender, CacheItemsDiscardedEventArgs<Guid, TSession> e) {
            SessionsDiscarded?.Invoke(this, new SessionDiscardedEventArgs<TSession>(e.DiscardedItems.Select(i => i.Value).ToList()));
        }
    }

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
