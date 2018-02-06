// ==================================================
// Copyright 2018(C) , DotLogix
// File:  JsonWebRequestProcessor.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  31.01.2018
// LastEdited:  31.01.2018
// ==================================================

#region
using System.Reflection;
using DotLogix.Core.Nodes;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Rest.Server.Http.Context;
using DotLogix.Core.Rest.Services.Processors.Dynamic;
#endregion

namespace DotLogix.Core.Rest.Services.Processors.Json {
    public class JsonWebRequestProcessor : DynamicWebRequestProcessor {
        private const string JsonDataParamName = ParseJsonBodyPreProcessor.JsonDataParamName;

        public JsonWebRequestProcessor(object target, DynamicInvoke dynamicInvoke) : base(target, dynamicInvoke) { }

        protected override bool TryGetParameterValue(IAsyncHttpRequest request, ParameterInfo methodParam, out object paramValue) {
            Node child = null;
            if(request.UserDefinedParameters.GetParameterValue(JsonDataParamName) is NodeMap jsonData)
                child = jsonData.GetChild(methodParam.Name);
            if(child == null)
                return base.TryGetParameterValue(request, methodParam, out paramValue);
            paramValue = Nodes.Nodes.ToObject(child, methodParam.ParameterType);
            return true;
        }
    }
}
