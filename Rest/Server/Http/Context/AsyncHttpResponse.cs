// ==================================================
// Copyright 2018(C) , DotLogix
// File:  AsyncHttpResponse.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.02.2018
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
using DotLogix.Core.Rest.Server.Http.State;
using HttpStatusCode = DotLogix.Core.Rest.Server.Http.State.HttpStatusCode;
#endregion

namespace DotLogix.Core.Rest.Server.Http.Context {
    public class AsyncHttpResponse : IAsyncHttpResponse {
        public const int DefaultChunkSize = 2_097_152; // 2MiB;

        public AsyncHttpResponse(HttpListenerResponse originalResponse) {
            OriginalResponse = originalResponse ?? throw new ArgumentNullException(nameof(originalResponse));
            ContentType = MimeTypes.Text.Plain;
            ContentLength64 = 0;
            ContentEncoding = Encoding.UTF8;
            StatusCode = HttpStatusCodes.Success.Ok;
            OutputStream = new MemoryStream();
            HeaderParameters = new ParameterCollection();
            ChunkSize = DefaultChunkSize;
            InitializeParameters();
        }

        public TransferState TransferState { get; private set; }
        public bool IsSent => TransferState != TransferState.None;

        public ParameterCollection HeaderParameters { get; }

        public MimeType ContentType { get; set; }
        public long ContentLength64 { get; set; }
        public int ChunkSize { get; set; }
        public Encoding ContentEncoding { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public MemoryStream OutputStream { get; private set; }
        public HttpListenerResponse OriginalResponse { get; }

        public async Task WriteToResponseStreamAsync(byte[] data, int offset, int count) {
            await OutputStream.WriteAsync(data, offset, count);
        }

        public async Task WriteToResponseStreamAsync(string message) {
            using(var writer = new StreamWriter(OutputStream, ContentEncoding, 1024, true))
                await writer.WriteAsync(message);
        }

        public async Task SendChunksAsync()
        {
            if (TransferState == TransferState.Completed)
                throw new InvalidOperationException("Sending is not possible if the transfer has been completed already");

            if(TransferState == TransferState.None) {
                OriginalResponse.SendChunked = true;
                EnsurePrepared();
            }

            if (OutputStream.Length > 0)
            {
                OutputStream.Seek(0, SeekOrigin.Begin);
                await OutputStream.CopyToAsync(OriginalResponse.OutputStream, ChunkSize);
                OutputStream.SetLength(0);
            }
            TransferState = TransferState.Started;
        }

        public async Task CompleteAsync() {
            if (TransferState == TransferState.Completed)
                throw new InvalidOperationException("Sending is not possible if the transfer has been completed already");

            if(OriginalResponse.SendChunked) {
                await SendChunksAsync();
            } else {
                if (TransferState == TransferState.None) {
                    EnsurePrepared();
                }
                if (OutputStream.Length > 0)
                {
                    OutputStream.Seek(0, SeekOrigin.Begin);
                    await OutputStream.CopyToAsync(OriginalResponse.OutputStream, 81920);
                }
            }

            OutputStream.Dispose();
            OriginalResponse.OutputStream.Close();
            TransferState = TransferState.Completed;
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

        private void EnsurePrepared() {
            if(TransferState != TransferState.None)
                return;

            PrepareHeaders();
            OriginalResponse.StatusCode = StatusCode.Code;
            OriginalResponse.StatusDescription = StatusCode.Description;
            OriginalResponse.ContentType = ContentType.Code;
            OriginalResponse.ContentEncoding = ContentEncoding;
            if (OriginalResponse.SendChunked == false)
                OriginalResponse.ContentLength64 = OutputStream.Length;
        }
    }
}
