// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ISessionManager.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Rest.Services.Sessions {
    public interface ISessionManager<TSession> where TSession : ISession {
        TimeSpan CleanupInterval { get; }
        TimeSpan SessionTimeout { get; }

        void Store(TSession session);
        bool TryLoad(Guid token, out TSession session);
        bool TryLoadAndRenew(Guid token, out TSession session);
    }
}
