// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebRequestStreamResult.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  28.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region

using System;
using System.IO;
using DotLogix.Core.Rest.Http;
using DotLogix.Core.Rest.Http.Context;
using DotLogix.Core.Rest.Http.Headers;
using DotLogix.Core.Rest.Services.ResultWriters;

#endregion

namespace DotLogix.Core.Rest.Services.Results {
    public class WebServiceStreamResult : IWebServiceResult {
        public Stream Stream { get; set; }
        public int ChunkSize { get; set; } = AsyncHttpResponse.DefaultChunkSize;
        public bool SendInChunks { get; set; }
        public TransportModes TransportMode { get; set; } = TransportModes.Raw;
        
        /// <inheritdoc />
        public HttpStatusCode StatusCode { get; set; }

        /// <inheritdoc />
        public MimeType ContentType { get; set; }

        /// <inheritdoc />
        public IWebServiceResultWriter ResultWriter { get; set; } = StreamResultWriter.Instance;

        /// <inheritdoc />
        public Optional<Exception> Exception => default;
    }
}
