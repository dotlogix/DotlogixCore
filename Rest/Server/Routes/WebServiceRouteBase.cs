// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebServiceRouteBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  29.01.2018
// LastEdited:  31.01.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using DotLogix.Core.Collections;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Services.Processors;
#endregion

namespace DotLogix.Core.Rest.Server.Routes {
    public abstract class WebServiceRouteBase : IWebServiceRoute {
        private readonly SortedCollection<IWebRequestProcessor> _postProcessors;
        private readonly SortedCollection<IWebRequestProcessor> _preProcessors;
        public string Pattern { get; }

        protected WebServiceRouteBase(int routeIndex, string pattern, HttpMethods acceptedRequests, IWebRequestProcessor requestProcessor, int priority) {
            Pattern = pattern ?? throw new ArgumentNullException(nameof(pattern));
            AcceptedRequests = acceptedRequests;
            RequestProcessor = requestProcessor ?? throw new ArgumentNullException(nameof(requestProcessor));
            Priority = priority;
            RouteIndex = routeIndex;
            _preProcessors = new SortedCollection<IWebRequestProcessor>(WebRequestProcessorComparer.Instance);
            _postProcessors = new SortedCollection<IWebRequestProcessor>(WebRequestProcessorComparer.Instance);
        }

        public IReadOnlyCollection<IWebRequestProcessor> PostProcessors => _postProcessors;
        public IReadOnlyCollection<IWebRequestProcessor> PreProcessors => _preProcessors;

        public IWebRequestProcessor RequestProcessor { get; }
        public IWebRequestResultWriter WebRequestResultWriter { get; set; }
        public HttpMethods AcceptedRequests { get; }
        public int RouteIndex { get; }
        public int Priority { get; }

        public abstract RouteMatch Match(HttpMethods method, string path);

        public override string ToString() {
            return $"{GetType().Name} ({Pattern})";
        }

        #region Processors
        public void AddPreProcessor(IWebRequestProcessor preProcessor) {
            _preProcessors.Add(preProcessor);
        }

        public void RemovePreProcessor(IWebRequestProcessor preProcessor) {
            _preProcessors.Remove(preProcessor);
        }

        public void AddPostProcessor(IWebRequestProcessor postProcessor) {
            _postProcessors.Add(postProcessor);
        }

        public void RemovePostProcessor(IWebRequestProcessor postProcessor) {
            _postProcessors.Remove(postProcessor);
        }
        #endregion
    }
}
