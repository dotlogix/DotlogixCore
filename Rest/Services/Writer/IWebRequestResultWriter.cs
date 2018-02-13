// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IWebRequestResultWriter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  31.01.2018
// LastEdited:  31.01.2018
// ==================================================

#region
using System;
using System.Threading.Tasks;
using DotLogix.Core.Nodes;
using DotLogix.Core.Rest.Server.Http.Mime;
using DotLogix.Core.Rest.Server.Http.State;
using DotLogix.Core.Rest.Services.Exceptions;
#endregion

namespace DotLogix.Core.Rest.Server.Http {
    public interface IWebRequestResultWriter {
        Task WriteAsync(WebRequestResult requestResult);
    }

    public class WebRequestResultJsonWriter : IWebRequestResultWriter {
        public static IWebRequestResultWriter Instance { get; } = new WebRequestResultJsonWriter();
        private WebRequestResultJsonWriter() { }

        public async Task WriteAsync(WebRequestResult requestResult) {
            var response = requestResult.Context.Response;

            string message;
            if(requestResult.Succeed) {
                message = JsonNodes.ToJson(requestResult.Result);
                if(requestResult.Result == null)
                    response.StatusCode = HttpStatusCodes.Success.NoContent;
            } else {
                if(response.StatusCode == HttpStatusCodes.Success.Ok) {
                    var restException = GetRestExceptionRecursive(requestResult.Exception);
                    response.StatusCode = restException != null
                                              ? restException.ErrorCode
                                              : HttpStatusCodes.ServerError.InternalServerError;
                }
                message = JsonNodes.ToJson(requestResult.Exception);
            }
            response.ContentType = MimeTypes.Json;

            await response.WriteToResponseStreamAsync(message);
            await response.SendAsync();
        }

        private RestException GetRestExceptionRecursive(Exception exception) {
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
