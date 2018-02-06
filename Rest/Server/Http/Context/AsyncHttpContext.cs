// ==================================================
// Copyright 2018(C) , DotLogix
// File:  AsyncHttpContext.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  29.01.2018
// LastEdited:  31.01.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Rest.Server.Http.Context {
    public class AsyncHttpContext : IAsyncHttpContext {
        public AsyncHttpContext(IAsyncHttpServer server, IAsyncHttpRequest request, IAsyncHttpResponse response) {
            Server = server ?? throw new ArgumentNullException(nameof(server));
            Request = request ?? throw new ArgumentNullException(nameof(request));
            Response = response ?? throw new ArgumentNullException(nameof(response));
        }

        public IAsyncHttpServer Server { get; }
        public IAsyncHttpRequest Request { get; }
        public IAsyncHttpResponse Response { get; }
        public bool PreventAutoSend { get; set; }
    }
}
