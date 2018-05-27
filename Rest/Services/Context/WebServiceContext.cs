// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebServiceContext.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Threading;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Server.Http.Context;
#endregion

namespace DotLogix.Core.Rest.Services.Context {
    public class WebServiceContext {
        private static readonly AsyncLocal<WebServiceContext> AsyncCurrent = new AsyncLocal<WebServiceContext>();
        private readonly Dictionary<string, object> _contextVariables = new Dictionary<string, object>();
        public static WebServiceContext Current => AsyncCurrent.Value;
        public static WebRequestResult CurrentRequestResult => Current.RequestResult;


        public IAsyncHttpContext HttpContext { get; }
        public IAsyncHttpRequest HttpRequest => HttpContext.Request;
        public IAsyncHttpResponse HttpResponse => HttpContext.Response;
        public Guid ContextId { get; } = Guid.NewGuid();
        public WebRequestResult RequestResult { get; }

        #region Indexer
        public object this[string key] {
            get => GetVariable(key);
            set => UpdateVariable(key, value);
        }
        #endregion

        internal WebServiceContext(IAsyncHttpContext httpContext) {
            HttpContext = httpContext;
            RequestResult = new WebRequestResult(httpContext);
        }

        #region Set
        public void UpdateVariable(string key, object value) {
            _contextVariables[key] = value;
        }
        #endregion

        internal static void SetLocalContext(WebServiceContext context) {
            AsyncCurrent.Value = context;
        }

        #region Get
        public object GetVariable(string key) {
            return _contextVariables[key];
        }

        public T GetVariable<T>(string key) {
            return (T)GetVariable(key);
        }

        public bool TryGetVariable(string key, out object value) {
            return _contextVariables.TryGetValue(key, out value);
        }

        public bool TryGetVariable<T>(string key, out T value) {
            if(TryGetVariable(key, out var objectValue) && objectValue is T typedValue) {
                value = typedValue;
                return true;
            }

            value = default(T);
            return false;
        }
        #endregion

        #region GetOrAdd
        public object GetOrAddVariable(string key, object value) {
            if(TryGetVariable(key, out var existing))
                return existing;

            UpdateVariable(key, value);
            return value;
        }

        public T GetOrAddVariable<T>(string key, T value) {
            if(TryGetVariable<T>(key, out var existing))
                return existing;

            UpdateVariable(key, value);
            return value;
        }

        public object GetOrAddVariable(string key, Func<object> valueCreator) {
            if(TryGetVariable(key, out var value))
                return value;

            value = valueCreator.Invoke();
            UpdateVariable(key, value);
            return value;
        }

        public T GetOrAddVariable<T>(string key, Func<T> valueCreator) {
            if(TryGetVariable<T>(key, out var value))
                return value;

            value = valueCreator.Invoke();
            UpdateVariable(key, value);
            return value;
        }
        #endregion
    }
}
