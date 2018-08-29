// ==================================================
// Copyright 2018(C) , DotLogix
// File:  AsyncWebRequestRouter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  16.07.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
using System.Threading.Tasks;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes;
using DotLogix.Core.Rest.Server.Http.Context;
using DotLogix.Core.Rest.Server.Http.State;
using DotLogix.Core.Rest.Server.Routes;
using DotLogix.Core.Rest.Services.Context;
using DotLogix.Core.Rest.Services.Processors;
using DotLogix.Core.Rest.Services.Writer;
#endregion

namespace DotLogix.Core.Rest.Server.Http {
    public class AsyncWebRequestRouter : IAsyncHttpRequestHandler {
        public const string EventSubscriptionParameterName = "$event";
        public const string EventSubscriptionChannelParameterName = "$eventChannel";

        public WebRequestProcessorCollection GlobalPreProcessors { get; } = new WebRequestProcessorCollection();
        public WebRequestProcessorCollection GlobalPostProcessors { get; } = new WebRequestProcessorCollection();
        public WebServiceRouteCollection ServerRoutes { get; } = new WebServiceRouteCollection();
        public WebServiceEventCollection ServerEvents { get; } = new WebServiceEventCollection();


        public int RegisteredRoutesCount => ServerRoutes.Count;

        public IAsyncWebRequestResultWriter DefaultResultWriter { get; set; } = WebRequestResultJsonWriter.Instance;

        #region Helper
        private static async Task ExecuteProcessorsAsync(WebServiceContext webServiceContext, IEnumerable<IWebRequestProcessor> requestProcessors) {
            foreach(var requestProcessor in requestProcessors) {
                if(requestProcessor.ShouldExecute(webServiceContext) == false)
                    continue;

                var processingTask = requestProcessor.ProcessAsync(webServiceContext);
                if((processingTask == null) || (processingTask.Status == TaskStatus.RanToCompletion))
                    continue;
                await processingTask;
            }
        }
        #endregion

        #region Processing
        async Task IAsyncHttpRequestHandler.HandleRequestAsync(IAsyncHttpContext asyncHttpContext) {
            if(TryGetRoute(asyncHttpContext, out var route) == false) {
                asyncHttpContext.Response.StatusCode = HttpStatusCodes.ClientError.NotFound;
                await asyncHttpContext.Response.CompleteAsync();
                return;
            }
            if(asyncHttpContext.Request.HeaderParameters.TryGetValue(EventSubscriptionParameterName, out string eventName)) {
                if(ServerEvents.TryGetValue(eventName, out var serverEvent) == false) {
                    await asyncHttpContext.Response.WriteToResponseStreamAsync("The event subscription could not be handled, because the event is not registered on the server");
                    asyncHttpContext.Response.StatusCode = HttpStatusCodes.ClientError.BadRequest;
                    await asyncHttpContext.Response.CompleteAsync();
                    return;
                }
                asyncHttpContext.PreventAutoSend = true;
                serverEvent.Subscribe(asyncHttpContext, route, this);
            } else
                await HandleAsync(asyncHttpContext, route);
        }

        public async Task HandleAsync(IAsyncHttpContext asyncHttpContext, IWebServiceRoute route) {
            using(var webServiceContext = new WebServiceContext(asyncHttpContext, route)) {
                // start of processing
                await ProcessRequest(webServiceContext);
                // end of processing

                var writer = route.WebRequestResultWriter ?? DefaultResultWriter;
                if(asyncHttpContext.Response.IsCompleted == false)
                    await writer.WriteAsync(webServiceContext.RequestResult);
            }
        }

        private bool TryGetRoute(IAsyncHttpContext asyncHttpContext, out IWebServiceRoute route) {
            var asyncHttpRequest = asyncHttpContext.Request;

            var httpMethod = asyncHttpRequest.HttpMethod;
            var path = asyncHttpRequest.Url.LocalPath;

            route = null;
            RouteMatch routeMatch = null;
            var routePriority = int.MinValue;
            var matchLength = int.MinValue;
            foreach(var serverRoute in ServerRoutes) {
                if((serverRoute.AcceptedRequests & httpMethod) == 0) // route does not accept the type of the request
                    continue;

                if(serverRoute.Priority < routePriority) // another route has a higher priority
                    continue;

                var match = serverRoute.Match(httpMethod, path);
                if(match.Success == false) // match is not successful
                    continue;

                if(match.Length < matchLength) // another route better matches the path
                    continue;

                route = serverRoute;
                routeMatch = match;
                routePriority = serverRoute.Priority;
                matchLength = match.Length;
            }

            if(routeMatch == null)
                return false;

            if(routeMatch.UrlParameters == null)
                return true;

            foreach(var parameter in routeMatch.UrlParameters)
                asyncHttpRequest.UrlParameters.Add(parameter.Key, parameter.Value);

            return true;
        }

        private async Task ProcessRequest(WebServiceContext webServiceContext) {
            var route = webServiceContext.Route;

            #region PreProcess
            if(GlobalPreProcessors.Count > 0)
                await ExecuteProcessorsAsync(webServiceContext, GlobalPreProcessors);

            if(route.PreProcessors.Count > 0)
                await ExecuteProcessorsAsync(webServiceContext, route.PreProcessors);
            #endregion

            if(route.RequestProcessor.ShouldExecute(webServiceContext)) {
                var processingTask = route.RequestProcessor.ProcessAsync(webServiceContext);
                if((processingTask != null) && (processingTask.Status != TaskStatus.RanToCompletion))
                    await processingTask;
            }

            #region PostProcess
            if(GlobalPostProcessors.Count > 0)
                await ExecuteProcessorsAsync(webServiceContext, GlobalPostProcessors);

            if(route.PostProcessors.Count > 0)
                await ExecuteProcessorsAsync(webServiceContext, route.PostProcessors);
            #endregion
        }
        #endregion
    }
}
