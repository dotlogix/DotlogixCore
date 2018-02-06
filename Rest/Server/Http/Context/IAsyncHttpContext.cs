// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IAsyncHttpContext.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  29.01.2018
// LastEdited:  31.01.2018
// ==================================================

namespace DotLogix.Core.Rest.Server.Http.Context {
    public interface IAsyncHttpContext {
        IAsyncHttpServer Server { get; }
        IAsyncHttpRequest Request { get; }
        IAsyncHttpResponse Response { get; }
        bool PreventAutoSend { get; set; }
    }
}
