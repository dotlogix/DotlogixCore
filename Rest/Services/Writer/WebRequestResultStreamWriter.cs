// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebRequestResultStreamWriter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  28.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DotLogix.Core.Extensions;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Server.Http.Context;
using DotLogix.Core.Rest.Server.Http.Headers;
using DotLogix.Core.Rest.Server.Http.State;
using DotLogix.Core.Rest.Services.Context;
#endregion

namespace DotLogix.Core.Rest.Services.Writer {
    public class WebRequestResultStreamWriter : WebRequestResultWriterBase {
        public static IAsyncWebRequestResultWriter Instance { get; } = new WebRequestResultStreamWriter();
        protected WebRequestResultStreamWriter() { }

        protected override async Task WriteResultAsync(WebServiceContext context) {
            var httpResponse = context.HttpResponse;
            var requestResult = context.RequestResult;

            var returnValue = requestResult.ReturnValue;
            if(returnValue == null) {
                httpResponse.StatusCode = HttpStatusCodes.Success.NoContent;
                return;
            }

            var result = await GetStreamResult(requestResult);
            httpResponse.ContentType = result.ContentType;
            try {
                if(result.SendInChunks) {
                    var chunkSize = result.ChunkSize;
                    httpResponse.ChunkSize = chunkSize;
                    var buffer = new byte[chunkSize];
                    int read;
                    while((read = await result.OutputStream.ReadAsync(buffer, 0, chunkSize)) > 0) {
                        await httpResponse.WriteToResponseStreamAsync(buffer, 0, read);
                        await httpResponse.CompleteChunksAsync();
                    }
                } else
                    await result.OutputStream.CopyToAsync(httpResponse.OutputStream);
            } finally {
                result.OutputStream?.Dispose();
            }
        }

        protected virtual async Task<WebRequestStreamResult> GetStreamResult(WebRequestResult webRequestResult) {
            WebRequestStreamResult result;
            switch(webRequestResult.ReturnValue) {
                case Stream stream:
                    result = new WebRequestStreamResult(stream, MimeTypes.Application.OctetStream);
                    break;
                case WebRequestStreamResult streamResult:
                    switch(streamResult.TransportMode) {
                        case TransportModes.Base64:
                            if(streamResult.SendInChunks)
                                throw new InvalidOperationException("Chunked streaming is not supported in base64 mode");

                            string base64String;
                            using(var stream = new MemoryStream()) {
                                await streamResult.OutputStream.CopyToAsync(stream);
                                streamResult.OutputStream.Dispose();
                                base64String = Convert.ToBase64String(stream.GetBuffer(), 0, (int)stream.Length);
                            }

                            var memStream = new MemoryStream(Encoding.UTF8.GetBytes(base64String));
                            memStream.Seek(0, SeekOrigin.Begin);
                            return new WebRequestStreamResult(memStream, MimeTypes.Application.OctetStream);
                        case TransportModes.Raw:
                            result = streamResult;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                default:
                    throw new InvalidOperationException($"Can not convert {webRequestResult.ReturnValue.GetType().GetFriendlyName()} to {nameof(WebRequestResult)}");
            }

            return result;
        }
    }
}
