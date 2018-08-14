// ==================================================
// Copyright 2018(C) , DotLogix
// File:  SessionDiscardedEventArgs.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
#endregion

namespace DotLogix.Core.Rest.Services.Sessions {
    public class SessionDiscardedEventArgs<TSession> : EventArgs {
        public IReadOnlyList<TSession> DiscardedSessions { get; }

        public SessionDiscardedEventArgs(IReadOnlyList<TSession> discardedSessions) {
            DiscardedSessions = discardedSessions;
        }
    }
}
