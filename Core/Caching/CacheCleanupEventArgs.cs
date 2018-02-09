using System;
using System.Collections.Generic;

namespace DotLogix.Core.Caching {
    public class CacheCleanupEventArgs : EventArgs {
        public IReadOnlyCollection<object> RemovedItems { get; }

        public CacheCleanupEventArgs(IReadOnlyCollection<object> removedItems) {
            RemovedItems = removedItems;
        }
    }
}