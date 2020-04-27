// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IAsyncHttpContext.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Rest.Http.Context {
    public interface IAsyncHttpContext : IDisposable {
        IAsyncWebServer Server { get; }
        IAsyncHttpRequest Request { get; }
        IAsyncHttpResponse Response { get; }
        bool PreventAutoSend { get; set; }
    }
}
