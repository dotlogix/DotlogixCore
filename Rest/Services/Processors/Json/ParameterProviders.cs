using System.Collections.Generic;
using DotLogix.Core.Rest.Server.Http.Context;

namespace DotLogix.Core.Rest.Services.Processors.Json {
    public static class ParameterProviders {
        public static IParameterProvider HttpHeader {get;} = new DictionaryParameterProvider("HttpHeader", ParameterSources.Header,c => c.HttpRequest.HeaderParameters);
        public static IParameterProvider HttpUrl { get;} = new DictionaryParameterProvider("HttpUrl", ParameterSources.Url, c=>c.HttpRequest.UrlParameters);
        public static IParameterProvider HttpQuery { get;} = new DictionaryParameterProvider("HttpQuery", ParameterSources.Query, c=>c.HttpRequest.QueryParameters);
        public static IEnumerable<IParameterProvider> Http {
            get { return new[] {HttpHeader, HttpUrl, HttpQuery}; }
        }

        public static IEnumerable<IParameterProvider> Context {
            get { return new[] {HttpHeader, HttpUrl, HttpQuery, Variables}; }
        }

        public static IParameterProvider Variables { get; } = new DictionaryParameterProvider("Variables", ParameterSources.Custom, c=>c.Variables);
    }
}