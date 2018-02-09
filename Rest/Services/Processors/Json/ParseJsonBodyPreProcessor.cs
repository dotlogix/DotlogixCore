// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ParseJsonBodyPreProcessor.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  31.01.2018
// LastEdited:  31.01.2018
// ==================================================

#region
using System.Threading.Tasks;
using DotLogix.Core.Nodes;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Server.Http.Mime;
using DotLogix.Core.Rest.Server.Http.Parameters;
#endregion

namespace DotLogix.Core.Rest.Services.Processors.Json {
    public class ParseJsonBodyPreProcessor : IWebRequestProcessor {
        public const string JsonDataParamName = "jsonData";
        public static IWebRequestProcessor Instance { get; } = new ParseJsonBodyPreProcessor();
        private ParseJsonBodyPreProcessor() { }
        public int Priority => int.MaxValue;
        public bool IgnoreHandled => false;

        public async Task ProcessAsync(WebRequestResult webRequestResult) {
            var request = webRequestResult.Context.Request;
            if(request.ContentType != MimeTypes.Json)
                return;

            var json = await request.ReadStringFromRequestStreamAsync();
            if(json.StartsWith("{")) {
                var jsonData = JsonNodes.ToNode(json) as NodeMap;
                request.UserDefinedParameters.Add(new Parameter(JsonDataParamName, jsonData));
            }
        }
    }
}
