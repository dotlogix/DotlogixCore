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
using DotLogix.Core.Rest.Server.Http.Context;
using DotLogix.Core.Rest.Server.Http.State;
using DotLogix.Core.Rest.Server.Routes;
using DotLogix.Core.Rest.Services.Context;
using DotLogix.Core.Rest.Services.Processors;
using DotLogix.Core.Rest.Services.Processors.Json;
using DotLogix.Core.Rest.Services.Writer;
#endregion

namespace DotLogix.Core.Rest.Server.Http {
    public class AsyncWebRequestRouter : IAsyncHttpRequestHandler {
        public const string EventSubscriptionParameterName = "$event";
        public const string EventSubscriptionChannelParameterName = "$eventChannel";

        public WebRequestProcessorCollection PreProcessors { get; } = new WebRequestProcessorCollection();
        public WebRequestProcessorCollection PostProcessors { get; } = new WebRequestProcessorCollection();
        public ParameterProviderCollection ParameterProviders { get; } = new ParameterProviderCollection(Services.Processors.Json.ParameterProviders.Context);
        public WebServiceRouteCollection Routes { get; } = new WebServiceRouteCollection();
        public WebServiceEventCollection Events { get; } = new WebServiceEventCollection();
        public IAsyncWebRequestResultWriter DefaultResultWriter { get; set; } = JsonNodesWebRequestResultWriter.Instance;


        #region Processing
        public async Task HandleRequestAsync(IAsyncHttpContext httpContext) {
            if (TryGetRoute(httpContext, out var route) == false) {
                httpContext.Response.StatusCode = HttpStatusCodes.ClientError.NotFound;
                await httpContext.Response.CompleteAsync();
                return;
            }
            if(httpContext.Request.HeaderParameters.TryGetValueAs(EventSubscriptionParameterName, out string eventName)) {
                if(Events.TryGet(eventName, out var serverEvent) == false) {
                    await httpContext.Response.WriteToResponseStreamAsync("The event subscription could not be handled, because the event is not registered on the server");
                    httpContext.Response.StatusCode = HttpStatusCodes.ClientError.BadRequest;
                    await httpContext.Response.CompleteAsync();
                    return;
                }
                httpContext.PreventAutoSend = true;
                serverEvent.Subscribe(httpContext, route, this);
            } else
                await HandleAsync(httpContext, route);
        }

        public async Task HandleAsync(IAsyncHttpContext asyncHttpContext, IWebServiceRoute route) {
            var routeParameterProviders = route.ParameterProviders;
            var globalParameterProviders = ParameterProviders;

            var initialProviderCount = globalParameterProviders.Count + routeParameterProviders.Count;
            var parameterProviders = new ParameterProviderCollection(initialProviderCount);
            parameterProviders.AddRange(globalParameterProviders);
            parameterProviders.AddRange(routeParameterProviders);


            using (var webServiceContext = new WebRequestContext(asyncHttpContext, route, parameterProviders)) {
                // start of processing
                await ProcessRequest(webServiceContext);
                // end of processing

                var result = webServiceContext.RequestResult;
                var resultWriter = result.ResultWriter
                                   ?? route.WebRequestResultWriter
                                   ?? DefaultResultWriter;

                if(asyncHttpContext.Response.IsCompleted == false)
                    await resultWriter.WriteAsync(webServiceContext);
            }
        }

        private bool TryGetRoute(IAsyncHttpContext asyncHttpContext, out IWebServiceRoute route) {
            var asyncHttpRequest = asyncHttpContext.Request;

            var httpMethod = asyncHttpRequest.HttpMethod;
            var path = asyncHttpRequest.Url.LocalPath;

            var routeMatch = Routes.FindBestMatch(httpMethod, path, out route);
            if(routeMatch == null)
                return false;

            if(routeMatch.UrlParameters == null)
                return true;

            foreach(var parameter in routeMatch.UrlParameters)
                asyncHttpRequest.UrlParameters.Add(parameter.Key, parameter.Value);

            return true;
        }

        private async Task ProcessRequest(WebRequestContext webRequestContext) {
            var route = webRequestContext.Route;

            #region PreProcess
            if(PreProcessors.Count > 0)
                await ExecuteProcessorsAsync(webRequestContext, PreProcessors);

            if(route.PreProcessors.Count > 0)
                await ExecuteProcessorsAsync(webRequestContext, route.PreProcessors);
            #endregion

            if(route.RequestProcessor.ShouldExecute(webRequestContext)) {
                var processingTask = route.RequestProcessor.ProcessAsync(webRequestContext);
                if((processingTask != null) && (processingTask.Status != TaskStatus.RanToCompletion))
                    await processingTask;
            }

            #region PostProcess
            if(PostProcessors.Count > 0)
                await ExecuteProcessorsAsync(webRequestContext, PostProcessors);

            if(route.PostProcessors.Count > 0)
                await ExecuteProcessorsAsync(webRequestContext, route.PostProcessors);
            #endregion
        }
        #endregion

        #region Helper
        private static async ValueTask ExecuteProcessorsAsync(WebRequestContext webRequestContext, IEnumerable<IWebRequestProcessor> requestProcessors) {
            foreach(var requestProcessor in requestProcessors) {
                if(requestProcessor.ShouldExecute(webRequestContext) == false)
                    continue;

                var processingTask = requestProcessor.ProcessAsync(webRequestContext);
                if((processingTask == null) || (processingTask.Status == TaskStatus.RanToCompletion))
                    continue;
                await processingTask;
            }
        }
        #endregion
    }
}
