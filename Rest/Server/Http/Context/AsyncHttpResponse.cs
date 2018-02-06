// ==================================================
// Copyright 2018(C) , DotLogix
// File:  AsyncHttpResponse.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  29.01.2018
// LastEdited:  31.01.2018
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
using DotLogix.Core.Rest.Server.Http.State;
using HttpStatusCode = DotLogix.Core.Rest.Server.Http.State.HttpStatusCode;
#endregion

namespace DotLogix.Core.Rest.Server.Http.Context {
    public class AsyncHttpResponse : IAsyncHttpResponse {
        public AsyncHttpResponse(HttpListenerResponse originalResponse) {
            OriginalResponse = originalResponse ?? throw new ArgumentNullException(nameof(originalResponse));
            ContentType = MimeTypes.PlainText;
            ContentLength64 = 0;
            ContentEncoding = Encoding.UTF8;
            StatusCode = HttpStatusCodes.Ok;
            OutputStream = new MemoryStream();
            HeaderParameters = new ParameterCollection();
            InitializeParameters();
        }

        public bool IsSent { get; private set; }

        public ParameterCollection HeaderParameters { get; }

        public MimeType ContentType { get; set; }
        public long ContentLength64 { get; set; }
        public Encoding ContentEncoding { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public MemoryStream OutputStream { get; }
        public HttpListenerResponse OriginalResponse { get; }

        public async Task WriteToResponseStreamAsync(byte[] data, int offset, int count) {
            await OutputStream.WriteAsync(data, offset, count);
        }

        public async Task WriteToResponseStreamAsync(string message) {
            using(var writer = new StreamWriter(OutputStream, ContentEncoding, 1024, true))
                await writer.WriteAsync(message);
        }

        public async Task SendAsync(HttpStatusCode statusCode = null, MimeType contentType = null,
                                    Encoding contentEncoding = null) {
            if(IsSent)
                return;
            IsSent = true;
            if(statusCode != null)
                StatusCode = statusCode;
            if(contentType != null)
                ContentType = contentType;
            if(contentEncoding != null)
                ContentEncoding = contentEncoding;


            PrepareHeaders();
            OriginalResponse.ContentLength64 = OutputStream.Length;
            OriginalResponse.StatusCode = StatusCode.Code;
            OriginalResponse.StatusDescription = StatusCode.Description;
            OriginalResponse.ContentType = ContentType.Code;
            OriginalResponse.ContentEncoding = ContentEncoding;

            if(OutputStream.Length > 0) {
                OutputStream.Seek(0, SeekOrigin.Begin);
                await OutputStream.CopyToAsync(OriginalResponse.OutputStream);
            }
            OutputStream.Dispose();
            OriginalResponse.OutputStream.Close();
        }

        private void InitializeParameters() {
            var header = OriginalResponse.Headers;
            for(var i = 0; i < header.Count; i++) {
                var name = header.GetKey(i);
                var values = header.GetValues(i);
                HeaderParameters.Add(new Parameter(name, values.AsEnumerable()));
            }
        }

        private void PrepareHeaders() {
            var header = new WebHeaderCollection();
            foreach(var parameter in HeaderParameters) {
                foreach(var value in parameter.Values)
                    header.Add(parameter.Name, value.ToString());
            }
            OriginalResponse.Headers = header;
        }
    }
}
