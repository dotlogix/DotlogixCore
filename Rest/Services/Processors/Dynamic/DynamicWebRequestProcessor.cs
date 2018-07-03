// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DynamicWebRequestProcessor.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Reflection;
using System.Text;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Server.Http.Context;
using DotLogix.Core.Rest.Server.Http.State;
using DotLogix.Core.Rest.Services.Exceptions;
using DotLogix.Core.Rest.Services.Processors.Base;
#endregion

namespace DotLogix.Core.Rest.Services.Processors.Dynamic {
    public class DynamicWebRequestProcessor : WebRequestProcessor {
        public DynamicWebRequestProcessor(object target, DynamicInvoke dynamicInvoke) : base(target, dynamicInvoke) { }

        protected override object[] CreateParameters(WebRequestResult webRequestResult) {
            var request = webRequestResult.Context.Request;
            if(TryGetParameterValues(request, out var parameters))
                return parameters;

            webRequestResult.TrySetException(CreateBadRequestException(request));
            return null;
        }

        protected virtual bool TryGetParameterValues(IAsyncHttpRequest request, out object[] paramValues) {
            var methodParams = DynamicInvoke.Parameters;
            paramValues = new object[methodParams.Length];
            for(var i = 0; i < paramValues.Length; i++) {
                var methodParam = methodParams[i];
                if(TryGetParameterValue(request, methodParam, out paramValues[i]) == false)
                    return false;
            }
            return true;
        }

        protected virtual bool TryGetParameterValue(IAsyncHttpRequest request, ParameterInfo methodParam, out object paramValue) {
            var name = methodParam.Name;
            var type = methodParam.ParameterType;
            if(request.UserDefinedParameters.TryGetChildValue(name, type, out paramValue) ||
               request.QueryParameters.TryGetChildValue(name, type, out paramValue) ||
               request.UrlParameters.TryGetChildValue(name, type, out paramValue) ||
               request.HeaderParameters.TryGetChildValue(name, type, out paramValue))
                return true;

            if(methodParam.IsOptional) {
                paramValue = methodParam.DefaultValue;
                return true;
            }
            paramValue = null;
            return false;
        }

        protected virtual RestException CreateBadRequestException(IAsyncHttpRequest request) {
            var builder = new StringBuilder();
            builder.AppendLine($"One or more arguments are not defined for method {DynamicInvoke.Name}");
            builder.AppendLine();

            var parameters = DynamicInvoke.Parameters;
            builder.AppendLine("Required arguments:");
            var currentParamIndex = 0;
            ParameterInfo currentParam;
            while((currentParamIndex < parameters.Length) &&
                  !(currentParam = parameters[currentParamIndex]).IsOptional) {
                builder.AppendLine($"{currentParam.Name} ({currentParam.ParameterType.Name})");
                currentParamIndex++;
            }
            builder.AppendLine();

            builder.AppendLine("Optional arguments:");
            while(currentParamIndex < parameters.Length) {
                currentParam = parameters[currentParamIndex];
                builder.AppendLine($"{currentParam.Name} ({currentParam.ParameterType.Name})");
                currentParamIndex++;
            }
            builder.AppendLine();
            builder.AppendLine("Given Parameters:");
            AppendParameterValues(builder, request);

            return new RestException(HttpStatusCodes.ClientError.BadRequest, builder.ToString());
        }
    }
}
