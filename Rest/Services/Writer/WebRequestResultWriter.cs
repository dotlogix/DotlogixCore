// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebRequestResultWriterBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  28.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Text;
using System.Threading.Tasks;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Server.Http.Context;
using DotLogix.Core.Rest.Server.Http.Headers;
using DotLogix.Core.Rest.Server.Http.State;
using DotLogix.Core.Rest.Services.Context;
using DotLogix.Core.Rest.Services.Exceptions;
#endregion

namespace DotLogix.Core.Rest.Services.Writer {
    public class WebRequestResultWriter : IAsyncWebRequestResultWriter {
        public static IAsyncWebRequestResultWriter Instance { get; } = new WebRequestResultWriter();

        /// <inheritdoc />
        protected WebRequestResultWriter() { }

        public virtual async Task WriteAsync(WebRequestContext context) {
            var response = context.HttpResponse;
            if (response.IsCompleted)
                return;

            var requestResult = context.RequestResult as IWebRequestObjectResult;
            if(requestResult == null)
                throw new ArgumentException($"This result writer accepts only values of type \"{nameof(IWebRequestObjectResult)}\"");

            if (requestResult.Exception.IsDefined) {
                await WriteExceptionAsync(context, requestResult.Exception.Value);
            } else if(requestResult.ReturnValue.IsDefined) {
                await WriteResultAsync(context, requestResult.ReturnValue.Value);
            }

            await response.CompleteAsync();
        }

        protected virtual Task WriteExceptionAsync(WebRequestContext context, Exception exception) {
            var httpResponse = context.HttpResponse;
            if(exception == null)
                throw new ArgumentNullException(nameof(exception));

            var restException = GetRestExceptionRecursive(exception);
            if(httpResponse.StatusCode == HttpStatusCodes.Success.Ok) {
                httpResponse.StatusCode = restException != null
                                                   ? restException.ErrorCode
                                                   : HttpStatusCodes.ServerError.InternalServerError;
            }

            httpResponse.ContentType = MimeTypes.Text.Plain;
            string message;
            if(restException != null) {
                var sb = new StringBuilder();
                sb.AppendLine(restException.ToString());
                sb.AppendLine();
                sb.AppendLine(exception.ToString());
                message = sb.ToString();
            } else
                message = exception.ToString();
            return httpResponse.WriteToResponseStreamAsync(message);
        }

        protected virtual Task WriteResultAsync(WebRequestContext context, object value) {
            var httpResponse = context.HttpResponse;
            
            if (value == null) {
                httpResponse.StatusCode = HttpStatusCodes.Success.NoContent;
                return Task.CompletedTask;
            }

            httpResponse.ContentType = MimeTypes.Text.Plain;
            return httpResponse.WriteToResponseStreamAsync(value.ToString());
        }


        protected RestException GetRestExceptionRecursive(Exception exception) {
            switch(exception) {
                case RestException restException:
                    return restException;
                case AggregateException ae:
                    foreach(var innerException in ae.InnerExceptions) {
                        var restException = GetRestExceptionRecursive(innerException);
                        if(restException != null)
                            return restException;
                    }
                    break;
            }
            return null;
        }
    }
}
