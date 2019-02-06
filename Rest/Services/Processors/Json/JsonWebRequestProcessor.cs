// ==================================================
// Copyright 2018(C) , DotLogix
// File:  JsonWebRequestProcessor.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Reflection;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Rest.Server.Http.Context;
using DotLogix.Core.Rest.Services.Context;
using DotLogix.Core.Rest.Services.Processors.Dynamic;
#endregion

namespace DotLogix.Core.Rest.Services.Processors.Json {
    public class JsonWebRequestProcessor : DynamicWebRequestProcessor {
        private const string JsonDataParamName = ParseJsonBodyPreProcessor.JsonDataParamName;

        public JsonWebRequestProcessor(object target, DynamicInvoke dynamicInvoke) : base(target, dynamicInvoke) { }

        protected override bool TryGetParameterValue(WebServiceContext context, ParameterInfo methodParam, out object paramValue) {
            Node child = null;

            if(context.Variables.TryGetValueAs(JsonDataParamName, out Node node)) {
                if(methodParam.IsDefined(typeof(JsonBodyAttribute)))
                    child = node;
                else if(node is NodeMap nodeMap)
                    child = nodeMap.GetChild(methodParam.Name);
            }
            if(child == null)
                return base.TryGetParameterValue(context, methodParam, out paramValue);
            paramValue = child.ToObject(methodParam.ParameterType);
            return true;
        }
    }
}
