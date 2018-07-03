// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebRequestProcessor.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.Text;
using System.Threading.Tasks;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Server.Http.Context;
using DotLogix.Core.Rest.Server.Http.Parameters;
using DotLogix.Core.Rest.Services.Context;
using DotLogix.Core.Rest.Services.Descriptors;
#endregion

namespace DotLogix.Core.Rest.Services.Processors.Base {
    public class WebRequestProcessor : WebRequestProcessorBase {
        public bool IsAsyncMethod { get; }
        public object Target { get; }
        public DynamicInvoke DynamicInvoke { get; }

        public WebRequestProcessor(object target, DynamicInvoke dynamicInvoke, int priority = 0, bool ignoreHandled = true) : base(priority, ignoreHandled) {
            Target = target;
            DynamicInvoke = dynamicInvoke;
            IsAsyncMethod = dynamicInvoke.ReturnType.IsAssignableTo(typeof(Task));

            Descriptors.Add(new MethodDescriptor(dynamicInvoke));
        }

        public override async Task ProcessAsync(WebServiceContext webServiceContext) {
            var webRequestResult = webServiceContext.RequestResult;
            var parameters = CreateParameters(webRequestResult);
            if(webRequestResult.Handled)
                return;

            try {
                var result = await InvokeAsync(webRequestResult, parameters);
                webRequestResult.TrySetResult(result);
            } catch(Exception e) {
                webRequestResult.TrySetException(e);
            }
        }

        protected virtual object[] CreateParameters(WebRequestResult webRequestResult) {
            return new object[] {webRequestResult.Context};
        }

        protected virtual async Task<object> InvokeAsync(WebRequestResult webRequest, object[] parameters) {
            var returnValue = DynamicInvoke.Invoke(Target, parameters);
            if(IsAsyncMethod)
                returnValue = await ((Task)returnValue).UnpackResultAsync();
            return returnValue;
        }

        protected virtual void AppendParameterValues(StringBuilder builder, IAsyncHttpRequest request) {
            if(request.UrlParameters.ChildCount > 0) {
                builder.AppendLine("Url:");
                AppendParameterValues(builder, request.UrlParameters);
                builder.AppendLine();
            }

            if(request.QueryParameters.ChildCount > 0) {
                builder.AppendLine("Query:");
                AppendParameterValues(builder, request.QueryParameters);
                builder.AppendLine();
            }

            if(request.HeaderParameters.ChildCount > 0) {
                builder.AppendLine("Header:");
                AppendParameterValues(builder, request.HeaderParameters);
                builder.AppendLine();
            }

            if(request.UserDefinedParameters.ChildCount > 0) {
                builder.AppendLine("UserDefined:");
                AppendParameterValues(builder, request.UserDefinedParameters);
                builder.AppendLine();
            }
        }

        protected virtual void AppendParameterValues(StringBuilder builder, NodeMap parameters)
        {
            builder.Append(parameters);
        }
    }
}
