// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebServiceRouteBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Services.Processors;
using DotLogix.Core.Rest.Services.Writer;
#endregion

namespace DotLogix.Core.Rest.Server.Routes {
    public abstract class WebServiceRouteBase : IWebServiceRoute {
        public string Pattern { get; }

        protected WebServiceRouteBase(int routeIndex, string pattern, HttpMethods acceptedRequests, IWebRequestProcessor requestProcessor, int priority) {
            Pattern = pattern ?? throw new ArgumentNullException(nameof(pattern));
            AcceptedRequests = acceptedRequests;
            RequestProcessor = requestProcessor ?? throw new ArgumentNullException(nameof(requestProcessor));
            Priority = priority;
            RouteIndex = routeIndex;
            PreProcessors = new WebRequestProcessorCollection();
            PostProcessors = new WebRequestProcessorCollection();
        }

        public WebRequestProcessorCollection PostProcessors { get; }

        public WebRequestProcessorCollection PreProcessors { get; }

        public IWebRequestProcessor RequestProcessor { get; }
        public IAsyncWebRequestResultWriter WebRequestResultWriter { get; set; }
        public bool IsRooted { get; set; }
        public HttpMethods AcceptedRequests { get; }
        public int RouteIndex { get; }
        public int Priority { get; }

        public abstract RouteMatch Match(HttpMethods method, string path);

        public override string ToString() {
            return $"{GetType().Name} ({Pattern})";
        }
    }
}
