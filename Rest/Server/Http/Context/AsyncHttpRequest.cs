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
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DotLogix.Core.Rest.Server.Http.Headers;
using DotLogix.Core.Rest.Server.Http.Parameters;
#endregion

namespace DotLogix.Core.Rest.Server.Http.Context {
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
}
