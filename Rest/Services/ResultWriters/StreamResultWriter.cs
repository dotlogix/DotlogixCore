// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebRequestResultStreamWriter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  28.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region using
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DotLogix.Core.Rest.Http;
#endregion

namespace DotLogix.Core.Rest.Services {
    public class StreamResultWriter : PrimitiveResultWriter {
        public static IWebServiceResultWriter Instance { get; } = new StreamResultWriter();
        protected StreamResultWriter() { }

        /// <inheritdoc />
        public override async Task WriteAsync(WebServiceContext context) {
            var response = context.HttpResponse;
            if(response.IsCompleted)
                return;

            if(context.Result.Exception.IsDefined) {
                await WriteExceptionAsync(context, context.Result.Exception.Value);
                return;
            }

            var requestResult = context.Result as IWebServiceObjectResult;
            if(requestResult == null)
                throw new ArgumentException($"The provided value type {(context.Result != null ? context.Result.GetType().Name : "null")} is not supported. This result writer accepts only values of type \"{nameof(IWebServiceObjectResult)}\"");

            if(requestResult.ReturnValue.IsUndefined) {
                await response.CompleteAsync();
                return;
            }

            var returnValue = requestResult.ReturnValue.Value;
            WebServiceStreamResult contentResult;
            switch(returnValue) {
                case WebServiceStreamResult streamResult:
                    contentResult = await GetStreamResult(streamResult);
                    break;
                case Stream stream:
                    contentResult = await GetStreamResult(stream);
                    break;
                default:
                    await base.WriteResultAsync(context, returnValue);
                    await response.CompleteAsync();
                    return;
            }

            await WriteResultAsync(context, contentResult);
            await response.CompleteAsync();
            return;
        }

        protected virtual async Task WriteResultAsync(WebServiceContext context, WebServiceStreamResult streamResult) {
            var httpResponse = context.HttpResponse;
            httpResponse.StatusCode = streamResult.StatusCode ?? HttpStatusCodes.Success.Ok;
            httpResponse.ContentType = streamResult.ContentType ?? MimeTypes.Application.OctetStream;
            
            if(streamResult.Stream == null) {
                if(streamResult.StatusCode == null) {
                    httpResponse.StatusCode = HttpStatusCodes.Success.NoContent;
                }
                return;
            }


            try {
                if(streamResult.SendInChunks) {
                    var chunkSize = streamResult.ChunkSize;
                    httpResponse.ChunkSize = chunkSize;
                    var buffer = new byte[chunkSize];
                    int read;
                    while((read = await streamResult.Stream.ReadAsync(buffer, 0, chunkSize)) > 0) {
                        await httpResponse.WriteToResponseStreamAsync(buffer, 0, read);
                        await httpResponse.CompleteChunksAsync();
                    }
                } else
                    await streamResult.Stream.CopyToAsync(httpResponse.OutputStream);
            } finally {
                streamResult.Stream?.Dispose();
            }
        }

        protected async Task<WebServiceStreamResult> GetStreamResult(WebServiceStreamResult streamResult) {
            switch(streamResult.TransportMode) {
                case TransportModes.Base64:
                    if(streamResult.SendInChunks)
                        throw new InvalidOperationException("Chunked streaming is not supported in base64 mode");

                    string base64String;
                    using(var stream = new MemoryStream()) {
                        await streamResult.Stream.CopyToAsync(stream);
                        streamResult.Stream.Dispose();
                        base64String = Convert.ToBase64String(stream.GetBuffer(), 0, (int)stream.Length);
                    }

                    var memStream = new MemoryStream(Encoding.UTF8.GetBytes(base64String));
                    memStream.Seek(0, SeekOrigin.Begin);

                    streamResult.Stream = memStream;
                    streamResult.ContentType ??= MimeTypes.Application.OctetStream;
                    streamResult.Stream = memStream;
                    return streamResult;
                case TransportModes.Raw:
                    return streamResult;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected virtual Task<WebServiceStreamResult> GetStreamResult(Stream stream) {
            return Task.FromResult(new WebServiceStreamResult {
                                                              Stream = stream,
                                                              ContentType = MimeTypes.Application.OctetStream
                                                              });
        }
    }
}
