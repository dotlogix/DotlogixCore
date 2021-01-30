// ==================================================
// Copyright 2018(C) , DotLogix
// File:  AsyncHttpServer.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Core.Diagnostics;
using DotLogix.Core.Extensions;
using DotLogix.Core.Rest.Http;
using DotLogix.Core.Rest.Http.Context;
using DotLogix.Core.Rest.Http.Parameters;
using DotLogix.Core.Rest.WebSockets;
using HttpStatusCode = DotLogix.Core.Rest.Http.HttpStatusCode;
#endregion

namespace DotLogix.Core.Rest {
    public class AsyncWebServer : IAsyncWebServer {
        private const string ServerDisposedMessage = "Server has been disposed already";

        private static readonly Dictionary<string, HttpMethods> CachedMethods = Enum.GetValues(typeof(HttpMethods))
                                                                                     .Cast<HttpMethods>()
                                                                                     .ToDictionary(m => m.ToString(), StringComparer.OrdinalIgnoreCase);

        private readonly HttpListener _httpListener = new HttpListener();
        private readonly IAsyncHttpRequestHandler _requestHandler;
        private readonly IAsyncWebSocketRequestHandler _webSocketRequestHandler;
        private SemaphoreSlim _requestSemaphore;
        private CancellationTokenSource _serverShutdownSource;
        
        public AsyncWebServer(IAsyncHttpRequestHandler requestHandler, IAsyncWebSocketRequestHandler webSocketRequestHandler, WebServerSettings settings = null) {
            Settings = settings ?? new WebServerSettings();
            _requestHandler = requestHandler;
            _webSocketRequestHandler = webSocketRequestHandler;

            if(Settings.UrlPrefixes != null) {
                _httpListener.Prefixes.AddRange(Settings.UrlPrefixes);
            }
        }

        public AsyncWebServer(IAsyncHttpRequestHandler requestHandler, WebServerSettings settings = null) : this(requestHandler, null, settings) {
        }
        public AsyncWebServer(IAsyncWebSocketRequestHandler webSocketRequestHandler, WebServerSettings settings = null) : this(null, webSocketRequestHandler, settings) {
        }

        public WebServerSettings Settings { get; }


        public bool IsRunning { get; private set; }
        public bool IsDisposed { get; private set; }

        public void Start() {
            if(IsDisposed)
                throw new ObjectDisposedException(nameof(AsyncWebServer), ServerDisposedMessage);
            if(IsRunning)
                return;
            IsRunning = true;
            _serverShutdownSource = new CancellationTokenSource();
            var requestLimit = Settings.EnableConcurrentRequests? Settings.RequestLimit : 1;
            _requestSemaphore = new SemaphoreSlim(0, requestLimit);

            _httpListener.Prefixes.Clear();
            _httpListener.Prefixes.AddRange(Settings.UrlPrefixes);
            _httpListener.Start();

            _requestSemaphore.Release(requestLimit);
            HandleRequestsAsync();
        }

        public void Stop() {
            if(IsDisposed)
                throw new ObjectDisposedException(nameof(AsyncWebServer), ServerDisposedMessage);
            if(IsRunning == false)
                return;
            IsRunning = false;
            _httpListener.Stop();

            _serverShutdownSource.Cancel();
            _serverShutdownSource.Dispose();
            _serverShutdownSource = null;

            _requestSemaphore.Dispose();
            _requestSemaphore = null;
        }

        public void AddServerPrefix(string uriPrefix) {
            if(IsRunning)
                throw new InvalidOperationException("Can not add prefix while the server is running");
            if(IsDisposed)
                throw new ObjectDisposedException(nameof(AsyncWebServer), ServerDisposedMessage);

            Settings.UrlPrefixes.Add(uriPrefix);
            _httpListener.Prefixes.Add(uriPrefix);
        }

        public void Dispose() {
            if(IsDisposed)
                return;
            Stop();
            _httpListener.Close();
            IsDisposed = true;
        }

        private async void HandleRequestsAsync() {
            void ReleaseSemaphore(Task task) {
                try {
                    _requestSemaphore.Release();
                } catch (Exception e) {
                    if (IsRunning)
                        Settings.LogSource.Error(e);
                }
            }
            
            while (IsRunning) {
                try {
                    await _requestSemaphore.WaitAsync(_serverShutdownSource.Token);
                } catch(OperationCanceledException e) {
                    if(IsRunning == false)
                        continue; // Suppress cancellation because of server shutdown
                    Settings.LogSource.Error(e);
                }

                try {
                    var context = await _httpListener.GetContextAsync().ConfigureAwait(false);
                    if(IsSupported(context) == false) {
                        SendRequestTypeNotSupported(context);
                        return;
                    }



                    var task = context.Request.IsWebSocketRequest
                               ? HandleWebSocketRequestAsync(context)
                               : HandleRequestAsync(context);

#pragma warning disable 4014
                    task.ContinueWith(ReleaseSemaphore)
                        .ConfigureAwait(false);
#pragma warning restore 4014

                } catch(HttpListenerException e) {
                    if(IsRunning == false)
                        continue;
                    Settings.LogSource.Error(e);
                }
            }
        }

