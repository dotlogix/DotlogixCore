// ==================================================
// Copyright 2018(C) , DotLogix
// File:  AsyncHttpResponse.cs
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
using DotLogix.Core.Rest.Server.Http.State;
using HttpStatusCode = DotLogix.Core.Rest.Server.Http.State.HttpStatusCode;
#endregion

namespace DotLogix.Core.Rest.Server.Http.Context {
    public class AsyncHttpResponse : IAsyncHttpResponse {
        public const int DefaultChunkSize = 2_097_152; // 2MiB;
        private readonly IParameterParser _parameterParser;

        private AsyncHttpResponse(HttpListenerResponse originalResponse, IParameterParser parameterParser) {
            _parameterParser = parameterParser;
            OriginalResponse = originalResponse ?? throw new ArgumentNullException(nameof(originalResponse));
            ContentType = MimeTypes.Text.Plain;
            ContentLength64 = 0;
            ContentEncoding = Encoding.UTF8;
            StatusCode = HttpStatusCodes.Success.Ok;
            OutputStream = new MemoryStream();
            HeaderParameters = parameterParser.Deserialize(originalResponse.Headers);
            ChunkSize = DefaultChunkSize;
        }

        public TransferState TransferState { get; private set; }
        public bool IsCompleted => TransferState == TransferState.Completed;

        public IDictionary<string, object> HeaderParameters { get; }

        public MimeType ContentType { get; set; }
        public long ContentLength64 { get; set; }
        public int ChunkSize { get; set; }
        public Encoding ContentEncoding { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public Stream OutputStream { get; }
        public HttpListenerResponse OriginalResponse { get; }

        public async Task WriteToResponseStreamAsync(byte[] data, int offset, int count) {
            await OutputStream.WriteAsync(data, offset, count);
        }

        public async Task WriteToResponseStreamAsync(string message) {
            using(var writer = new StreamWriter(OutputStream, ContentEncoding, 1024, true))
                await writer.WriteAsync(message);
        }

        public async Task CompleteChunksAsync() {
            if(TransferState == TransferState.Completed)
                throw new InvalidOperationException("Sending is not possible if the transfer has been completed already");

            if(TransferState == TransferState.None) {
                OriginalResponse.SendChunked = true;
                EnsurePrepared();
            }

            if(OutputStream.Length > 0) {
                OutputStream.Seek(0, SeekOrigin.Begin);
                await OutputStream.CopyToAsync(OriginalResponse.OutputStream, ChunkSize);
                OutputStream.SetLength(0);
            }
            TransferState = TransferState.Started;
        }

        public async Task CompleteAsync() {
            if(TransferState == TransferState.Completed)
                throw new InvalidOperationException("Sending is not possible if the transfer has been completed already");

            if(OriginalResponse.SendChunked)
                await CompleteChunksAsync();
            else {
                if(TransferState == TransferState.None)
                    EnsurePrepared();
                if(OutputStream.Length > 0) {
                    OutputStream.Seek(0, SeekOrigin.Begin);
                    await OutputStream.CopyToAsync(OriginalResponse.OutputStream, 81920);
                }
            }

            OutputStream.Dispose();
            OriginalResponse.OutputStream.Close();
            TransferState = TransferState.Completed;
        }

        private void PrepareHeaders() {
            OriginalResponse.Headers = _parameterParser.Serialize<WebHeaderCollection>(HeaderParameters);
        }

        private void EnsurePrepared() {
            if(TransferState != TransferState.None)
                return;

            PrepareHeaders();
            OriginalResponse.StatusCode = StatusCode.Code;
            OriginalResponse.StatusDescription = StatusCode.Description;
            OriginalResponse.ContentType = ContentType.Value;
            OriginalResponse.ContentEncoding = ContentEncoding;
            if(OriginalResponse.SendChunked == false)
                OriginalResponse.ContentLength64 = OutputStream.Length;
        }

        public static AsyncHttpResponse Create(HttpListenerResponse originalResponse, IParameterParser parameterParser) {
            return new AsyncHttpResponse(originalResponse, parameterParser);
        }
    }
}
