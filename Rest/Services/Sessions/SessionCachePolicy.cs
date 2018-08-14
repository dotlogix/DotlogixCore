// ==================================================
// Copyright 2018(C) , DotLogix
// File:  SessionCachePolicy.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  05.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Core.Caching;
#endregion

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
