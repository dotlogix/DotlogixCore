// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IWebServiceRoute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Services.Processors;
using DotLogix.Core.Rest.Services.Writer;
#endregion

namespace DotLogix.Core.Rest.Server.Routes {
    public interface IWebServiceRoute {
        WebRequestProcessorCollection PreProcessors { get; }
        WebRequestProcessorCollection PostProcessors { get; }
        IWebRequestProcessor RequestProcessor { get; }
        IAsyncWebRequestResultWriter WebRequestResultWriter { get; set; }


        int RouteIndex { get; }
        int Priority { get; }
        HttpMethods AcceptedRequests { get; }
        RouteMatch Match(HttpMethods method, string path);
    }
}
