// ==================================================
// Copyright 2018(C) , DotLogix
// File:  CacheExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Core.Caching;
#endregion

namespace DotLogix.Core.Extensions {
    /// <summary>
    /// A static class providing extension methods for <see cref="ICache{TKey,TValue}"/>
    /// </summary>
    public static class CacheExtensions {
        /// <summary>
        ///     Stores a value in a cache using a <see cref="DynamicCachePolicy" /> with the given timestamp
        /// </summary>
        /// <param name="cache">The cache</param>
        /// <param name="key">The key of the value</param>
        /// <param name="value">The value</param>
        /// <param name="hasExpiredFunc">The callback to get the expiration status</param>
        public static void Store<TKey, TValue>(this ICache<TKey, TValue> cache, TKey key, TValue value, Func<bool> hasExpiredFunc) {
            cache.Store(key, value, new DynamicCachePolicy(hasExpiredFunc));
        }

        /// <summary>
        ///     Stores a value in a cache using a <see cref="DynamicCachePolicy" /> with the given timestamp
        /// </summary>
        /// <param name="cache">The cache</param>
        /// <param name="key">The key of the value</param>
        /// <param name="value">The value</param>
        /// <param name="hasExpiredFunc">The callback to get the expiration status</param>
        public static void Store<TKey, TValue>(this ICache<TKey, TValue> cache, TKey key, TValue value, Func<DateTime, bool> hasExpiredFunc) {
            cache.Store(key, value, new DynamicCachePolicy(hasExpiredFunc));
        }
        
        /// <summary>
        ///     Stores a value in a cache using a <see cref="ValidUntilCachePolicy" /> with the given timestamp
        /// </summary>
        /// <param name="cache">The cache</param>
        /// <param name="key">The key of the value</param>
        /// <param name="value">The value</param>
        /// <param name="validUntilUtc">The time when the value exceed</param>
        public static void Store<TKey, TValue>(this ICache<TKey, TValue> cache, TKey key, TValue value, DateTime validUntilUtc) {
            cache.Store(key, value, new ValidUntilCachePolicy(validUntilUtc));
        }

        /// <summary>
        ///     Stores a value in a cache using a <see cref="ValidUntilCachePolicy" /> with the given duration
        /// </summary>
        /// <param name="cache">The cache</param>
        /// <param name="key">The key of the value</param>
        /// <param name="value">The value</param>
        /// <param name="duration">The duration until the value exceeds</param>
        public static void Store<TKey, TValue>(this ICache<TKey, TValue> cache, TKey key, TValue value, TimeSpan duration) {
            cache.Store(key, value, new ValidUntilCachePolicy(duration));
        }

        /// <summary>
        ///     Retrieves a value by its key. Creates one if the key is not present
        /// </summary>
        public static TValue RetrieveOrCreate<TKey, TValue>(this ICache<TKey, TValue> cache, TKey key, Func<TKey, TValue> createFunc, DateTime validUntilUtc, bool updatePolicy = true) {
            return cache.RetrieveOrCreate(key, createFunc, new ValidUntilCachePolicy(validUntilUtc), updatePolicy);
        }

        /// <summary>
        ///     Retrieves a value by its key. Creates one if the key is not present
        /// </summary>
        public static TValue RetrieveOrCreate<TKey, TValue>(this ICache<TKey, TValue> cache, TKey key, TValue value, DateTime validUntilUtc, bool updatePolicy = true)
        {
            return cache.RetrieveOrCreate(key, value, new ValidUntilCachePolicy(validUntilUtc), updatePolicy);
        }

        /// <summary>
        ///     Retrieves a value by its key. Creates one if the key is not present
        /// </summary>
        public static TValue RetrieveOrCreate<TKey, TValue>(this ICache<TKey, TValue> cache, TKey key, Func<TKey, TValue> createFunc, TimeSpan duration, bool updatePolicy = true)
        {
            return cache.RetrieveOrCreate(key, createFunc, new ValidUntilCachePolicy(duration), updatePolicy);
        }

        /// <summary>
        ///     Retrieves a value by its key. Creates one if the key is not present
        /// </summary>
        public static TValue RetrieveOrCreate<TKey, TValue>(this ICache<TKey, TValue> cache, TKey key, TValue value, TimeSpan duration, bool updatePolicy = true)
        {
            return cache.RetrieveOrCreate(key, value, new ValidUntilCachePolicy(duration), updatePolicy);
        }

        /// <summary>
        ///     Retrieves a value by its key. Returns default if the key is not present
        /// </summary>
        public static T Retrieve<T>(this ICache<object, object> cache, object key)
        {
            return Retrieve<object, T>(cache, key);
        }

        /// <summary>Trys to retrieve a value by its key.</summary>
        public static bool TryRetrieve<T>(this ICache<object, object> cache, object key, out T value)
        {
            return TryRetrieve<object, T>(cache, key, out value);
        }

        /// <summary>
        ///     Gets and remove a value by its key. Returns default if the key is not present or value is not the type specified
        /// </summary>
        public static T Pop<T>(this ICache<object, object> cache, object key) {
            return Pop<object, T>(cache, key);
        }

        /// <summary>Tries to get and remove a value by its key.</summary>
        public static bool TryPop<T>(this ICache<object, object> cache, object key, out T value)
        {
            if (cache.TryPop(key, out var objValue) && objValue is T typedValue)
            {
                value = typedValue;
                return true;
            }

            value = default;
            return false;
        }

        /// <summary>
        ///     Retrieves a value by its key. Returns default if the key is not present
        /// </summary>
        public static TValue Retrieve<TKey, TValue>(this ICache<TKey, object> cache, TKey key)
        {
            return cache.Retrieve(key) is TValue value ? value : default;
        }

        /// <summary>Trys to retrieve a value by its key.</summary>
        public static bool TryRetrieve<TKey, TValue>(this ICache<TKey, object> cache, TKey key, out TValue value)
        {
            if (cache.TryRetrieve(key, out var objValue) && objValue is TValue typedValue)
            {
                value = typedValue;
                return true;
            }

            value = default;
            return false;
        }

        /// <summary>
        ///     Gets and remove a value by its key. Returns default if the key is not present or value is not the type specified
        /// </summary>
        public static TValue Pop<TKey, TValue>(this ICache<TKey, object> cache, TKey key)
        {
            return cache.Pop(key) is TValue value ? value : default;
        }

        /// <summary>Tries to get and remove a value by its key.</summary>
        public static bool TryPop<TKey, TValue>(this ICache<TKey, object> cache, TKey key, out TValue value)
        {
            if (cache.TryPop(key, out var objValue) && objValue is TValue typedValue)
            {
                value = typedValue;
                return true;
            }

            value = default;
            return false;
        }
    }
}
