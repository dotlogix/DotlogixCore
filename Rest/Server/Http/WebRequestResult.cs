// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebRequestResult.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Text;
using System.Threading.Tasks;
using DotLogix.Core.Nodes;
using DotLogix.Core.Rest.Server.Http.Headers;
using DotLogix.Core.Rest.Server.Http.State;
using DotLogix.Core.Rest.Services.Attributes.ResultWriter;
using DotLogix.Core.Rest.Services.Context;
using DotLogix.Core.Rest.Services.Exceptions;
using DotLogix.Core.Rest.Services.Writer;
using DotLogix.Core.Tracking.Snapshots;
#endregion

namespace DotLogix.Core.Rest.Server.Http {
    public static class FluentWebRequestResult {
        public static T WithStatusCode<T>(this T result, HttpStatusCode statusCode) where T : WebRequestResultBase {
            result.StatusCode = statusCode;
            return result;
        }

        public static T WithContentType<T>(this T result, MimeType contentType) where T : WebRequestResultBase {
            result.ContentType = contentType;
            return result;
        }

        public static T WithException<T>(this T result, Exception exception) where T : WebRequestResultBase {
            result.Exception = exception;
            return result;
        }

        public static T WithWriter<T>(this T result, IAsyncWebRequestResultWriter resultWriter) where T : WebRequestResultBase {
            result.ResultWriter = resultWriter;
            return result;
        }

        public static T WithResult<T>(this T result, object value) where T : WebRequestObjectResult {
            result.ReturnValue = value;
            return result;
        }

        public static T WithResult<T, TValue>(this T result, TValue value) where T : WebRequestObjectResult<TValue> {
            result.ReturnValue = value;
            return result;
        }
    }
    

    public interface IWebRequestResult {
        HttpStatusCode StatusCode { get; }
        MimeType ContentType { get; }
        IAsyncWebRequestResultWriter ResultWriter { get; }
        Optional<Exception> Exception { get; }
    }

    public class WebRequestResultBase : IWebRequestResult {
        public HttpStatusCode StatusCode { get; set; }
        public MimeType ContentType { get; set; }
        public IAsyncWebRequestResultWriter ResultWriter { get; set; }

        /// <inheritdoc />
        public Optional<Exception> Exception { get; set; }

        /// <inheritdoc />
        public WebRequestResultBase(HttpStatusCode statusCode = null, MimeType contentType = null) {
            StatusCode = statusCode;
            ContentType = contentType;
        }
    }

    public interface IWebRequestObjectResult : IWebRequestResult {
        Type ReturnType { get; }
        Optional<object> ReturnValue { get; }
    }
    public interface IWebRequestObjectResult<T> : IWebRequestObjectResult {
        new Optional<T> ReturnValue { get; }
    }
    
    public class WebRequestObjectResult<T> : WebRequestResultBase, IWebRequestObjectResult<T> {
        /// <inheritdoc />
        public Type ReturnType => typeof(T);

        /// <inheritdoc />
        public Optional<T> ReturnValue { get; set; }
        
        /// <inheritdoc />
        Optional<object> IWebRequestObjectResult.ReturnValue => ReturnValue.IsDefined
                                                                ? new Optional<object>(ReturnValue.Value)
                                                                : Optional<object>.Undefined;

        public static implicit operator WebRequestObjectResult<T>(T value) {
            return new WebRequestObjectResult<T>{ReturnValue = value};
        }
    }


    public class WebRequestObjectResult : WebRequestResultBase, IWebRequestObjectResult {
        /// <inheritdoc />
        public WebRequestObjectResult(HttpStatusCode statusCode = null, MimeType contentType = null, Optional<object> result = default) : base(statusCode, contentType) {
            ReturnValue = result;
        }

        /// <inheritdoc />
        public WebRequestObjectResult() {
            
        }

        /// <inheritdoc />
        public Type ReturnType => ReturnValue.IsDefined ? ReturnValue.Value.GetType() : null;

        /// <inheritdoc />
        public Optional<object> ReturnValue { get; set; }
    }
}
