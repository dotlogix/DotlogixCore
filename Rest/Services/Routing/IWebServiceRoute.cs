// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IWebServiceRoute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Core.Rest.Events;
using DotLogix.Core.Rest.Http;
using DotLogix.Core.Rest.Services.Descriptors;
using DotLogix.Core.Rest.Services.Processors;
using DotLogix.Core.Rest.Services.ResultWriters;
using DotLogix.Core.Rest.Services.Routing.Matching;

#endregion

namespace DotLogix.Core.Rest.Services.Routing {
    public interface IWebServiceRoute {
        ParameterProviderCollection ParameterProviders { get; }
        RouteDescriptorCollection Descriptors { get; }
        WebRequestProcessorCollection PreProcessors { get; }
        WebRequestProcessorCollection PostProcessors { get; }
        IWebRequestProcessor RequestProcessor { get; }
        IWebServiceResultWriter ResultWriter { get; }


        int RouteIndex { get; }
        string Pattern { get; }
        int Priority { get; }
        bool IsRooted { get; }
        HttpMethods AcceptedRequests { get; }
        RouteMatch Match(HttpMethods method, string path);
    }
}
