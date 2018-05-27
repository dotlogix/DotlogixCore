// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IAsyncHttpResponse.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DotLogix.Core.Rest.Server.Http.Mime;
using DotLogix.Core.Rest.Server.Http.Parameters;
using HttpStatusCode = DotLogix.Core.Rest.Server.Http.State.HttpStatusCode;
#endregion

namespace DotLogix.Core.Rest.Server.Http.Context {
    public interface IAsyncHttpResponse {
        TransferState TransferState { get; }
        bool IsSent { get; }
        ParameterCollection HeaderParameters { get; }
        MimeType ContentType { get; set; }
        long ContentLength64 { get; set; }
        int ChunkSize { get; set; }
        Encoding ContentEncoding { get; set; }
        HttpStatusCode StatusCode { get; set; }
        MemoryStream OutputStream { get; }
        HttpListenerResponse OriginalResponse { get; }
        Task WriteToResponseStreamAsync(byte[] data, int offset, int count);
        Task WriteToResponseStreamAsync(string message);
        Task SendChunksAsync();
        Task CompleteAsync();
    }
}
