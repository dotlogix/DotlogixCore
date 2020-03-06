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
using System.Threading.Tasks;
using DotLogix.Core.Extensions;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Server.Http.Context;
using DotLogix.Core.Rest.Server.Routes;
using DotLogix.Core.Rest.Services.Descriptors;
using DotLogix.Core.Rest.Services.Processors.Json;
using DotLogix.Core.Utils;
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
        public List<IParameterProvider> ParameterProviders { get; }

        public WebServiceContext(IAsyncHttpContext httpContext, IWebServiceRoute route, List<IParameterProvider> parameterProviders) {
            HttpContext = httpContext;
            Route = route;
            ParameterProviders = parameterProviders;
            RequestResult = new WebRequestResult(httpContext);
            var methodDescriptor = route.RequestProcessor?.Descriptors.GetCustomDescriptor<MethodDescriptor>();
            if(methodDescriptor != null) {
                var returnType = methodDescriptor.DynamicInvoke.ReturnType;
                if(returnType.IsAssignableToOpenGeneric(typeof(Task<>), out var arguments))
                    returnType = arguments[0];
                RequestResult.ReturnType = returnType;
            }
            var settingsDescriptor = route.RequestProcessor?.Descriptors.GetCustomDescriptor<SettingsDescriptor>();
            if (settingsDescriptor != null) {
                Settings = settingsDescriptor.Settings;
            }

            AsyncCurrent.Value = this;

            Variables["webServiceContext"] = this;
            Variables["webServiceRoute"] = route;
            Variables["webServiceResult"] = RequestResult;
            Variables["webServiceSettings"] = Settings;

            Variables["httpContext"] = httpContext;
            Variables["httpResponse"] = httpContext.Response;
            Variables["httpRequest"] = httpContext.Request;
        }

        public IReadOnlySettings Settings { get; }

        public void Dispose() {
            AsyncCurrent.Value = null;
        }
    }
}
