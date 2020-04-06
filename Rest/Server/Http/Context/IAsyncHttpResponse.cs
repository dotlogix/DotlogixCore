// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IAsyncHttpResponse.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DotLogix.Core.Rest.Server.Http.Headers;
using HttpStatusCode = DotLogix.Core.Rest.Server.Http.State.HttpStatusCode;
#endregion

namespace DotLogix.Core.Rest.Server.Http.Context {
    public interface IAsyncHttpResponse {
        TransferState TransferState { get; }
        bool IsCompleted { get; }
        IDictionary<string, object> HeaderParameters { get; }
        MimeType ContentType { get; set; }
        long ContentLength64 { get; set; }
        int ChunkSize { get; set; }
        Encoding ContentEncoding { get; set; }
        HttpStatusCode StatusCode { get; set; }
        Stream OutputStream { get; }
        object OriginalResponse { get; }
        Task WriteToResponseStreamAsync(byte[] data, int offset, int count);
        Task WriteToResponseStreamAsync(string message);
        Task CompleteChunksAsync();
        Task CompleteAsync();
    }
}
