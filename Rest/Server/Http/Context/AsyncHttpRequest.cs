// ==================================================
// Copyright 2018(C) , DotLogix
// File:  AsyncHttpRequest.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DotLogix.Core.Rest.Server.Http.Mime;
#endregion

namespace DotLogix.Core.Rest.Server.Http.Context {
    public interface IParameterParser {
        IDictionary<string, object> Deserialize(NameValueCollection collection);
        TCollection Serialize<TCollection>(IDictionary<string, object> parameters) where TCollection : NameValueCollection, new();
        void DeserializeValue(string name, string[] values, IDictionary<string, object> parameters);
        void SerializeValue(string name, object value, NameValueCollection collection);
    }

    public class ExtendedParameterParser : ParameterParserBase {
        public IParameterParser FallBackParser { get; }
        public Dictionary<string, IParameterParser> SpecializedParsers { get; } = new Dictionary<string, IParameterParser>();
        public static ExtendedParameterParser Default => CreateDefaultParser();

        public ExtendedParameterParser(IParameterParser fallBackParser = null) {
            FallBackParser = fallBackParser ?? PrimitiveParameterParser.Instance;
        }

        private static ExtendedParameterParser CreateDefaultParser() {
            return new ExtendedParameterParser();
        }

        public override void DeserializeValue(string name, string[] values, IDictionary<string, object> parameters) {
            if(SpecializedParsers.TryGetValue(name, out var parameterParser) == false)
                parameterParser = FallBackParser;
            parameterParser.DeserializeValue(name, values, parameters);
        }

        public override void SerializeValue(string name, object value, NameValueCollection collection) {
            if(SpecializedParsers.TryGetValue(name, out var parameterParser) == false)
                parameterParser = FallBackParser;
            parameterParser.SerializeValue(name, value, collection);
        }
    }

    public class AsyncHttpRequest : IAsyncHttpRequest {
        private AsyncHttpRequest(HttpListenerRequest originalRequest, IDictionary<string, object> headerParameters, IDictionary<string, object> queryParameters) {
            OriginalRequest = originalRequest;
            HeaderParameters = headerParameters;
            QueryParameters = queryParameters;
            UrlParameters = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            UserDefinedParameters = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            ContentType = MimeType.Parse(originalRequest.ContentType);
            HttpMethod = AsyncHttpServer.HttpMethodFromString(originalRequest.HttpMethod);
            ContentLength64 = originalRequest.ContentLength64;
            ContentEncoding = originalRequest.ContentEncoding;
            InputStream = originalRequest.InputStream;
        }

        public IDictionary<string, object> HeaderParameters { get; }
        public IDictionary<string, object> QueryParameters { get; }
        public IDictionary<string, object> UrlParameters { get; }
        public IDictionary<string, object> UserDefinedParameters { get; }

        public Uri Url => OriginalRequest.Url;
        public HttpMethods HttpMethod { get; }
        public MimeType ContentType { get; }
        public long ContentLength64 { get; }
        public Encoding ContentEncoding { get; }
        public Stream InputStream { get; }

        public HttpListenerRequest OriginalRequest { get; }

        public virtual async Task<int> ReadDataFromRequestStreamAsync(byte[] data, int offset, int count) {
            return await InputStream.ReadAsync(data, offset, count);
        }

        public virtual async Task<byte[]> ReadDataFromRequestStreamAsync() {
            using(var memoryStream = new MemoryStream()) {
                await InputStream.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public virtual async Task<string> ReadStringFromRequestStreamAsync() {
            var data = await ReadDataFromRequestStreamAsync();
            return ContentEncoding.GetString(data);
        }

        public static AsyncHttpRequest Create(HttpListenerRequest originalRequest, IParameterParser parameterParser) {
            var headerParameters = parameterParser.Deserialize(originalRequest.Headers);
            var queryParameters = parameterParser.Deserialize(originalRequest.QueryString);
            return new AsyncHttpRequest(originalRequest, headerParameters, queryParameters);
        }
    }


    public abstract class ParameterParserBase : IParameterParser {
        public IDictionary<string, object> Deserialize(NameValueCollection collection) {
            var parameters = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            for(var i = 0; i < collection.Count; i++) {
                var name = collection.GetKey(i);
                var values = collection.GetValues(i);

                DeserializeValue(name, values, parameters);
            }
            return parameters;
        }

        public TCollection Serialize<TCollection>(IDictionary<string, object> parameters) where TCollection : NameValueCollection, new() {
            var collection = new TCollection();
            foreach(var parameter in parameters) {
                SerializeValue(parameter.Key, parameter.Value, collection);
            }
            return collection;
        }

        public abstract void DeserializeValue(string name, string[] values, IDictionary<string, object> parameters);
        public abstract void SerializeValue(string name, object value, NameValueCollection collection);
    }

    public class PrimitiveParameterParser : ParameterParserBase {
        public static IParameterParser Instance { get; } = new PrimitiveParameterParser();
        private PrimitiveParameterParser() { }

        public override void DeserializeValue(string name, string[] values, IDictionary<string, object> parameters) {
            if(values == null)
                return;

            switch(values.Length) {
                case 0:
                    parameters.Add(name, null);
                    break;
                case 1:
                    parameters.Add(name, values[0]);
                    break;
                default:
                    parameters.Add(name, values);
                    break;
            }
        }

        public override void SerializeValue(string name, object value, NameValueCollection collection) {
            if(!(value is Array array))
            {
                collection.Add(name, value.ToString());
                return;
            }

            foreach(var child in array)
                collection.Add(name, child.ToString());
        }
    }
}
