// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IWebServiceEventSubscription.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  05.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Core.Rest.Http.Context;
using DotLogix.Core.Rest.Services.Routing;
#endregion

namespace DotLogix.Core.Rest.Events {
    public interface IWebServiceEventSubscription {
        Guid Guid { get; }
        IAsyncHttpContext Context { get; }
        IWebServiceRoute Route { get; }
        WebServiceRouter Router { get; }

        bool CheckPreCondition(object sender, WebServiceEventArgs eventArgs);
    }
}
