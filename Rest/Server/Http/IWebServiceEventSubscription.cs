using System;
using DotLogix.Core.Rest.Server.Http.Context;
using DotLogix.Core.Rest.Server.Routes;

namespace DotLogix.Core.Rest.Server.Http {
    public interface IWebServiceEventSubscription {
        Guid Guid { get; }
        IAsyncHttpContext Context { get; }
        IWebServiceRoute Route { get; }
        AsyncWebRequestRouter Router { get; }

        bool CheckPreCondition(object sender, WebServiceEventArgs eventArgs);
    }
}