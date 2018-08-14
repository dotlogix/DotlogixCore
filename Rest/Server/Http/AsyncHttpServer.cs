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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Core.Diagnostics;
using DotLogix.Core.Extensions;
using DotLogix.Core.Rest.Server.Http.Context;
using DotLogix.Core.Rest.Server.Http.Mime;
using DotLogix.Core.Rest.Server.Http.State;
using DotLogix.Core.Rest.Services.Exceptions;
using HttpStatusCode = DotLogix.Core.Rest.Server.Http.State.HttpStatusCode;
#endregion

namespace DotLogix.Core.Rest.Server.Http {
    public class AsyncHttpServer : IAsyncHttpServer {
        private const string ServerDisposedMessage = "Server has been disposed already";
        private static readonly Dictionary<string, HttpMethods> ChachedMethods = Enum.GetValues(typeof(HttpMethods)).Cast<HttpMethods>().ToDictionary(m => m.ToString(), StringComparer.OrdinalIgnoreCase);
        private readonly HttpListener _httpListener = new HttpListener();
        private readonly IAsyncHttpRequestHandler _requestHandler;
        private SemaphoreSlim _requestSemaphore;
        private CancellationTokenSource _serverShutdownSource;
        public bool IsDisposed { get; private set; }

        public AsyncHttpServer(IAsyncHttpRequestHandler requestHandler, Configuration configuration = null) {
            Configuration = configuration ?? Configuration.Default;
            _requestHandler = requestHandler ?? throw new ArgumentNullException(nameof(requestHandler));
        }

        public Configuration Configuration { get; }
        public bool IsRunning { get; private set; }

        public void Start() {
            if(IsDisposed)
                throw new ObjectDisposedException(nameof(AsyncHttpServer), ServerDisposedMessage);
            if(IsRunning)
                return;
            IsRunning = true;
            _serverShutdownSource = new CancellationTokenSource();
            _requestSemaphore = new SemaphoreSlim(0, Configuration.MaxConcurrentRequests);
            _httpListener.Start();

            _requestSemaphore.Release(Configuration.MaxConcurrentRequests);
            HandleRequestsAsync();
        }

        public void Stop() {
            if(IsDisposed)
                throw new ObjectDisposedException(nameof(AsyncHttpServer), ServerDisposedMessage);
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
                throw new ObjectDisposedException(nameof(AsyncHttpServer), ServerDisposedMessage);
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
            while(IsRunning) {
                try {
                    await _requestSemaphore.WaitAsync(_serverShutdownSource.Token);
                } catch(OperationCanceledException e) {
                    if(IsRunning == false)
                        continue; // Supress cancellation because of server shutdown
                    Log.Error(e);
                }

                try {
                    var context = await _httpListener.GetContextAsync().ConfigureAwait(false);
                    HandleRequestAsync(context).ConfigureAwait(false);
                } catch(HttpListenerException e) {
                    if(IsRunning == false)
                        continue;
                    Log.Error(e);
                }
            }
        }

        private async Task HandleRequestAsync(HttpListenerContext originalContext) {
            IAsyncHttpContext httpContext;
            try {
                httpContext = CreateContext(originalContext);
            } catch(Exception e) {
                if(IsRunning)
                    Log.Error(e);
                return;
            }

            var response = httpContext.Response;
            try {
                await _requestHandler.HandleRequestAsync(httpContext);
                if((response.IsCompleted == false) && (httpContext.PreventAutoSend == false))
                    await response.CompleteAsync();
            } catch(Exception e) {
                Log.Error(e);
                await SendErrorMessageAsync(response, e);
            } finally {
                httpContext.Dispose();
                try {
                    _requestSemaphore.Release();
                } catch(Exception e) {
                    if(IsRunning)
                        Log.Error(e);
                }
            }
        }

        private IAsyncHttpContext CreateContext(HttpListenerContext originalContext) {
            var request = CreateRequest(originalContext);
            var response = CreateResponse(originalContext);
            return new AsyncHttpContext(this, request, response);
        }

        private AsyncHttpResponse CreateResponse(HttpListenerContext originalContext) {
            return AsyncHttpResponse.Create(originalContext.Response, Configuration.ParameterParser ?? PrimitiveParameterParser.Instance);
        }

        private AsyncHttpRequest CreateRequest(HttpListenerContext originalContext) {
            return AsyncHttpRequest.Create(originalContext.Request, Configuration.ParameterParser ?? PrimitiveParameterParser.Instance);
        }

        public static async Task SendErrorMessageAsync(IAsyncHttpResponse response, Exception exception) {
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
                Log.Error(e);
            }
        }

        private static void CreateExceptionMessage(StringBuilder builder, Exception exception, ref HttpStatusCode statusCode) {
            if(exception is RestException re)
                statusCode = re.ErrorCode;

            builder.Append("ExceptionType: ");
            builder.Append(exception.GetType().GetFriendlyName());
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
            return ChachedMethods.TryGetValue(originalRequestHttpMethod, out var httpMethod) ? httpMethod : HttpMethods.None;
        }
    }
}
