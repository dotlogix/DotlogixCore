// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ParseJsonBodyPreProcessor.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Threading.Tasks;
using DotLogix.Core.Diagnostics;
using DotLogix.Core.Nodes;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Server.Http.Headers;
using DotLogix.Core.Rest.Server.Http.State;
using DotLogix.Core.Rest.Services.Context;
using DotLogix.Core.Rest.Services.Exceptions;
using DotLogix.Core.Rest.Services.Writer;
#endregion

namespace DotLogix.Core.Rest.Services.Processors.Json {
    public class JsonNodesParseBodyPreProcessor : WebRequestProcessorBase {
        public static IWebRequestProcessor Instance { get; } = new JsonNodesParseBodyPreProcessor();
        
        private JsonNodesParseBodyPreProcessor() : base(int.MaxValue) { }

        public override async Task ProcessAsync(WebRequestContext context) {
            var request = context.HttpRequest;
            if(request.HasBody == false || request.ContentType != MimeTypes.Application.Json)
                return;

            var json = await request.ReadStringFromRequestStreamAsync();
            if(json.Length > 1) {
                try {
                    var jsonData = JsonNodes.ToNode(json);
                    context.Variables.Add(JsonNodesParameterProvider.JsonRawParamName, json);
                    context.Variables.Add(JsonNodesParameterProvider.JsonDataParamName, jsonData);
                    var formatterSettings = context.Settings.Get(WebServiceSettings.JsonNodesFormatterSettings, JsonFormatterSettings.Idented);
                    context.ParameterProviders.Add(new JsonNodesParameterProvider(jsonData, formatterSettings));
                } catch(Exception e) {
                    Log.Warn(e);
                    context.SetException(e, HttpStatusCodes.ClientError.BadRequest);
                }
            }
        }
    }
}
