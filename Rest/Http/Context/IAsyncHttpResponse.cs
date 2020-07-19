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
using DotLogix.Core.Rest.Http.Headers;
#endregion

namespace DotLogix.Core.Rest.Http.Context {
    public interface IAsyncHttpResponse {
        TransferState TransferState { get; }
        bool IsCompleted { get; }
        IDictionary<string, object> HeaderParameters { get; }
        ICollection<Cookie> Cookies { get; set; }
        MimeType ContentType { get; set; }
        long ContentLength { get; set; }
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
