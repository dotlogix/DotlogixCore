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
using DotLogix.Core.Rest.Events;
using DotLogix.Core.Rest.Http;
using DotLogix.Core.Rest.Http.Context;
using DotLogix.Core.Rest.Services.Processors;
using DotLogix.Core.Rest.Services.ResultWriters;

#endregion

namespace DotLogix.Core.Rest.Services.Routing {
    public class WebServiceRouter : IAsyncHttpRequestHandler {
        public WebServiceSettings ServiceSettings { get; }
        public WebServiceRouterSettings Settings => ServiceSettings.Router;

        public WebServiceRouter(WebServiceSettings settings = null) {
            ServiceSettings = settings ?? new WebServiceSettings();
        }

        public const string EventSubscriptionParameterName = "$event";
        public const string EventSubscriptionChannelParameterName = "$eventChannel";

        public WebRequestProcessorCollection PreProcessors => Settings.PreProcessors;
        public WebRequestProcessorCollection PostProcessors => Settings.PostProcessors;
        public ParameterProviderCollection ParameterProviders => Settings.ParameterProviders;
        public WebServiceRouteCollection Routes => Settings.Routes;
        public WebServiceEventCollection Events => Settings.Events;
        public IWebServiceResultWriter DefaultResultWriter {
            get => Settings.DefaultResultWriter;
            set => Settings.DefaultResultWriter = value;
        }


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

            using var webServiceContext = new WebServiceContext(asyncHttpContext, route, parameterProviders, ServiceSettings);
            
            // start of processing
            await ProcessRequest(webServiceContext);
            // end of processing
            if(asyncHttpContext.Response.IsCompleted)
                return;

            var result = webServiceContext.Result;
            var resultWriter = result.ResultWriter
                            ?? route.ResultWriter
                            ?? DefaultResultWriter;

            await resultWriter.WriteAsync(webServiceContext);
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

        private async Task ProcessRequest(WebServiceContext webServiceContext) {
            var route = webServiceContext.Route;

            #region PreProcess
            if(PreProcessors.Count > 0)
                await ExecuteProcessorsAsync(webServiceContext, PreProcessors);

            if(route.PreProcessors.Count > 0)
                await ExecuteProcessorsAsync(webServiceContext, route.PreProcessors);
            #endregion

            if(route.RequestProcessor.ShouldExecute(webServiceContext)) {
                var processingTask = route.RequestProcessor.ProcessAsync(webServiceContext);
                if((processingTask != null) && (processingTask.Status != TaskStatus.RanToCompletion))
                    await processingTask;
            }

            #region PostProcess
            if(PostProcessors.Count > 0)
                await ExecuteProcessorsAsync(webServiceContext, PostProcessors);

            if(route.PostProcessors.Count > 0)
                await ExecuteProcessorsAsync(webServiceContext, route.PostProcessors);
            #endregion
        }
        #endregion

        #region Helper
        private static async ValueTask ExecuteProcessorsAsync(WebServiceContext webServiceContext, IEnumerable<IWebRequestProcessor> requestProcessors) {
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
    }
}
