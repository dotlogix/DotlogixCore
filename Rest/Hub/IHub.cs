using System;
using System.Collections.Generic;
using DotLogix.Core.Rest.Events;

namespace DotLogix.Core.Rest.Server.Http.WebSockets {
    public interface IHub {
        IReadOnlyDictionary<Guid, IHubClient> Clients { get; }
    }
}