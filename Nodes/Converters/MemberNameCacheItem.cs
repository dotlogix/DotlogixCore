namespace DotLogix.Core.Nodes.Converters {
    public class MemberNameCacheItem
    {
        public MemberSettings Settings { get; }
        public bool Serialize { get; }
        public bool Use { get; }
        public bool UseInCtor { get; }


        public string RewrittenName { get; }
    }
}