using System;

namespace DotLogix.Core.Rest.Events {
    public interface IHubMessage {
        Guid Guid { get; }
        string Type { get; }
        DateTime TimestampUtc { get; }
        object Payload { get; }
    }
}