// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ValidateTokenAsyncCallback.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.Threading.Tasks;
using DotLogix.Core.Rest.Server.Http;
#endregion

namespace DotLogix.Core.Rest.Services.Processors.Authentication.Token {
    public delegate Task ValidateTokenAsyncCallback(TokenAuthenticationMethod authenticationMethod, WebRequestResult webRequestResult, Guid token);
}