        private async Task HandleWebSocketRequestAsync(HttpListenerContext originalContext) {
            IAsyncWebSocketRequest webSocketRequest = null;

            try {
                string selectedProtocol = null;

                var requestedProtocols = originalContext.Request.Headers.GetValues("Sec-WebSocket-Protocol");
                var supportedSubProtocols = _webSocketRequestHandler.SupportedSubProtocols;
                if(requestedProtocols == null || requestedProtocols.Length == 0) {
                    selectedProtocol = supportedSubProtocols.FirstOrDefault();
                } else {
                    selectedProtocol = supportedSubProtocols.FirstOrDefault(supportedSubProtocols.Contains);
                    if(selectedProtocol == null) {
                        SendProtocolNotSupported(originalContext);
                        return;
                    }
                }

                var originalWebSocketContext = await originalContext.AcceptWebSocketAsync(selectedProtocol);
                webSocketRequest = CreateContext(originalWebSocketContext);

                await _webSocketRequestHandler.HandleRequestAsync(webSocketRequest);
            } catch(Exception e) {
                if (IsRunning)
                    Settings.LogSource.Error(e);
                try {
                    webSocketRequest?.Dispose();
                } catch(Exception ex) {
                    if(IsRunning)
                        Settings.LogSource.Error(ex);
                }
            }
        }

        private async Task HandleRequestAsync(HttpListenerContext originalContext) {
            IAsyncHttpContext httpContext = null;

            try {
                httpContext = CreateContext(originalContext);
                var response = httpContext.Response;

                await _requestHandler.HandleRequestAsync(httpContext);
                if((response.IsCompleted == false) && (httpContext.PreventAutoSend == false))
                    await response.CompleteAsync();
            } catch(Exception e) {
                Settings.LogSource.Error(e);
                if(httpContext != null)
                    await SendErrorMessageAsync(httpContext.Response, e);
            } finally {
                httpContext?.Dispose();
            }
        }

        #region Create

        private AsyncHttpContext CreateContext(HttpListenerContext originalContext) {
            var request = CreateRequest(originalContext);
            var response = CreateResponse(originalContext);
            return new AsyncHttpContext(this, request, response);
        }
        private AsyncWebSocketContext CreateContext(HttpListenerWebSocketContext originalContext) {
            return AsyncWebSocketContext.Create(originalContext, Settings.ParameterParser ?? PrimitiveParameterParser.Default);
        }
        private AsyncHttpResponse CreateResponse(HttpListenerContext originalContext) {
            return AsyncHttpResponse.Create(originalContext.Response, Settings.ParameterParser ?? PrimitiveParameterParser.Default);
        }
        private AsyncHttpRequest CreateRequest(HttpListenerContext originalContext) {
            return AsyncHttpRequest.Create(originalContext.Request, Settings.ParameterParser ?? PrimitiveParameterParser.Default);
        }

        #endregion

        #region Send

        private static void SendBadRequest(HttpListenerContext context, string message = null) {
            var badRequest = HttpStatusCodes.ClientError.BadRequest;
            context.Response.StatusCode = badRequest.Code;
            context.Response.StatusDescription = message == null
                                                 ? badRequest.Description
                                                 : $"{badRequest.Description} ({message})";
            context.Response.Close();
        }
        private static void SendRequestTypeNotSupported(HttpListenerContext context) {
            SendBadRequest(context, "Request mode not supported");
        }
        private static void SendProtocolNotSupported(HttpListenerContext context) {
            SendBadRequest(context, "Protocol not supported");
        }

        public async Task SendErrorMessageAsync(IAsyncHttpResponse response, Exception exception) {
            var stringBuilder = new StringBuilder();
            var httpStatusCode = HttpStatusCodes.ServerError.InternalServerError;
            CreateExceptionMessage(stringBuilder, exception, ref httpStatusCode);

            response.StatusCode = httpStatusCode;
            response.ContentType = MimeTypes.Text.Plain;
            response.OutputStream.SetLength(0);
            try {
                await response.WriteToResponseStreamAsync(stringBuilder.ToString());
                await response.CompleteAsync();
            } catch(Exception e) {
                Settings.LogSource.Error(e);
            }
        }
        #endregion

        #region Helper

        private bool IsSupported(HttpListenerContext context) {
            if(context.Request.IsWebSocketRequest)
                return _webSocketRequestHandler != null;
            return _requestHandler != null;
        }

        private static void CreateExceptionMessage(StringBuilder builder, Exception exception, ref HttpStatusCode statusCode) {
            if(exception is RestException re)
                statusCode = re.ErrorCode;

            builder.Append("ExceptionType: ");
            builder.Append(exception.GetType()
                                    .GetFriendlyName());
            builder.Append("Message: ");
            builder.AppendLine(exception.Message);
            builder.AppendLine("Stacktrace:");
            builder.AppendLine(exception.StackTrace);

            if(exception is AggregateException ag) {
                builder.AppendLine("InnerExceptions:");
                foreach(var innerException in ag.InnerExceptions)
                    CreateExceptionMessage(builder, innerException, ref statusCode);
            } else {
                var innerException = exception.InnerException;
                if(innerException != null) {
                    builder.AppendLine("InnerException:");
                    CreateExceptionMessage(builder, innerException, ref statusCode);
                }
            }
        }

        public static HttpMethods HttpMethodFromString(string originalRequestHttpMethod) {
            return CachedMethods.TryGetValue(originalRequestHttpMethod, out var httpMethod)
                   ? httpMethod
                   : HttpMethods.None;
        }

        #endregion
    }
}
