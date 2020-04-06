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
using DotLogix.Core.Rest.Server.Http.Headers;
using DotLogix.Core.Rest.Server.Http.State;
using DotLogix.Core.Rest.Server.Routes;
using DotLogix.Core.Rest.Services.Descriptors;
using DotLogix.Core.Rest.Services.Processors.Json;
using DotLogix.Core.Utils;
#endregion

namespace DotLogix.Core.Rest.Services.Context {
    public class WebRequestContext : IDisposable {
        private static readonly AsyncLocal<WebRequestContext> AsyncCurrent = new AsyncLocal<WebRequestContext>();
        public IDictionary<string, object> Variables { get; } = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        public static WebRequestContext Current => AsyncCurrent.Value;

        public Guid ContextId { get; } = Guid.NewGuid();
        public IAsyncHttpContext HttpContext { get; }
        public IAsyncHttpRequest HttpRequest => HttpContext.Request;
        public IAsyncHttpResponse HttpResponse => HttpContext.Response;
        public IWebRequestResult RequestResult { get; private set; }
        public IWebServiceRoute Route { get; }
        public List<IParameterProvider> ParameterProviders { get; }

        public WebRequestContext(IAsyncHttpContext httpContext, IWebServiceRoute route, List<IParameterProvider> parameterProviders) {
            HttpContext = httpContext;
            Route = route;
            ParameterProviders = parameterProviders;
            RequestResult = null;
            var settingsDescriptor = route.Descriptors.GetCustomDescriptor<SettingsDescriptor>();
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


        public void SetResult(object result, HttpStatusCode statusCode = null, MimeType contentType = null) {
            switch(result) {
                case IWebRequestResult webRequestResult:
                    SetResult(webRequestResult);
                    break;
                case Exception exception:
                    SetException(exception, statusCode);
                    break;
                default:
                    SetResult(new WebRequestObjectResult(statusCode, contentType){ReturnValue=result});
                    break;
            }
        }

        public void SetResult(IWebRequestResult result) {
            RequestResult = result;
        }

        public void SetException(Exception exception, HttpStatusCode statusCode = null) {
            RequestResult = new WebRequestResultBase(statusCode){Exception = exception};
        }

        public bool TrySetResult(object result, HttpStatusCode statusCode = null, MimeType contentType = null) {
            if(RequestResult != null)
                return false;
            SetResult(result, statusCode, contentType);
            return true;
        }

        public bool TrySetException(Exception exception, HttpStatusCode statusCode = null) {
            if(RequestResult != null)
                return false;
            SetException(exception, statusCode);
            return true;
        }


        public void Dispose() {
            AsyncCurrent.Value = null;
        }
    }
}
