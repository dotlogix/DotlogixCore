// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IAsyncHttpRequestHandler.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Threading.Tasks;
using DotLogix.Core.Rest.Server.Http.Context;
#endregion

namespace DotLogix.Core.Rest.Server.Http {
    public interface IAsyncHttpRequestHandler {
        Task HandleRequestAsync(IAsyncHttpContext asyncHttpContext);
    }
}
