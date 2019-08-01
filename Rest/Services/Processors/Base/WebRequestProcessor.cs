// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebRequestProcessor.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DotLogix.Core.Diagnostics;
using DotLogix.Core.Extensions;
using DotLogix.Core.Reflection.Dynamics;
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

        public override async Task ProcessAsync(WebServiceContext context) {
            var webRequestResult = context.RequestResult;
            var parameters = CreateParameters(context);
            if(webRequestResult.Handled)
                return;

            try {
                var result = await InvokeAsync(context, parameters);
                webRequestResult.TrySetResult(result);
            } catch(Exception e) {
                Log.Error(e);
                webRequestResult.TrySetException(e);
            }
        }

        protected virtual object[] CreateParameters(WebServiceContext context) {
            return new object[] {context};
        }

        protected virtual async Task<object> InvokeAsync(WebServiceContext context, object[] parameters) {
            var returnValue = DynamicInvoke.Invoke(Target, parameters);
            if(IsAsyncMethod)
                returnValue = await ((Task)returnValue).UnpackResultAsync();
            return returnValue;
        }

        protected virtual void AppendParameterValues(StringBuilder builder, WebServiceContext context) {
            var request = context.HttpRequest;
            if(request.UrlParameters.Count > 0) {
                builder.AppendLine("Url:");
                AppendParameterValues(builder, request.UrlParameters);
                builder.AppendLine();
            }

            if(request.QueryParameters.Count > 0) {
                builder.AppendLine("Query:");
                AppendParameterValues(builder, request.QueryParameters);
                builder.AppendLine();
            }

            if(request.HeaderParameters.Count > 0) {
                builder.AppendLine("Header:");
                AppendParameterValues(builder, request.HeaderParameters);
                builder.AppendLine();
            }

            if(context.Variables.Count > 0) {
                builder.AppendLine("UserDefined:");
                AppendParameterValues(builder, context.Variables);
                builder.AppendLine();
            }
        }

        protected virtual void AppendParameterValues(StringBuilder builder, IDictionary<string, object> parameters) {
            foreach(var parameter in parameters) {
                builder.Append($"{parameter.Key} = {(parameter.Value is Array array ? string.Join(",", array) : parameter.Value)}");
            }

        }
    }
}
