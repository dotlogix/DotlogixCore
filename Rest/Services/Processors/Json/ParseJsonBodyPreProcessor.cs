﻿// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ParseJsonBodyPreProcessor.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.Threading.Tasks;
using DotLogix.Core.Nodes;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Server.Http.Mime;
using DotLogix.Core.Rest.Server.Http.Parameters;
using DotLogix.Core.Rest.Server.Http.State;
using DotLogix.Core.Rest.Services.Exceptions;
#endregion

namespace DotLogix.Core.Rest.Services.Processors.Json {
    public class ParseJsonBodyPreProcessor : IWebRequestProcessor {
        public const string JsonDataParamName = "jsonData";
        public static IWebRequestProcessor Instance { get; } = new ParseJsonBodyPreProcessor();
        private ParseJsonBodyPreProcessor() { }
        public int Priority => int.MaxValue;
        public bool IgnoreHandled => true;

        public async Task ProcessAsync(WebRequestResult webRequestResult) {
            var request = webRequestResult.Context.Request;
            if(request.ContentType.Code.Contains(MimeTypes.Application.Json.Code) == false)
                return;

            var json = await request.ReadStringFromRequestStreamAsync();
            if(json.Length > 1) {
                try {
                    var jsonData = JsonNodes.ToNode(json);
                    request.UserDefinedParameters.Add(new Parameter(JsonDataParamName, jsonData));
                } catch (Exception e) {
                    webRequestResult.SetException(new RestException(HttpStatusCodes.ClientError.BadRequest, "The body of the request is not in a valid json format", e));
                }
            }
        }
    }
}
