// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IWebRequestProcessor.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Threading.Tasks;
using DotLogix.Core.Rest.Services.Descriptors;
#endregion

namespace DotLogix.Core.Rest.Services.Processors {
    public interface IWebRequestProcessor {
        int Priority { get; }
        Task ProcessAsync(WebServiceContext context);
        bool ShouldExecute(WebServiceContext webServiceContext);
    }
}
