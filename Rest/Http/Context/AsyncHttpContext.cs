// ==================================================
// Copyright 2018(C) , DotLogix
// File:  AsyncHttpContext.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Threading;
#endregion

namespace DotLogix.Core.Rest.Http.Context {
    public class AsyncHttpContext : IAsyncHttpContext {
        private static readonly AsyncLocal<AsyncHttpContext> AsyncCurrent = new AsyncLocal<AsyncHttpContext>();
        public static AsyncHttpContext Current => AsyncCurrent.Value;

        public AsyncHttpContext(IAsyncWebServer server, IAsyncHttpRequest request, IAsyncHttpResponse response) {
            Server = server ?? throw new ArgumentNullException(nameof(server));
            Request = request ?? throw new ArgumentNullException(nameof(request));
            Response = response ?? throw new ArgumentNullException(nameof(response));

            AsyncCurrent.Value = this;
        }

        public IAsyncWebServer Server { get; }

        public IAsyncHttpRequest Request { get; }
        public IAsyncHttpResponse Response { get; }
        public bool PreventAutoSend { get; set; }

        public void Dispose() {
            AsyncCurrent.Value = null;
        }
    }
}
