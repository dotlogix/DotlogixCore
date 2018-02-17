// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Session.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Rest.Services.Sessions {
    public class Session : ISession {
        public Session(Guid token) {
            Token = token;
        }

        public Guid Token { get; }
        public DateTime ValidUntilUtc { get; private set; }

        public void Renew(DateTime validUntilUtc) {
            ValidUntilUtc = validUntilUtc;
        }
    }
}
