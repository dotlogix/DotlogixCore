// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IWebServiceEvent.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  05.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Threading.Tasks;
using DotLogix.Core.Rest.Server.Http.Context;
using DotLogix.Core.Rest.Server.Routes;
#endregion

namespace DotLogix.Core.Rest.Server.Http {
    public interface IWebServiceEvent {
        string Name { get; }

        void Subscribe(IAsyncHttpContext context, IWebServiceRoute route, AsyncWebRequestRouter router);
        Task TriggerAsync(object sender, WebServiceEventArgs eventArgs);
    }
}
