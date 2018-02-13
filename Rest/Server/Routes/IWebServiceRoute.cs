// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IWebServiceRoute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  29.01.2018
// LastEdited:  31.01.2018
// ==================================================

#region
using System.Collections.Generic;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Services.Processors;
#endregion

namespace DotLogix.Core.Rest.Server.Routes {
    public interface IWebServiceRoute {
        IReadOnlyCollection<IWebRequestProcessor> PreProcessors { get; }
        IReadOnlyCollection<IWebRequestProcessor> PostProcessors { get; }
        IWebRequestProcessor RequestProcessor { get; }
        IWebRequestResultWriter WebRequestResultWriter { get; }


        int RouteIndex { get; }
        int Priority { get; }
        HttpMethods AcceptedRequests { get; }
        void AddPreProcessor(IWebRequestProcessor preProcessor);
        void RemovePreProcessor(IWebRequestProcessor preProcessor);
        void AddPostProcessor(IWebRequestProcessor postProcessor);
        void RemovePostProcessor(IWebRequestProcessor postProcessor);
        RouteMatch Match(HttpMethods method, string path);
    }
}
