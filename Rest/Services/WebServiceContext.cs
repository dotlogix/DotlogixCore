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
using DotLogix.Core.Rest.Http;
using DotLogix.Core.Rest.Http.Context;
using DotLogix.Core.Rest.Http.Headers;
using DotLogix.Core.Rest.Services.Descriptors;
using DotLogix.Core.Rest.Services.Parameters;
using DotLogix.Core.Rest.Services.Processors;
using DotLogix.Core.Rest.Services.Routing;
using DotLogix.Core.Utils;
#endregion

namespace DotLogix.Core.Rest.Services {
    public class WebServiceContext : IDisposable {
        private static readonly AsyncLocal<WebServiceContext> AsyncCurrent = new AsyncLocal<WebServiceContext>();
        public IDictionary<string, object> Variables { get; } = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        public static WebServiceContext Current => AsyncCurrent.Value;

        public Guid ContextId { get; } = Guid.NewGuid();
        public IAsyncHttpContext HttpContext { get; }
        public IAsyncHttpRequest HttpRequest => HttpContext.Request;
        public IAsyncHttpResponse HttpResponse => HttpContext.Response;
        public IWebServiceResult Result { get; private set; }
        public IWebServiceRoute Route { get; }
        public List<IParameterProvider> ParameterProviders { get; }

        public WebServiceContext(IAsyncHttpContext httpContext, IWebServiceRoute route, List<IParameterProvider> parameterProviders) {
            HttpContext = httpContext;
            Route = route;
            ParameterProviders = parameterProviders;
            Result = null;
            var settingsDescriptor = route.Descriptors.GetCustomDescriptor<SettingsDescriptor>();
            if (settingsDescriptor != null) {
                Settings = settingsDescriptor.Settings;
            }

            AsyncCurrent.Value = this;

            Variables["webServiceContext"] = this;
            Variables["webServiceRoute"] = route;
            Variables["webServiceResult"] = Result;
            Variables["webServiceSettings"] = Settings;

            Variables["httpContext"] = httpContext;
            Variables["httpResponse"] = httpContext.Response;
            Variables["httpRequest"] = httpContext.Request;
        }

        public IReadOnlySettings Settings { get; }


        public void SetResult(object result, HttpStatusCode statusCode = null, MimeType contentType = null) {
            switch(result) {
                case IWebServiceResult webRequestResult:
                    SetResult(webRequestResult);
                    break;
                case Exception exception:
                    SetException(exception, statusCode);
                    break;
                default:
                    var objectResult = new WebServiceObjectResult {
                                                                  StatusCode = statusCode,
                                                                  ContentType = contentType,
                                                                  ReturnValue = result
                                                                  };
                    SetResult(objectResult);
                    break;
            }
        }

        public void SetResult(IWebServiceResult result) {
            Result = result;
        }

        public void SetException(Exception exception, HttpStatusCode statusCode = null) {
            Result = new WebServiceResult(statusCode){Exception = exception};
        }

        public bool TrySetResult(object result, HttpStatusCode statusCode = null, MimeType contentType = null) {
            if(Result != null)
                return false;
            SetResult(result, statusCode, contentType);
            return true;
        }

        public bool TrySetException(Exception exception, HttpStatusCode statusCode = null) {
            if(Result != null)
                return false;
            SetException(exception, statusCode);
            return true;
        }


        public void Dispose() {
            AsyncCurrent.Value = null;
        }
    }
}
