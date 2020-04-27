namespace DotLogix.Core.Rest.Events {
    public enum HubMessageType {
        Empty,
        Connect,
        Disconnect,

        Text,
        Binary,

        Custom
    }
}