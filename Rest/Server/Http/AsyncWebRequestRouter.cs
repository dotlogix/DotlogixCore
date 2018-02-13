// ==================================================
// Copyright 2018(C) , DotLogix
// File:  AsyncWebRequestRouter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  31.01.2018
// LastEdited:  01.02.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotLogix.Core.Collections;
using DotLogix.Core.Diagnostics;
using DotLogix.Core.Extensions;
using DotLogix.Core.Rest.Server.Http.Context;
using DotLogix.Core.Rest.Server.Http.State;
using DotLogix.Core.Rest.Server.Routes;
using DotLogix.Core.Rest.Services.Context;
using DotLogix.Core.Rest.Services.Processors;
#endregion

namespace DotLogix.Core.Rest.Server.Http {
    public class AsyncWebRequestRouter : IAsyncHttpRequestHandler {
        public const string EventSubscriptionParameterName = "$eventSubscription";

        public static readonly IComparer<IWebRequestProcessor> ProcessorPriorityComparer = new SelectorComparer<IWebRequestProcessor, int>(p => p.Priority);
        public static readonly IComparer<IWebServiceRoute> ServerRoutePriorityComparer = new SelectorComparer<IWebServiceRoute, int>(p => p.Priority);
        private readonly SortedCollection<IWebRequestProcessor> _globalPostProcessors = new SortedCollection<IWebRequestProcessor>(ProcessorPriorityComparer);

        private readonly SortedCollection<IWebRequestProcessor> _globalPreProcessors = new SortedCollection<IWebRequestProcessor>(ProcessorPriorityComparer);
        private readonly Dictionary<string, WebServiceEvent> _serverEvents = new Dictionary<string, WebServiceEvent>();
        private readonly SortedCollection<IWebServiceRoute> _serverRoutes = new SortedCollection<IWebServiceRoute>(ServerRoutePriorityComparer);


        public int RegisteredRoutesCount => _serverRoutes.Count;

        public IWebRequestResultWriter DefaultResultWriter { get; set; } = WebRequestResultJsonWriter.Instance;

        #region Subscription
        private class WebServiceEventSubscription : IWebServiceEventSubscription {
            private readonly IAsyncHttpContext _asyncHttpContext;
            private readonly IWebServiceRoute _route;

            private readonly AsyncWebRequestRouter _router;

            public WebServiceEventSubscription(IAsyncHttpContext asyncHttpContext, IWebServiceRoute route, AsyncWebRequestRouter router) {
                _asyncHttpContext = asyncHttpContext;
                _route = route;
                _router = router;
            }

            public async Task TriggerAsync() {
                var response = _asyncHttpContext.Response;
                try {
                    await _router.HandleAsync(_asyncHttpContext, _route);
                    if(response.IsSent == false)
                        await response.SendAsync();
                } catch(Exception e) {
                    Log.Error(e);
                    await AsyncHttpServer.SendErrorMessageAsync(response, e);
                }
            }
        }
        #endregion

        #region Processing
        async Task IAsyncHttpRequestHandler.HandleRequestAsync(IAsyncHttpContext asyncHttpContext) {
            if(TryGetRoute(asyncHttpContext, out var route) == false) {
                await asyncHttpContext.Response.SendAsync(HttpStatusCodes.NotFound);
                return;
            }

            if(asyncHttpContext.Request.HeaderParameters.TryGetParameterValue(EventSubscriptionParameterName, out var eventName)) {
                if(_serverEvents.TryGetValue(eventName.ToString(), out var serverEvent) == false) {
                    await asyncHttpContext.Response.WriteToResponseStreamAsync("The event subscription could not be handled, because the event is not registered on the server");
                    await asyncHttpContext.Response.SendAsync(HttpStatusCodes.BadRequest);
                    return;
                }
                asyncHttpContext.PreventAutoSend = true;
                serverEvent.Subscribe(new WebServiceEventSubscription(asyncHttpContext, route, this));
            } else {
                await HandleAsync(asyncHttpContext, route);
            }
        }

        internal async Task HandleAsync(IAsyncHttpContext asyncHttpContext, IWebServiceRoute route) {
            var webServiceContext = new WebServiceContext(asyncHttpContext);
            WebServiceContext.SetLocalContext(webServiceContext);

            // start of processing
            await ProcessRequest(route, webServiceContext.Result);
            // end of processing

            WebServiceContext.SetLocalContext(null);

            var writer = route.WebRequestResultWriter ?? DefaultResultWriter;
            await writer.WriteAsync(webServiceContext.Result);
        }

