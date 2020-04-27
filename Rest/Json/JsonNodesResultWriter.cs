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
using DotLogix.Core.Rest.Http;
using DotLogix.Core.Rest.Services;
#endregion

namespace DotLogix.Core.Rest.Json {
    public class JsonNodesResultWriter : PrimitiveResultWriter {
        public static IWebServiceResultWriter Instance { get; } = new JsonNodesResultWriter();
        protected JsonNodesResultWriter() { }
        
        protected override async Task WriteResultAsync(WebServiceContext context, object value) {
            var formatterSettings = context.Settings.Get(WebServiceJsonSettings.JsonNodesFormatterSettings, JsonFormatterSettings.Idented);
            var httpResponse = context.HttpResponse;
            httpResponse.StatusCode = context.Result.StatusCode ?? HttpStatusCodes.Success.Ok;
            httpResponse.ContentType = context.Result.ContentType ?? MimeTypes.Application.Json;

            if(value == null) {
                if(context.Result.StatusCode == null) {
                    httpResponse.StatusCode = HttpStatusCodes.Success.NoContent;
                }

                return;
            }

            var json = JsonNodes.ToJson(value, formatterSettings);
            await httpResponse.WriteToResponseStreamAsync(json);
        }
    }
}
