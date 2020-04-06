// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IAuthenticationMethod.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Threading.Tasks;
using DotLogix.Core.Rest.Services.Context;
#endregion

namespace DotLogix.Core.Rest.Authentication.Base {
    public interface IAuthenticationMethod {
        string Name { get; }
        string[] SupportedDataFormats { get; }

        Task AuthenticateAsync(WebRequestContext context, string authorizationData);
    }
}
