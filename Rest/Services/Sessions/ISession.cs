// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ISession.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Rest.Services.Sessions {
    public interface ISession {
        Guid Token { get; }
        DateTime ValidUntilUtc { get; }

        void Renew(DateTime validUntilUtc);
    }
}
