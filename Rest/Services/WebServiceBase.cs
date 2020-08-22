// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebServiceBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.IO;
using System.Reflection;
using DotLogix.Core.Rest.Http;
using DotLogix.Core.Rest.Http.Headers;
using DotLogix.Core.Rest.Services.Attributes;
#endregion

namespace DotLogix.Core.Rest.Services {
    public abstract class WebServiceBase : IWebService {
        protected WebServiceBase() : this(null, null) { }

        protected WebServiceBase(string route) : this(null, route) {
            
        }

        protected WebServiceBase(string name, string route) {
            var attribute = GetType().GetCustomAttribute<WebServiceAttribute>();
            Name = name ?? attribute?.Name ?? GetType().Name;
            RoutePrefix = route ?? attribute?.Route ?? throw new ArgumentNullException(nameof(route));
        }

        public string Name { get; }
        public string RoutePrefix { get; }

        public static WebServiceResult StatusCode(HttpStatusCode statusCode) {
            return new WebServiceResult(statusCode);
        }

        public static WebServiceResult StatusCode(HttpStatusCode statusCode, Exception exception) {
            return new WebServiceResult(statusCode) {Exception = exception};
        }

        public static WebServiceObjectResult<T> Value<T>(Optional<T> value, HttpStatusCode statusCode = null, MimeType contentType = null, IWebServiceResultWriter resultWriter = null) {
            var webServiceObjectResult = new WebServiceObjectResult<T> {
                StatusCode = statusCode,
                ReturnValue = value,
                ContentType = contentType,
                ResultWriter = resultWriter
            };

            if(value.Value is Exception exception)
                webServiceObjectResult.Exception = exception;

            return webServiceObjectResult;
        }

        #region Stream
        public static WebServiceStreamResult Stream(Stream value, HttpStatusCode statusCode = null, MimeType contentType = null, IWebServiceResultWriter resultWriter = null) {
            return new WebServiceStreamResult {
                Stream = value,
                StatusCode = statusCode,
                ContentType = contentType,
                ResultWriter = resultWriter
            };
        }

        public static WebServiceStreamResult Stream(byte[] byteArray, HttpStatusCode statusCode = null, MimeType contentType = null, IWebServiceResultWriter resultWriter = null) {
            return Stream(byteArray, 0, byteArray.Length, statusCode, contentType);
        }

        public static WebServiceStreamResult Stream(byte[] byteArray, int index, int count, HttpStatusCode statusCode = null, MimeType contentType = null, IWebServiceResultWriter resultWriter = null) {
            var stream = new MemoryStream(byteArray, index, count);
            var result = new WebServiceStreamResult {
                Stream = stream,
                StatusCode = statusCode,
                ContentType = contentType
            };
            return result;
        }

        public static WebServiceStreamResult Stream(string path, HttpStatusCode statusCode = null, MimeType contentType = null, IWebServiceResultWriter resultWriter = null) {
            if(File.Exists(path) == false)
                return new WebServiceStreamResult {StatusCode = HttpStatusCodes.ClientError.NotFound};

            var result = new WebServiceStreamResult {
                Stream = File.OpenRead(path),
                StatusCode = statusCode,
                ContentType = contentType ?? MimeTypes.GetByExtension(Path.GetExtension(path))
            };
            return result;
        }
        #endregion

        #region Success
        public static WebServiceResult Ok() {
            return StatusCode(HttpStatusCodes.Success.Ok);
        }

        public static WebServiceObjectResult<T> Ok<T>(T value = default) {
            return Value<T>(value, HttpStatusCodes.Success.Ok);
        }

        public static WebServiceResult Created() {
            return StatusCode(HttpStatusCodes.Success.Created);
        }

        public static WebServiceObjectResult<T> Created<T>(T value = default) {
            return Value<T>(value, HttpStatusCodes.Success.Created);
        }

        public static WebServiceResult Accepted() {
            return StatusCode(HttpStatusCodes.Success.Accepted);
        }

        public static WebServiceObjectResult<T> Accepted<T>(T value = default) {
            return Value<T>(value, HttpStatusCodes.Success.Accepted);
        }

        public static WebServiceResult NoContent() {
            return StatusCode(HttpStatusCodes.Success.NoContent);
        }

        public static WebServiceObjectResult<T> NoContent<T>(T value = default) {
            return Value<T>(value, HttpStatusCodes.Success.NoContent);
        }
        #endregion

        #region ClientError
        public static WebServiceResult BadRequest() {
            return StatusCode(HttpStatusCodes.ClientError.BadRequest);
        }

        public static WebServiceObjectResult<T> BadRequest<T>(T value = default) {
            return Value<T>(value, HttpStatusCodes.ClientError.BadRequest);
        }

        public static WebServiceResult Unauthorized() {
            return StatusCode(HttpStatusCodes.ClientError.Unauthorized);
        }

        public static WebServiceObjectResult<T> Unauthorized<T>(T value = default) {
            return Value<T>(value, HttpStatusCodes.ClientError.Unauthorized);
        }

        public static WebServiceResult Forbidden() {
            return StatusCode(HttpStatusCodes.ClientError.Forbidden);
        }

        public static WebServiceObjectResult<T> Forbidden<T>(T value = default) {
            return Value<T>(value, HttpStatusCodes.ClientError.Forbidden);
        }

        public static WebServiceResult NotFound() {
            return StatusCode(HttpStatusCodes.ClientError.NotFound);
        }

        public static WebServiceObjectResult<T> NotFound<T>(T value = default) {
            return Value<T>(value, HttpStatusCodes.ClientError.NotFound);
        }

        public static WebServiceResult NotAcceptable() {
            return StatusCode(HttpStatusCodes.ClientError.NotAcceptable);
        }

        public static WebServiceObjectResult<T> NotAcceptable<T>(T value = default) {
            return Value<T>(value, HttpStatusCodes.ClientError.NotAcceptable);
        }
        #endregion

        #region ServerError
        public static RedirectWebServiceResult Redirect(string url, bool preserveMethod = false) {
            return new RedirectWebServiceResult {
                RedirectTo = url,
                Permanent = false,
                PreserveMethod = preserveMethod
            };
        }

        public static RedirectWebServiceResult RedirectPermanent(string url, bool preserveMethod = false) {
            return new RedirectWebServiceResult {
                RedirectTo = url,
                Permanent = false,
                PreserveMethod = preserveMethod
            };
        }
        #endregion

        #region ServerError
        public static WebServiceResult InternalServerError(Exception exception = null) {
            return StatusCode(HttpStatusCodes.ServerError.InternalServerError);
        }

        public static WebServiceObjectResult<T> InternalServerError<T>(Exception exception = null) {
            return Value<T>(default, HttpStatusCodes.ServerError.InternalServerError);
        }

        public static WebServiceResult NotImplemented(Exception exception = null) {
            return StatusCode(HttpStatusCodes.ServerError.NotImplemented);
        }

        public static WebServiceObjectResult<T> NotImplemented<T>(Exception exception = null) {
            return Value<T>(default, HttpStatusCodes.ServerError.NotImplemented);
        }
        #endregion
    }
}