        private bool TryGetRoute(IAsyncHttpContext asyncHttpContext, out IWebServiceRoute route) {
            var asyncHttpRequest = asyncHttpContext.Request;

            var httpMethod = asyncHttpRequest.HttpMethod;
            var path = asyncHttpRequest.Url.LocalPath;

            route = null;
            RouteMatch routeMatch = null;
            foreach(var serverRoute in _serverRoutes) {
                var match = serverRoute.Match(httpMethod, path);
                if(match.Success == false)
                    continue;

                route = serverRoute;
                routeMatch = match;
                break;
            }

            if(routeMatch == null)
                return false;

            asyncHttpRequest.UrlParameters.AddRange(routeMatch.UrlParameters);
            return true;
        }

        private async Task ProcessRequest(IWebServiceRoute route, WebRequestResult result) {

            #region PreProcess
            if(_globalPreProcessors.Count > 0)
                await ExecuteProcessorsAsync(result, _globalPreProcessors);

            if(route.PreProcessors.Count > 0)
                await ExecuteProcessorsAsync(result, route.PreProcessors);
            #endregion

            if(ShouldExecute(result, route.RequestProcessor))
                await route.RequestProcessor.ProcessAsync(result);

            #region PostProcess
            if(_globalPostProcessors.Count > 0)
                await ExecuteProcessorsAsync(result, _globalPostProcessors);

            if(route.PostProcessors.Count > 0)
                await ExecuteProcessorsAsync(result, route.PostProcessors);
            #endregion
        }
        #endregion

        #region Processors
        public void AddGlobalPreProcessor(IWebRequestProcessor preProcessor) {
            _globalPreProcessors.Add(preProcessor);
        }

        public void RemoveGlobalPreProcessor(IWebRequestProcessor preProcessor) {
            _globalPreProcessors.Remove(preProcessor);
        }

        public void AddGlobalPostProcessor(IWebRequestProcessor postProcessor) {
            _globalPostProcessors.Add(postProcessor);
        }

        public void RemoveGlobalPostProcessor(IWebRequestProcessor postProcessor) {
            _globalPostProcessors.Remove(postProcessor);
        }
        #endregion

        #region Routes
        public void AddServerRoute(IWebServiceRoute route) {
            _serverRoutes.Add(route);
        }

        public void RemoveServerRoute(IWebServiceRoute route) {
            _serverRoutes.Remove(route);
        }
        #endregion

        #region Events
        public WebServiceEvent AddServerEvent(string name)
        {
            if(_serverEvents.ContainsKey(name))
                throw new InvalidOperationException("Event names should be unique");
            var webServiceEvent = new WebServiceEvent(name);
            _serverEvents.Add(name, webServiceEvent);
            return webServiceEvent;
        }

        public WebServiceEvent GetServerEvent(string name) {
            return _serverEvents.TryGetValue(name, out var serverEvent) ? serverEvent : null;
        }

        public WebServiceEvent GetOrAddServerEvent(string name) {
            if(_serverEvents.TryGetValue(name, out var serverEvent))
                return serverEvent;

            var webServiceEvent = new WebServiceEvent(name);
            _serverEvents.Add(name, webServiceEvent);
            return webServiceEvent;
        }

        public Task TriggerServerEventAsync(string name) {
            return _serverEvents.TryGetValue(name, out var serverEvent) ? serverEvent.TriggerEventAsync() : Task.CompletedTask;
        }
        #endregion

        #region Helper
        private static async Task ExecuteProcessorsAsync(WebRequestResult result, IEnumerable<IWebRequestProcessor> requestProcessors) {
            foreach(var requestProcessor in requestProcessors) {
                if(ShouldExecute(result, requestProcessor) == false)
                    continue;

                await requestProcessor.ProcessAsync(result);
            }
        }

        private static bool ShouldExecute(WebRequestResult result, IWebRequestProcessor requestProcessor) {
            return (result.Handled == false) || requestProcessor.IgnoreHandled;
        }
        #endregion
    }

    public interface IWebServiceEventSubscription {
        Task TriggerAsync();
    }


    public class WebServiceEvent {
        private readonly object _eventLock = new object();
        private readonly List<IWebServiceEventSubscription> _subscriptions = new List<IWebServiceEventSubscription>();
        public string Name { get; }

        public WebServiceEvent(string name) {
            Name = name;
        }

        public void Subscribe(IWebServiceEventSubscription subscription) {
            lock(_eventLock)
                _subscriptions.Add(subscription);
        }

        public async Task TriggerEventAsync() {
            IWebServiceEventSubscription[] actions;
            lock(_eventLock) {
                actions = _subscriptions.ToArray();
                _subscriptions.Clear();
            }
            foreach(var eventSubscription in actions)
                await eventSubscription.TriggerAsync();
        }
    }
}
