// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebRequestStreamResult.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  28.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.IO;
using DotLogix.Core.Rest.Server.Http.Context;
using DotLogix.Core.Rest.Server.Http.Headers;
#endregion

namespace DotLogix.Core.Rest.Services.Writer {
    public class WebRequestStreamResult {
        public Stream OutputStream { get; }
        public MimeType ContentType { get; }
        public int ChunkSize { get; }
        public bool SendInChunks { get; }
        public TransportModes TransportMode { get; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public WebRequestStreamResult(Stream outputStream, MimeType contentType, bool sendInChunks = false, int chunkSize = AsyncHttpResponse.DefaultChunkSize, TransportModes transportMode = TransportModes.Raw) {
            OutputStream = outputStream;
            ContentType = contentType;
            ChunkSize = chunkSize;
            TransportMode = transportMode;
            SendInChunks = sendInChunks;
        }
    }
}
