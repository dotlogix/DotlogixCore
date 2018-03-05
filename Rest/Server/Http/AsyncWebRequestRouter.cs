// ==================================================
// Copyright 2018(C) , DotLogix
// File:  AsyncWebRequestRouter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  05.03.2018
// ==================================================

#region
using System.Collections.Generic;
using System.Threading.Tasks;
using DotLogix.Core.Rest.Server.Http.Context;
using DotLogix.Core.Rest.Server.Http.State;
using DotLogix.Core.Rest.Server.Routes;
using DotLogix.Core.Rest.Services.Context;
using DotLogix.Core.Rest.Services.Processors;
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

        public IWebRequestResultWriter DefaultResultWriter { get; set; } = WebRequestResultJsonWriter.Instance;

        #region Processing
        async Task IAsyncHttpRequestHandler.HandleRequestAsync(IAsyncHttpContext asyncHttpContext) {
            if(TryGetRoute(asyncHttpContext, out var route) == false) {
                await asyncHttpContext.Response.SendAsync(HttpStatusCodes.ClientError.NotFound);
                return;
            }

            if(asyncHttpContext.Request.HeaderParameters.TryGetParameterValue(EventSubscriptionParameterName, out var eventName)) {
                if(ServerEvents.TryGetValue(eventName.ToString(), out var serverEvent) == false) {
                    await asyncHttpContext.Response.WriteToResponseStreamAsync("The event subscription could not be handled, because the event is not registered on the server");
                    await asyncHttpContext.Response.SendAsync(HttpStatusCodes.ClientError.BadRequest);
                    return;
                }
                asyncHttpContext.PreventAutoSend = true;
                serverEvent.Subscribe(asyncHttpContext, route, this);
            } else
                await HandleAsync(asyncHttpContext, route);
        }

        public async Task HandleAsync(IAsyncHttpContext asyncHttpContext, IWebServiceRoute route) {
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
            foreach(var serverRoute in ServerRoutes) {
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
            if(GlobalPreProcessors.Count > 0)
                await ExecuteProcessorsAsync(result, GlobalPreProcessors);

            if(route.PreProcessors.Count > 0)
                await ExecuteProcessorsAsync(result, route.PreProcessors);
            #endregion

            if(ShouldExecute(result, route.RequestProcessor))
                await route.RequestProcessor.ProcessAsync(result);

            #region PostProcess
            if(GlobalPostProcessors.Count > 0)
                await ExecuteProcessorsAsync(result, GlobalPostProcessors);

            if(route.PostProcessors.Count > 0)
                await ExecuteProcessorsAsync(result, route.PostProcessors);
            #endregion
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
}
