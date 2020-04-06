namespace DotLogix.Core.Caching {
    /// <summary>
    /// Some reasons why a cache item was discarded
    /// </summary>
    public enum CacheItemDiscardReason {
        /// <summary>
        /// The discard reason is unknown
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// The item was discarded by expiration
        /// </summary>
        Expired = 1,
        /// <summary>
        /// The item was discarded by user action
        /// </summary>
        Discarded = 2,
    }
}