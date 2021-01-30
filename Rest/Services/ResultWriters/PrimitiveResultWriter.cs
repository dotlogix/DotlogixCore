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
using DotLogix.Core.Rest.Http;
using DotLogix.Core.Rest.Services.Results;

#endregion

namespace DotLogix.Core.Rest.Services.ResultWriters {
    public class PrimitiveResultWriter : IWebServiceResultWriter {
        public static IWebServiceResultWriter Instance { get; } = new PrimitiveResultWriter();

        /// <inheritdoc />
        protected PrimitiveResultWriter() { }

        public virtual async Task WriteAsync(WebServiceContext context) {
            var response = context.HttpResponse;
            if (response.IsCompleted)
                return;
            
            if(context.Result.Exception.IsDefined) {
                await WriteExceptionAsync(context, context.Result.Exception.Value);
                await response.CompleteAsync();
                return;
            }

            switch (context.Result) {
                case IWebServiceObjectResult requestResult:
                    var value = requestResult.ReturnValue.GetValueOrDefault();
                    if (value is Exception exception)
                        await WriteExceptionAsync(context, exception);
                    else
                        await WriteResultAsync(context, value);
                    break;
                default:
                    var httpResponse = context.HttpResponse;
                    httpResponse.StatusCode = context.Result.StatusCode ?? HttpStatusCodes.Success.Ok;
                    httpResponse.ContentType = context.Result.ContentType ?? MimeTypes.Text.Plain;
                    break;
            }
            await response.CompleteAsync();
        }

        protected virtual Task WriteExceptionAsync(WebServiceContext context, Exception exception) {
            var httpResponse = context.HttpResponse;
            if(exception == null)
                throw new ArgumentNullException(nameof(exception));

            var restException = GetRestExceptionRecursive(exception);
            
            var statusCode = context.Result.StatusCode ?? restException?.ErrorCode;
            httpResponse.StatusCode = statusCode ?? HttpStatusCodes.ServerError.InternalServerError;
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

        protected virtual Task WriteResultAsync(WebServiceContext context, object value) {
            var httpResponse = context.HttpResponse;
            httpResponse.StatusCode = context.Result.StatusCode ?? HttpStatusCodes.Success.Ok;
            httpResponse.ContentType = context.Result.ContentType ?? MimeTypes.Text.Plain;

            if(value != null)
                return httpResponse.WriteToResponseStreamAsync(value.ToString());
            
            if(context.Result.StatusCode == null) {
                httpResponse.StatusCode = HttpStatusCodes.Success.NoContent;
            }
            return Task.CompletedTask;

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
