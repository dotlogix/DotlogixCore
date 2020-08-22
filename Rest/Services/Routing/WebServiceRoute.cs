// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebServiceRoute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Core.Rest.Events;
using DotLogix.Core.Rest.Http;
using DotLogix.Core.Rest.Services.Descriptors;
using DotLogix.Core.Rest.Services.Processors;
#endregion

namespace DotLogix.Core.Rest.Services.Routing {
    public interface IRouteMatchingStrategy {
        bool IsRooted { get; }
        string Pattern { get; }
        HttpMethods AcceptedMethods { get; }
        RouteMatch Match(HttpMethods method, string path);
    }

    public class WebServiceRoute : IWebServiceRoute {
        public WebServiceRoute(int routeIndex, IWebRequestProcessor requestProcessor, IRouteMatchingStrategy matchingStrategy, int priority) {
            MatchingStrategy = matchingStrategy ?? throw new ArgumentNullException(nameof(matchingStrategy));
            RequestProcessor = requestProcessor ?? throw new ArgumentNullException(nameof(requestProcessor));
            Priority = priority;
            MatchingStrategy = matchingStrategy;
            RouteIndex = routeIndex;
            PreProcessors = new WebRequestProcessorCollection();
            PostProcessors = new WebRequestProcessorCollection();
            Descriptors = new RouteDescriptorCollection();
            ParameterProviders = new ParameterProviderCollection();
        }

        /// <inheritdoc />
        public ParameterProviderCollection ParameterProviders { get; }

        /// <inheritdoc />
        public RouteDescriptorCollection Descriptors { get; }

        public WebRequestProcessorCollection PostProcessors { get; }

        public WebRequestProcessorCollection PreProcessors { get; }

        public IWebRequestProcessor RequestProcessor { get; set; }
        public IWebServiceResultWriter ResultWriter { get; set; }
        public bool IsRooted => MatchingStrategy.IsRooted;
        public IRouteMatchingStrategy MatchingStrategy { get; }
        public HttpMethods AcceptedRequests => MatchingStrategy.AcceptedMethods;
        public string Pattern => MatchingStrategy.Pattern;
        public int RouteIndex { get; }
        public int Priority { get; }

        public RouteMatch Match(HttpMethods method, string path) {
            return MatchingStrategy.Match(method, path);
        }

        public override string ToString() {
            return $"{GetType().Name} ({Pattern})";
        }
    }
}
