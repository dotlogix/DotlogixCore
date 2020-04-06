// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ValidateTokenAsyncCallback.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Threading.Tasks;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Services.Context;
#endregion

namespace DotLogix.Core.Rest.Authentication.Token {
    public delegate Task ValidateTokenAsyncCallback(TokenAuthenticationMethod authenticationMethod, WebRequestContext context, Guid token);
}
