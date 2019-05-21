// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ICache.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
#endregion

namespace DotLogix.Core.Caching {
    public interface ICache<TKey, TValue> : IEnumerable<CacheItem<TKey, TValue>> {
        /// <summary>
        ///     The timespan to check if values are no longer valid
        /// </summary>
        TimeSpan CheckPolicyInterval { get; }
        
        /// <summary>
        ///     The current amount of items
        /// </summary>
        int Count { get; }

        /// <summary>
        ///     Get the value for a given key. Returns default if value can not be found
        /// </summary>
        TValue this[TKey key] { get; }

        /// <summary>
        ///     Disposes the cache
        /// </summary>
        void Dispose();

        /// <summary>
        ///     Checks if a value is defined for a given key
        /// </summary>
        bool IsAlive(TKey key);

        /// <summary>
        ///     Stores a value with the given key to the cache. Existing values will be overridden
        /// </summary>
        /// <param name="key">The key of the value</param>
        /// <param name="value">The vlaue</param>
        /// <param name="policy">
        ///     The policy used to check if the value exceed, or null to keep the value until the cache is
        ///     disposed
        /// </param>
        void Store(TKey key, TValue value, ICachePolicy policy = null);

        /// <summary>
        ///     Retrieves a value by its key. Returns default if the key is not present
        /// </summary>
        TValue Retrieve(TKey key);

        /// <summary>
        ///     Trys to retrieve a value by its key.
        /// </summary>
        bool TryRetrieve(TKey key, out TValue value);

        /// <summary>
        ///     Gets and remove a value by its key. Returns default if the key is not present
        /// </summary>
        TValue Pop(TKey key);

        /// <summary>
        ///     Tries to get and remove a value by its key.
        /// </summary>
        bool TryPop(TKey key, out TValue value);

        /// <summary>
        ///     Removes a value by its key
        /// </summary>
        bool Discard(TKey key);

        /// <summary>
        ///     Forces a revalidation of all items in the cache and removes exeeded items
        /// </summary>
        void Cleanup();

        /// <summary>
        ///     Removes all items of the cache
        /// </summary>
        void Clear();

        /// <summary>
        ///     Occures when items are discarded in the cache
        /// </summary>
        event EventHandler<CacheItemsDiscardedEventArgs<TKey, TValue>> ItemsDiscarded;
    }
}
