// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IAsyncHttpRequestHandler.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  29.01.2018
// LastEdited:  31.01.2018
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
