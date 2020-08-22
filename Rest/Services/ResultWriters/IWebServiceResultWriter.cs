// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IAsyncWebRequestResultWriter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region

using System.Threading.Tasks;

#endregion

namespace DotLogix.Core.Rest.Services.ResultWriters {
    public interface IWebServiceResultWriter {
        Task WriteAsync(WebServiceContext context);
    }
}
