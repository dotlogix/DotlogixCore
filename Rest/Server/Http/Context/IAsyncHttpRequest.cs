// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IAsyncHttpRequest.cs
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
#endregion

namespace DotLogix.Core.Rest.Server.Http.Context {
    public interface IAsyncHttpRequest {
        IDictionary<string, object> HeaderParameters { get; }
        IDictionary<string, object> QueryParameters { get; }
        IDictionary<string, object> UrlParameters { get; }
        Uri Url { get; }
        HttpMethods HttpMethod { get; }
        MimeType ContentType { get; }
        long ContentLength64 { get; }
        Encoding ContentEncoding { get; }
        bool HasBody { get; }
        Stream InputStream { get; }
        object OriginalRequest { get; }
        Task<int> ReadDataFromRequestStreamAsync(byte[] data, int offset, int count);
        Task<byte[]> ReadDataFromRequestStreamAsync();
        Task<string> ReadStringFromRequestStreamAsync();
    }
}
