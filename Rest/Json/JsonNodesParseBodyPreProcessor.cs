﻿// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ParseJsonBodyPreProcessor.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Threading.Tasks;
using DotLogix.Core.Nodes;
using DotLogix.Core.Rest.Http;
using DotLogix.Core.Rest.Services;
using DotLogix.Core.Rest.Services.Processors;
#endregion

namespace DotLogix.Core.Rest.Json {
    public class JsonNodesParseBodyPreProcessor : WebRequestProcessorBase {
        public static IWebRequestProcessor Instance { get; } = new JsonNodesParseBodyPreProcessor();
        
        private JsonNodesParseBodyPreProcessor() : base(int.MaxValue) { }

        public override async Task ProcessAsync(WebServiceContext context) {
            var request = context.HttpRequest;
            if(request.HasBody == false || request.ContentType != MimeTypes.Application.Json)
                return;

            var json = await request.ReadStringFromRequestStreamAsync();
            if(json.Length > 1) {
                try {
                    var jsonData = await JsonNodes.ToNodeAsync(request.InputStream, request.ContentEncoding);
                    context.Variables.Add(WebServiceJsonSettings.JsonRawParamName, json);
                    context.Variables.Add(WebServiceJsonSettings.JsonDataParamName, jsonData);
                    var formatterSettings = context.Settings.Get(WebServiceJsonSettings.JsonNodesFormatterSettings, JsonFormatterSettings.Idented);
                    context.ParameterProviders.Add(new JsonNodesParameterProvider(jsonData, formatterSettings));
                } catch(Exception e) {
                    context.LogSource.Warn(e);
                    context.SetException(e, HttpStatusCodes.ClientError.BadRequest);
                }
            }
        }
    }
}
