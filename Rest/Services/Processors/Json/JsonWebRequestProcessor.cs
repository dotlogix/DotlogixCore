// ==================================================
// Copyright 2018(C) , DotLogix
// File:  JsonWebRequestProcessor.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using DotLogix.Core.Diagnostics;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Rest.Server.Http.Context;
using DotLogix.Core.Rest.Server.Http.Headers;
using DotLogix.Core.Rest.Server.Http.State;
using DotLogix.Core.Rest.Services.Context;
using DotLogix.Core.Rest.Services.Exceptions;
using DotLogix.Core.Rest.Services.Processors.Dynamic;
using DotLogix.Core.Rest.Services.Writer;
using DotLogix.Core.Utils;
#endregion

namespace DotLogix.Core.Rest.Services.Processors.Json {
    public interface IParameterProvider {
        string Name { get; }
        ParameterSources Source { get; }
        IEnumerable<KeyValuePair<string, object>> EnumerateValues(WebServiceContext context);
        bool TryResolve(WebServiceContext context, ParameterInfo parameter, out object paramValue);
    }

    public class JsonParameterProvider : IParameterProvider {
        public const string JsonDataParamName = "$jsonData";

        /// <inheritdoc />
        public string Name { get; } = "JsonBody";

        /// <inheritdoc />
        public ParameterSources Source { get; } = ParameterSources.Body;

        /// <inheritdoc />
        public IEnumerable<KeyValuePair<string, object>> EnumerateValues(WebServiceContext context) {
            if (context.Variables.TryGetValueAs(JsonDataParamName, out Node node) == false)
                return Enumerable.Empty<KeyValuePair<string, object>>();

            var values = new Dictionary<string, object>();
            if(node is NodeMap nodeMap) {
                foreach(var property in nodeMap.Properties()) {
                    values.Add(property.Key, property.Value);
                }
            }
            values.Add(JsonDataParamName, node);
            return values;
        }

        public bool TryResolve(WebServiceContext context, ParameterInfo parameter, out object paramValue) {
            var formatterSettings = context.Settings.Get(WebServiceSettings.JsonFormatterSettings, JsonFormatterSettings.Idented);

            Node child = null;
            if (context.Variables.TryGetValueAs(JsonDataParamName, out Node node)) {
                if (parameter.IsDefined(typeof(JsonBodyAttribute)))
                    child = node;
                else if (node is NodeMap nodeMap)
                    child = nodeMap.GetChild(parameter.Name);
            }
            if (child == null) {
                paramValue = null;
                return false;
            }

            try {
                paramValue = child.ToObject(parameter.ParameterType, formatterSettings);
                return true;
            } catch (Exception e) {
                Log.Warn(e);
                // Not convertible exception ignored in a try method
                paramValue = null;
                return false;
            }
        }

    }

    public class DictionaryParameterProvider : IParameterProvider {
        private readonly Func<WebServiceContext, IDictionary<string, object>> GetSourceFunc;

        public DictionaryParameterProvider(string name, ParameterSources source, Func<WebServiceContext, IDictionary<string, object>> getSourceFunc) {
            GetSourceFunc = getSourceFunc;
            Name = name;
            Source = source;
        }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public ParameterSources Source { get; }

        /// <inheritdoc />
        public IEnumerable<KeyValuePair<string, object>> EnumerateValues(WebServiceContext context) {
            return GetSourceFunc(context);
        }

        public bool TryResolve(WebServiceContext context, ParameterInfo parameter, out object paramValue) {
            var source = GetSourceFunc(context);
            if(source != null)
                return source.TryGetValueAs(parameter.Name, parameter.ParameterType, out paramValue);

            paramValue = null;
            return false;
        }
    }

    public static class ParameterProviders {
        public static IParameterProvider HttpHeader {get;} = new DictionaryParameterProvider("HttpHeader", ParameterSources.Header,c => c.HttpRequest.HeaderParameters);
        public static IParameterProvider HttpUrl { get;} = new DictionaryParameterProvider("HttpUrl", ParameterSources.Url, c=>c.HttpRequest.UrlParameters);
        public static IParameterProvider HttpQuery { get;} = new DictionaryParameterProvider("HttpQuery", ParameterSources.Query, c=>c.HttpRequest.QueryParameters);
        public static IEnumerable<IParameterProvider> Http {
            get { return new[] {HttpHeader, HttpUrl, HttpQuery}; }
        }

        public static IParameterProvider Variables { get; } = new DictionaryParameterProvider("Variables", ParameterSources.Custom, c=>c.Variables);
    }

    public class JsonWebRequestProcessor : DynamicWebRequestProcessor {
        private const string JsonDataParamName = ParseJsonBodyPreProcessor.JsonDataParamName;
        private readonly ConverterSettings _settings;

        public JsonWebRequestProcessor(object target, DynamicInvoke dynamicInvoke, ConverterSettings settings = null) : base(target, dynamicInvoke) {
            _settings = settings ?? new ConverterSettings();
        }

        protected override bool TryGetParameterValue(WebServiceContext context, ParameterInfo methodParam, out object paramValue) {
            Node child = null;
            if (context.Variables.TryGetValueAs(JsonDataParamName, out Node node)) {
                if(methodParam.IsDefined(typeof(JsonBodyAttribute)))
                    child = node;
                else if(node is NodeMap nodeMap)
                    child = nodeMap.GetChild(methodParam.Name);
            }
            if(child == null)
                
                return base.TryGetParameterValue(context, methodParam, out paramValue);
            try {
                paramValue = child.ToObject(methodParam.ParameterType,  _settings);
                return true;
            } catch (Exception e){
                Log.Warn(e);
                // Not convertible exception ignored in a try method
                paramValue = null;
                return false;
            }
        }
    }
}
