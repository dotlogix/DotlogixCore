// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Session.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  09.02.2018
// LastEdited:  09.02.2018
// ==================================================

#region
using System;
using System.Threading.Tasks;
using DotLogix.Core.Caching;
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Core.Rest.Services.Sessions {
    public abstract class AsyncSessionManager<TSession> where TSession : ISession {
        private readonly Cache<Guid> _sessionCache;

        public TimeSpan SessionCleanupInterval { get; }
        public TimeSpan SessionTimeout { get; }

        protected AsyncSessionManager(int sessionCleanupInterval, int sessionTimeout) : this(TimeSpan.FromMilliseconds(sessionCleanupInterval), TimeSpan.FromMilliseconds(sessionTimeout)){
        }

        protected AsyncSessionManager(TimeSpan sessionCleanupInterval, TimeSpan sessionTimeout) {
            _sessionCache = new Cache<Guid>(sessionCleanupInterval);
            _sessionCache.ValuesRemoved += _sessionCache_ValuesRemoved;
            SessionCleanupInterval = sessionCleanupInterval;
            SessionTimeout = sessionTimeout;
        }

        private async void _sessionCache_ValuesRemoved(object sender, CacheCleanupEventArgs e) {
            foreach(var item in e.RemovedItems)
                await UnloadSessionAsync((TSession)item);
        }


        public async Task<TSession> RetrieveSessionAsync(Guid token) {
            if(_sessionCache.TryRetrieve(token, out TSession session) == false)
                session = await LoadSessionAsync(token);
            _sessionCache.Store(token, session, SessionTimeout);
            return session;
        }

        public void StoreSession(TSession session)
        {
            _sessionCache.Store(session.Token, session, SessionTimeout);
        }

        public bool TryRetrieveSession(Guid token, out TSession session) {
            return _sessionCache.TryRetrieve(token, out session);
        }

        protected abstract Task UnloadSessionAsync(TSession session);
        protected abstract Task<TSession> LoadSessionAsync(Guid token);
    }
}
