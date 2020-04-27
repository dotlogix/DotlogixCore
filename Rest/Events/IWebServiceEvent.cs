// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IWebServiceEvent.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  05.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Threading.Tasks;
using DotLogix.Core.Rest.Http.Context;
using DotLogix.Core.Rest.Services.Routing;
#endregion

namespace DotLogix.Core.Rest.Events {
    public interface IWebServiceEvent {
        string Name { get; }

        void Subscribe(IAsyncHttpContext context, IWebServiceRoute route, WebServiceRouter router);
        Task DispatchAsync(object sender, WebServiceEventArgs eventArgs);
    }
}
