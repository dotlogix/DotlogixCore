// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebServiceContext.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Threading;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Server.Http.Context;
using DotLogix.Core.Rest.Server.Routes;
#endregion

namespace DotLogix.Core.Rest.Services.Context {
    public class WebServiceContext : IDisposable {
        private static readonly AsyncLocal<WebServiceContext> AsyncCurrent = new AsyncLocal<WebServiceContext>();
        public IDictionary<string, object> Variables { get; } = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        public static WebServiceContext Current => AsyncCurrent.Value;
        public static WebRequestResult CurrentRequestResult => Current.RequestResult;

        public IAsyncHttpContext HttpContext { get; }
        public IAsyncHttpRequest HttpRequest => HttpContext.Request;
        public IAsyncHttpResponse HttpResponse => HttpContext.Response;
        public Guid ContextId { get; } = Guid.NewGuid();
        public WebRequestResult RequestResult { get; }
        public IWebServiceRoute Route { get; }

        public WebServiceContext(IAsyncHttpContext httpContext, IWebServiceRoute route) {
            HttpContext = httpContext;
            Route = route;
            RequestResult = new WebRequestResult(httpContext);

            AsyncCurrent.Value = this;
        }

        public void Dispose() {
            AsyncCurrent.Value = null;
        }
    }
}
