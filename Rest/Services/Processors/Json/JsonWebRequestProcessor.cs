// ==================================================
// Copyright 2018(C) , DotLogix
// File:  JsonWebRequestProcessor.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Reflection;
using DotLogix.Core.Nodes;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Rest.Server.Http.Context;
using DotLogix.Core.Rest.Services.Attributes.Routes;
using DotLogix.Core.Rest.Services.Processors.Dynamic;
#endregion

namespace DotLogix.Core.Rest.Services.Processors.Json {
    public class JsonWebRequestProcessor : DynamicWebRequestProcessor {
        private const string JsonDataParamName = ParseJsonBodyPreProcessor.JsonDataParamName;

        public JsonWebRequestProcessor(object target, DynamicInvoke dynamicInvoke) : base(target, dynamicInvoke) { }

        protected override bool TryGetParameterValue(IAsyncHttpRequest request, ParameterInfo methodParam, out object paramValue) {
            Node child = null;

            if(request.UserDefinedParameters.TryGetParameterValue(JsonDataParamName, out var jsonBody) && jsonBody is Node node) {
                if(methodParam.IsDefined(typeof(JsonBodyAttribute)))
                    child = node;
                else if(node is NodeMap nodeMap)
                    child = nodeMap.GetChild(methodParam.Name);
            }
            if(child == null)
                return base.TryGetParameterValue(request, methodParam, out paramValue);
            paramValue = Nodes.Nodes.ToObject(child, methodParam.ParameterType);
            return true;
        }
    }
}
