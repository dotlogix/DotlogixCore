namespace DotLogix.Core.Caching {
    public class CacheItem<TKey> {
        public TKey Key { get; }
        public object Value { get; }
        public ICachePolicy Policy { get; }

        public CacheItem(TKey key, object value, ICachePolicy policy) {
            Key = key;
            Value = value;
            Policy = policy;
        }
    }
}