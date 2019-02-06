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
using DotLogix.Core.Nodes;
using DotLogix.Core.Rest.Server.Http.Headers;
using DotLogix.Core.Rest.Server.Http.State;
using DotLogix.Core.Rest.Services.Context;
using DotLogix.Core.Rest.Services.Exceptions;
#endregion

namespace DotLogix.Core.Rest.Services.Processors.Json {
    public class ParseJsonBodyPreProcessor : WebRequestProcessorBase {
        public const string JsonDataParamName = "$jsonData";
        public static IWebRequestProcessor Instance { get; } = new ParseJsonBodyPreProcessor();
        private ParseJsonBodyPreProcessor() : base(int.MaxValue) { }

        public override async Task ProcessAsync(WebServiceContext context) {
            var request = context.HttpRequest;
            if(request.ContentType != MimeTypes.Application.Json)
                return;

            var json = await request.ReadStringFromRequestStreamAsync();
            if(json.Length > 1) {
                try {
                    var jsonData = JsonNodes.ToNode(json);
                    context.Variables.Add(JsonDataParamName, jsonData);
                } catch(Exception e) {
                    context.RequestResult.SetException(new RestException(HttpStatusCodes.ClientError.BadRequest, "The body of the request is not in a valid json format", e));
                }
            }
        }
    }
}
