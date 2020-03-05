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
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DotLogix.Core.Diagnostics;
using DotLogix.Core.Extensions;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Rest.Server.Http.State;
using DotLogix.Core.Rest.Services.Context;
using DotLogix.Core.Rest.Services.Descriptors;
using DotLogix.Core.Rest.Services.Exceptions;
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

            if (TryGetParameterValues(context, out var parameters))
                return parameters;

            context.RequestResult.TrySetException(CreateBadRequestException(context));
            return null;
        }

        protected virtual bool TryGetParameterValues(WebServiceContext context, out object[] paramValues) {
            var methodParams = DynamicInvoke.Parameters;
            paramValues = new object[methodParams.Length];
            for (var i = 0; i < paramValues.Length; i++) {
                var methodParam = methodParams[i];
                if (TryGetParameterValue(context, methodParam, out paramValues[i]) == false)
                    return false;
            }
            return true;
        }

        protected virtual bool TryGetParameterValue(WebServiceContext context, ParameterInfo methodParam, out object paramValue) {
            var name = methodParam.Name;
            var type = methodParam.ParameterType;
            var request = context.HttpRequest;

            if(context.Variables.TryGetValueAs(name, type, out paramValue)) {
                return true;
            }

            for(var i = context.ParameterProviders.Count - 1; i >= 0; i--) {
                var parameterProvider = context.ParameterProviders[i];
                if(parameterProvider.TryResolve(context, methodParam, out paramValue)) {
                    return true;
                }
            }

            if (methodParam.IsOptional) {
                paramValue = methodParam.DefaultValue;
                return true;
            }
            paramValue = null;
            return false;
        }

        protected virtual RestException CreateBadRequestException(WebServiceContext context) {
            var builder = new StringBuilder();
            builder.AppendLine($"One or more arguments are not defined for method {DynamicInvoke.Name}");
            builder.AppendLine();

            var parameters = DynamicInvoke.Parameters;
            builder.AppendLine("Required arguments:");
            var currentParamIndex = 0;
            ParameterInfo currentParam;
            while ((currentParamIndex < parameters.Length) &&
                  !(currentParam = parameters[currentParamIndex]).IsOptional) {
                builder.AppendLine($"{currentParam.Name} ({currentParam.ParameterType.Name})");
                currentParamIndex++;
            }
            builder.AppendLine();

            builder.AppendLine("Optional arguments:");
            while (currentParamIndex < parameters.Length) {
                currentParam = parameters[currentParamIndex];
                builder.AppendLine($"{currentParam.Name} ({currentParam.ParameterType.Name})");
                currentParamIndex++;
            }
            builder.AppendLine();
            builder.AppendLine("Given Parameters:");
            AppendParameterValues(builder, context);

            return new RestException(HttpStatusCodes.ClientError.BadRequest, builder.ToString());
        }

        protected virtual async Task<object> InvokeAsync(WebServiceContext context, object[] parameters) {
            var returnValue = DynamicInvoke.Invoke(Target, parameters);
            if(IsAsyncMethod)
                returnValue = await ((Task)returnValue).UnpackResultAsync();
            return returnValue;
        }

        protected virtual void AppendParameterValues(StringBuilder builder, WebServiceContext context) {
            for(var i = context.ParameterProviders.Count - 1; i >= 0; i--) {
                var parameterProvider = context.ParameterProviders[i];
                var values = parameterProvider.EnumerateValues(context)
                                              .OrderBy(v => v.Key)
                                              .ToList();

                if(values.Count == 0)
                    continue;

                builder.Append(parameterProvider.Source)
                       .Append(" { ")
                       .Append(parameterProvider.Name)
                       .AppendLine("}");
                AppendParameterValues(builder, values);
                builder.AppendLine();
            }
        }

        protected virtual void AppendParameterValues(StringBuilder builder, IEnumerable<KeyValuePair<string, object>> parameters) {
            foreach(var parameter in parameters) {
                builder.AppendLine($"{parameter.Key} = {(parameter.Value is Array array ? string.Join(",", array) : parameter.Value)}");
            }
        }
    }
}
