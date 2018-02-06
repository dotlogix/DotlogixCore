// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IWebRequestProcessor.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  31.01.2018
// LastEdited:  31.01.2018
// ==================================================

#region
using System.Threading.Tasks;
using DotLogix.Core.Rest.Server.Http;
#endregion

namespace DotLogix.Core.Rest.Services.Processors {
    public interface IWebRequestProcessor {
        int Priority { get; }
        bool IgnoreHandled { get; }
        Task ProcessAsync(WebRequestResult webRequestResult);
    }
}
