// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebRequestResultJsonWriter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  05.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.IO;
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
    public class JsonNodesWebRequestResultWriter : WebRequestResultWriter {
        public static IAsyncWebRequestResultWriter Instance { get; } = new JsonNodesWebRequestResultWriter();
        protected JsonNodesWebRequestResultWriter() { }
        
        protected override async Task WriteResultAsync(WebRequestContext context, object value) {
            var formatterSettings = context.Settings.Get(WebServiceSettings.JsonNodesFormatterSettings, JsonFormatterSettings.Idented);
            var httpResponse = context.HttpResponse;
            if(value == null) {
                httpResponse.StatusCode = HttpStatusCodes.Success.NoContent;
                return;
            }

            httpResponse.ContentType = MimeTypes.Application.Json;
            var json = JsonNodes.ToJson(value, formatterSettings);
            await httpResponse.WriteToResponseStreamAsync(json);
        }
    }
}
