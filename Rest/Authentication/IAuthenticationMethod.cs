// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IAuthenticationMethod.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Threading.Tasks;
using DotLogix.Core.Rest.Services;
#endregion

namespace DotLogix.Core.Rest.Authentication {
    public interface IAuthenticationMethod {
        string Name { get; }
        string[] SupportedDataFormats { get; }

        Task AuthenticateAsync(WebServiceContext context, string authorizationData);
    }
}
