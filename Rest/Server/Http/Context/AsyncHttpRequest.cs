// ==================================================
// Copyright 2018(C) , DotLogix
// File:  AsyncHttpRequest.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DotLogix.Core.Rest.Server.Http.Mime;
using DotLogix.Core.Rest.Server.Http.Parameters;
#endregion

namespace DotLogix.Core.Rest.Server.Http.Context {
    public class AsyncHttpRequest : IAsyncHttpRequest {
        public AsyncHttpRequest(HttpListenerRequest originalRequest) {
            OriginalRequest = originalRequest;
            HeaderParameters = new ParameterCollection();
            QueryParameters = new ParameterCollection();
            UrlParameters = new ParameterCollection();
            UserDefinedParameters = new ParameterCollection();
            ContentType = new MimeType(originalRequest.ContentType);
            HttpMethod = AsyncHttpServer.HttpMethodFromString(originalRequest.HttpMethod);
            ContentLength64 = originalRequest.ContentLength64;
            ContentEncoding = originalRequest.ContentEncoding;
            InputStream = originalRequest.InputStream;
            InitializeParameters();
        }

        public ParameterCollection HeaderParameters { get; }
        public ParameterCollection QueryParameters { get; }
        public ParameterCollection UrlParameters { get; }
        public ParameterCollection UserDefinedParameters { get; }

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

        public bool TryFindParameter(string name, ParameterSources sources, out Parameter parameter) {
            if(((sources & ParameterSources.UserDefined) != 0) && UserDefinedParameters.TryGetParameter(name, out parameter))
                return true;
            if(((sources & ParameterSources.Query) != 0) && QueryParameters.TryGetParameter(name, out parameter))
                return true;
            if(((sources & ParameterSources.Url) != 0) && UrlParameters.TryGetParameter(name, out parameter))
                return true;
            if(((sources & ParameterSources.Header) != 0) && HeaderParameters.TryGetParameter(name, out parameter))
                return true;
            parameter = null;
            return false;
        }

        public Parameter FindParameter(string name, ParameterSources sources) {
            return TryFindParameter(name, sources, out var parameter) ? parameter : null;
        }

        private void InitializeParameters() {
            var queryString = OriginalRequest.QueryString;
            for(var i = 0; i < queryString.Count; i++) {
                var name = queryString.GetKey(i);
                var values = queryString.GetValues(i);
                QueryParameters.Add(new Parameter(name, values.AsEnumerable()));
            }
            var header = OriginalRequest.Headers;
            for(var i = 0; i < header.Count; i++) {
                var name = header.GetKey(i);
                var values = header.GetValues(i);
                HeaderParameters.Add(new Parameter(name, values.AsEnumerable()));
            }
        }
    }
}
