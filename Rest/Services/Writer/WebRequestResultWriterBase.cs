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
using DotLogix.Core.Rest.Services.Exceptions;
#endregion

namespace DotLogix.Core.Rest.Services.Writer {
    public class WebRequestResultWriterBase : IAsyncWebRequestResultWriter {
        public async Task WriteAsync(WebRequestResult requestResult) {
            var response = requestResult.HttpContext.Response;
            if(response.IsCompleted)
                return;

            if(requestResult.Succeed)
                await WriteResultAsync(response, requestResult);
            else
                await WriteExceptionAsync(response, requestResult);

            if(requestResult.CustomStatusCode != null)
                response.StatusCode = requestResult.CustomStatusCode;

            await response.CompleteAsync();
        }

        protected virtual Task WriteExceptionAsync(IAsyncHttpResponse asyncHttpResponse, WebRequestResult webRequestResult) {
            var restException = GetRestExceptionRecursive(webRequestResult.Exception);
            if(asyncHttpResponse.StatusCode == HttpStatusCodes.Success.Ok) {
                asyncHttpResponse.StatusCode = restException != null
                                                   ? restException.ErrorCode
                                                   : HttpStatusCodes.ServerError.InternalServerError;
            }

            asyncHttpResponse.ContentType = MimeTypes.Text.Plain;
            string message;
            if(restException != null) {
                var sb = new StringBuilder();
                sb.AppendLine(restException.ToString());
                sb.AppendLine();
                sb.AppendLine(webRequestResult.Exception.ToString());
                message = sb.ToString();
            } else
                message = webRequestResult.Exception.ToString();
            return asyncHttpResponse.WriteToResponseStreamAsync(message);
        }

        protected virtual Task WriteResultAsync(IAsyncHttpResponse asyncHttpResponse, WebRequestResult webRequestResult) {
            if(webRequestResult.ReturnValue == null) {
                asyncHttpResponse.StatusCode = HttpStatusCodes.Success.NoContent;
                return Task.CompletedTask;
            }

            asyncHttpResponse.ContentType = MimeTypes.Text.Plain;
            return asyncHttpResponse.WriteToResponseStreamAsync(webRequestResult.ReturnValue.ToString());
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
