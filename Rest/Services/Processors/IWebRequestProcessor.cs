// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IWebRequestProcessor.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Threading.Tasks;
using DotLogix.Core.Rest.Services.Context;
using DotLogix.Core.Rest.Services.Descriptors;
#endregion

namespace DotLogix.Core.Rest.Services.Processors {
    public interface IWebRequestProcessor {
        WebRequestProcessorDescriptorCollection Descriptors { get; }
        int Priority { get; }
        Task ProcessAsync(WebServiceContext webServiceContext);
        bool ShouldExecute(WebServiceContext webServiceContext);
    }
}
