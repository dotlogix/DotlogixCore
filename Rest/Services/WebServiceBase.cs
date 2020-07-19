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
using DotLogix.Core.Rest.Http;
using DotLogix.Core.Rest.Http.Headers;
#endregion

namespace DotLogix.Core.Rest.Services {
    public abstract class WebServiceBase : IWebService {
        protected WebServiceBase(string routePrefix, string name = null) {
            Name = name ?? GetType().Name;
            RoutePrefix = routePrefix ?? throw new ArgumentNullException(nameof(routePrefix));
        }

        public string Name { get; }
        public string RoutePrefix { get; }

        public static WebServiceResult StatusCode(HttpStatusCode statusCode) {
            return new WebServiceResult(statusCode);
        }

        public static WebServiceObjectResult<T> Value<T>(Optional<T> value, HttpStatusCode statusCode = null, MimeType contentType = null, IWebServiceResultWriter resultWriter = null) {
            return new WebServiceObjectResult<T> {
                StatusCode = statusCode,
                ReturnValue = value,
                ContentType = contentType,
                ResultWriter = resultWriter
            };
        }

        public static WebServiceStreamResult Stream(Stream value, HttpStatusCode statusCode = null, MimeType contentType = null, IWebServiceResultWriter resultWriter = null) {
            return new WebServiceStreamResult {
                Stream = value,
                StatusCode = statusCode,
                ContentType = contentType,
                ResultWriter = resultWriter
            };
        }

        public static WebServiceStreamResult File(string path, HttpStatusCode statusCode = null, MimeType contentType = null, IWebServiceResultWriter resultWriter = null) {
            if(System.IO.File.Exists(path) == false)
                return new WebServiceStreamResult {StatusCode = HttpStatusCodes.ClientError.NotFound};

            var result = new WebServiceStreamResult {
                Stream = System.IO.File.OpenRead(path),
                StatusCode = statusCode,
                ContentType = contentType ?? MimeTypes.GetByExtension(Path.GetExtension(path))
            };
            return result;
        }

        #region Success
        public static WebServiceResult Ok() {
            return StatusCode(HttpStatusCodes.Success.Ok);
        }

        public static WebServiceObjectResult<T> Ok<T>(T value) {
            return Value<T>(value, HttpStatusCodes.Success.Ok);
        }

        public static WebServiceResult Created() {
            return StatusCode(HttpStatusCodes.Success.Created);
        }

        public static WebServiceObjectResult<T> Created<T>(T value) {
            return Value<T>(value, HttpStatusCodes.Success.Created);
        }

        public static WebServiceResult Accepted() {
            return StatusCode(HttpStatusCodes.Success.Accepted);
        }

        public static WebServiceObjectResult<T> Accepted<T>() {
            return Value<T>(default, HttpStatusCodes.Success.Accepted);
        }

        public static WebServiceResult NoContent() {
            return StatusCode(HttpStatusCodes.Success.NoContent);
        }

        public static WebServiceObjectResult<T> NoContent<T>() {
            return Value<T>(default, HttpStatusCodes.Success.NoContent);
        }
        #endregion

        #region ClientError
        public static WebServiceResult BadRequest() {
            return StatusCode(HttpStatusCodes.ClientError.BadRequest);
        }

        public static WebServiceObjectResult<T> BadRequest<T>() {
            return Value<T>(default, HttpStatusCodes.ClientError.BadRequest);
        }

        public static WebServiceResult Unauthorized() {
            return StatusCode(HttpStatusCodes.ClientError.Unauthorized);
        }

        public static WebServiceObjectResult<T> Unauthorized<T>() {
            return Value<T>(default, HttpStatusCodes.ClientError.Unauthorized);
        }

        public static WebServiceResult Forbidden() {
            return StatusCode(HttpStatusCodes.ClientError.Forbidden);
        }

        public static WebServiceObjectResult<T> Forbidden<T>() {
            return Value<T>(default, HttpStatusCodes.ClientError.Forbidden);
        }

        public static WebServiceResult NotFound() {
            return StatusCode(HttpStatusCodes.ClientError.NotFound);
        }

        public static WebServiceObjectResult<T> NotFound<T>() {
            return Value<T>(default, HttpStatusCodes.ClientError.NotFound);
        }

        public static WebServiceResult NotAcceptable() {
            return StatusCode(HttpStatusCodes.ClientError.NotAcceptable);
        }

        public static WebServiceObjectResult<T> NotAcceptable<T>() {
            return Value<T>(default, HttpStatusCodes.ClientError.NotAcceptable);
        }
        #endregion

        #region ServerError
        public static WebServiceResult InternalServerError() {
            return StatusCode(HttpStatusCodes.ServerError.InternalServerError);
        }

        public static WebServiceObjectResult<T> InternalServerError<T>() {
            return Value<T>(default, HttpStatusCodes.ServerError.InternalServerError);
        }

        public static WebServiceResult NotImplemented() {
            return StatusCode(HttpStatusCodes.ServerError.NotImplemented);
        }

        public static WebServiceObjectResult<T> NotImplemented<T>() {
            return Value<T>(default, HttpStatusCodes.ServerError.NotImplemented);
        }
        #endregion
    }
}
