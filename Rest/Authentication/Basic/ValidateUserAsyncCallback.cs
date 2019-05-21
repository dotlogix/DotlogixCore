// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ValidateUserAsyncCallback.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Threading.Tasks;
using DotLogix.Core.Rest.Server.Http;
#endregion

namespace DotLogix.Core.Rest.Authentication.Basic {
    public delegate Task ValidateUserAsyncCallback(BasicAuthenticationMethod authenticationMethod, WebRequestResult webRequestResult, string username, string password);
}
