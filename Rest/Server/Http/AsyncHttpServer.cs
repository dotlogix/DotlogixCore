// ==================================================
// Copyright 2018(C) , DotLogix
// File:  AsyncHttpServer.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  29.01.2018
// LastEdited:  31.01.2018
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
        private SemaphoreSlim _connectionSemaphore;
        private SemaphoreSlim _requestSemaphore;
        private CancellationTokenSource _serverShutdownSource;
        public bool IsDisposed { get; private set; }

        public AsyncHttpServer(IAsyncHttpRequestHandler requestHandler, ConcurrencyOptions concurrencyOptions = null) {
            ConcurrencyOptions = concurrencyOptions ?? ConcurrencyOptions.Default;
            _requestHandler = requestHandler ?? throw new ArgumentNullException(nameof(requestHandler));
        }

        public ConcurrencyOptions ConcurrencyOptions { get; }
        public bool IsRunning { get; private set; }

        public void Start() {
            if(IsDisposed)
                throw new ObjectDisposedException(nameof(AsyncHttpServer), ServerDisposedMessage);
            if(IsRunning)
                return;
            IsRunning = true;
            _serverShutdownSource = new CancellationTokenSource();
            _connectionSemaphore = new SemaphoreSlim(0, ConcurrencyOptions.Connections);
            _requestSemaphore = new SemaphoreSlim(0, ConcurrencyOptions.Requests);
            _httpListener.Start();

            _connectionSemaphore.Release(ConcurrencyOptions.Connections);
            _requestSemaphore.Release(ConcurrencyOptions.Requests);
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

            _connectionSemaphore.Dispose();
            _requestSemaphore.Dispose();
            _connectionSemaphore = null;
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
            IsDisposed = true;
            Stop();
            _httpListener.Close();
        }

        private async void HandleRequestsAsync() {
            while(IsRunning) {
                try {
                    await _connectionSemaphore.WaitAsync(_serverShutdownSource.Token);
                } catch(OperationCanceledException e) {
                    if(IsRunning == false)
                        continue; // Supress cancellation because of server shutdown
                    Log.Error(e);
                }

                try {
#pragma warning disable 4014
                    var contextTask = _httpListener.GetContextAsync();
                    contextTask.ContinueWith(HandleRequestAsync, TaskContinuationOptions.OnlyOnRanToCompletion);
                    contextTask.ContinueWith(t => _connectionSemaphore.Release());
#pragma warning restore 4014
                } catch(HttpListenerException e) {
                    if(IsRunning == false)
                        continue;
                    Log.Error(e);
                }
            }
        }

        private async Task HandleRequestAsync(Task<HttpListenerContext> context) {
            try {
                await _requestSemaphore.WaitAsync(_serverShutdownSource.Token);
            } catch(OperationCanceledException e) {
                if(IsRunning == false)
                    return; // Supress cancellation because of server shutdown
                Log.Error(e);
            }

            var httpListenerContext = await context;
            var request = new AsyncHttpRequest(httpListenerContext.Request);
            var response = new AsyncHttpResponse(httpListenerContext.Response);
            var httpContext = new AsyncHttpContext(this, request, response);

            try {
                await _requestHandler.HandleRequestAsync(httpContext);
                if(response.IsSent == false && httpContext.PreventAutoSend == false) {
                    await response.SendAsync();
                }
            } catch(Exception e) {
                Log.Error(e);
                await OnError(response, e);
            } finally {
                try {
                    _requestSemaphore.Release();
                } catch(Exception e) {
                    if(IsRunning)
                        Log.Error(e);
                }
            }
        }

        private async Task OnError(IAsyncHttpResponse response, Exception exception) {
            await SendErrorMessageAsync(response, exception);
        }

        public static async Task SendErrorMessageAsync(IAsyncHttpResponse response, Exception exception) {
            var stringBuilder = new StringBuilder();
            var httpStatusCode = HttpStatusCodes.InternalServerError;
            CreateExceptionMessage(stringBuilder, exception, ref httpStatusCode);

            response.StatusCode = httpStatusCode;
            response.ContentType = MimeTypes.PlainText;
            response.OutputStream.SetLength(0);
            try {
                await response.WriteToResponseStreamAsync(stringBuilder.ToString());
                await response.SendAsync();
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
