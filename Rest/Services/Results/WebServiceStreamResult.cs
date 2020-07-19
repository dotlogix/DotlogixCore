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
using DotLogix.Core.Rest.Http.Context;
#endregion

namespace DotLogix.Core.Rest.Services {
    public class WebServiceStreamResult : WebServiceResult, IWebServiceObjectResult<WebServiceStreamResult> {
        public Stream Stream { get; set; }
        public int ChunkSize { get; set; } = AsyncHttpResponse.DefaultChunkSize;
        public bool SendInChunks { get; set; }
        public TransportModes TransportMode { get; set; } = TransportModes.Raw;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public WebServiceStreamResult() {
            ResultWriter = StreamResultWriter.Instance;
        }

        Optional<WebServiceStreamResult> IWebServiceObjectResult<WebServiceStreamResult>.ReturnValue => this;
        Optional<object> IWebServiceObjectResult.ReturnValue => this;
        Type IWebServiceObjectResult.ReturnType => GetType();
    }
}
