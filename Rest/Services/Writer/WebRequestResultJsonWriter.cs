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
using DotLogix.Core.Rest.Services.Context;
using DotLogix.Core.Rest.Services.Processors.Json;
#endregion

namespace DotLogix.Core.Rest.Services.Writer {
    public class WebRequestResultJsonWriter : WebRequestResultWriterBase {
        public static IAsyncWebRequestResultWriter Instance { get; } = new WebRequestResultJsonWriter();
        protected WebRequestResultJsonWriter() { }

        protected override Task WriteResultAsync(WebServiceContext context) {
            var formatterSettings = context.Settings.Get(WebServiceSettings.JsonFormatterSettings, JsonFormatterSettings.Idented);

            var webRequestResult = context.RequestResult;
            var httpResponse = context.HttpResponse;

            if(webRequestResult.ReturnValue == null) {
                httpResponse.StatusCode = HttpStatusCodes.Success.NoContent;
                return Task.CompletedTask;
            }

            httpResponse.ContentType = MimeTypes.Application.Json;
            return httpResponse.WriteToResponseStreamAsync(JsonNodes.ToJson(webRequestResult.ReturnValue, webRequestResult.ReturnType, formatterSettings));
        }
    }

    public static class WebServiceSettings {
        public const string JsonFormatterSettings = "jsonFormatterSettings";
    }
}
