using System.Collections.Generic;

namespace DotLogix.Core.Caching {
    public class CacheItemsDiscardedEventArgs<TKey, TValue> {
        public CacheItemsDiscardedEventArgs(IReadOnlyList<CacheItem<TKey, TValue>> discardedItems) {
            DiscardedItems = discardedItems;
        }
        public IReadOnlyList<CacheItem<TKey, TValue>> DiscardedItems { get; }
    }
}