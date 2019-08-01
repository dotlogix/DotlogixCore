// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebRequestResultJsonWriter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  05.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Threading.Tasks;
using DotLogix.Core.Nodes;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Server.Http.Context;
using DotLogix.Core.Rest.Server.Http.Headers;
using DotLogix.Core.Rest.Server.Http.State;
#endregion

namespace DotLogix.Core.Rest.Services.Writer {
    public class WebRequestResultJsonWriter : WebRequestResultWriterBase {
        public static JsonFormatterSettings DefaultFormatterSettings { get; } = JsonFormatterSettings.Idented;
        public WebRequestResultJsonWriter(JsonFormatterSettings settings = null) {
            FormatterSettings = settings ?? DefaultFormatterSettings;
        }

        public JsonFormatterSettings FormatterSettings { get; }


        protected override Task WriteResultAsync(IAsyncHttpResponse asyncHttpResponse, WebRequestResult webRequestResult) {
            if(webRequestResult.ReturnValue == null) {
                asyncHttpResponse.StatusCode = HttpStatusCodes.Success.NoContent;
                return Task.CompletedTask;
            }

            asyncHttpResponse.ContentType = MimeTypes.Application.Json;
            return asyncHttpResponse.WriteToResponseStreamAsync(JsonNodes.ToJson(webRequestResult.ReturnValue, webRequestResult.ReturnType, FormatterSettings));
        }
    }
}
