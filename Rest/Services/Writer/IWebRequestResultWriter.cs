// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IWebRequestResultWriter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Threading.Tasks;
#endregion

namespace DotLogix.Core.Rest.Server.Http {
    public interface IWebRequestResultWriter {
        Task WriteAsync(WebRequestResult requestResult);
    }
}
