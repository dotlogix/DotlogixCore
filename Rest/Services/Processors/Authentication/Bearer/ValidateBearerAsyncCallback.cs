// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ValidateBearerAsyncCallback.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Threading.Tasks;
using DotLogix.Core.Rest.Server.Http;
#endregion

namespace DotLogix.Core.Rest.Services.Processors.Authentication.Bearer {
    public delegate Task ValidateBearerAsyncCallback(BearerAuthenticationMethod authenticationMethod, WebRequestResult webRequestResult, string token);
}
