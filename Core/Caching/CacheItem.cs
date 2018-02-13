namespace DotLogix.Core.Caching {
    public class CacheItem<TKey, TValue> {
        public TKey Key { get; }
        public TValue Value { get; }
        public ICachePolicy Policy { get; }

        public CacheItem(TKey key, TValue value, ICachePolicy policy) {
            Key = key;
            Value = value;
            Policy = policy;
        }
    }
}