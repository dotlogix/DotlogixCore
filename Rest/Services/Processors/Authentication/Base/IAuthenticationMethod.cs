// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IAuthenticationMethod.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Threading.Tasks;
using DotLogix.Core.Rest.Server.Http;
#endregion

namespace DotLogix.Core.Rest.Services.Processors.Authentication.Base {
    public interface IAuthenticationMethod {
        string Name { get; }
        string[] SupportedDataFormats { get; }

        Task AuthenticateAsync(WebRequestResult webRequestResult, string authorizationData);
    }
}
