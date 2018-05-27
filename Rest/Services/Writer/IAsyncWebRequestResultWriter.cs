// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IWebRequestResultWriter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Threading.Tasks;
using DotLogix.Core.Rest.Server.Http;
#endregion

namespace DotLogix.Core.Rest.Services.Writer {
    public interface IAsyncWebRequestResultWriter {
        Task WriteAsync(WebRequestResult requestResult);
    }
}
