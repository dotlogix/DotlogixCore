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
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Server.Http.Context;
#endregion

namespace DotLogix.Core.Rest.Services.Processors.Base {
    public class WebRequestProcessor : IWebRequestProcessor {
        public bool IsAsyncMethod { get; }
        public object Target { get; }
        public DynamicInvoke DynamicInvoke { get; }

        public WebRequestProcessor(object target, DynamicInvoke dynamicInvoke, int priority = 0, bool ignoreHandled = true) {
            Target = target;
            DynamicInvoke = dynamicInvoke;
            Priority = priority;
            IgnoreHandled = ignoreHandled;
            IsAsyncMethod = dynamicInvoke.ReturnType.IsAssignableTo(typeof(Task));
        }

        public int Priority { get; }
        public bool IgnoreHandled { get; }

        public virtual async Task ProcessAsync(WebRequestResult webRequestResult) {
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
            if(request.UrlParameters.HasValues) {
                builder.AppendLine("Url:");
                request.UrlParameters.AppendString(builder);
                builder.AppendLine();
            }

            if(request.QueryParameters.HasValues) {
                builder.AppendLine("Query:");
                request.QueryParameters.AppendString(builder);
                builder.AppendLine();
            }

            if(request.HeaderParameters.HasValues) {
                builder.AppendLine("Header:");
                request.HeaderParameters.AppendString(builder);
                builder.AppendLine();
            }

            if(request.UserDefinedParameters.HasValues) {
                builder.AppendLine("UserDefined:");
                request.HeaderParameters.AppendString(builder);
                builder.AppendLine();
            }
        }
    }
}
