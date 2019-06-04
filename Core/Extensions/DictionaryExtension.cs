// ==================================================
// Copyright 2019(C) , DotLogix
// File:  DictionaryExtension.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  30.08.2018
// LastEdited:  07.02.2019
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace DotLogix.Core.Extensions {
    /// <summary>
    /// A static class providing extension methods for <see cref="IDictionary{TKey,TValue}"/>
    /// </summary>
    public static class DictionaryExtension {
        #region Reverse
        /// <summary>
        ///     Reverses a dictionary so values will be the keys and the keys will be the values
        /// </summary>
        public static Dictionary<TValue, TKey> ReverseDictionary<TKey, TValue>(this IDictionary<TKey, TValue> dict) {
            return dict.ToDictionary(k => k.Value, k => k.Key);
        }

        /// <summary>
        ///     Reverses a key value pair so value will be the key and the key will be the value
        /// </summary>
        public static KeyValuePair<TValue, TKey> Reverse<TKey, TValue>(this KeyValuePair<TKey, TValue> kvpair) {
            return new KeyValuePair<TValue, TKey>(kvpair.Value, kvpair.Key);
        }
        #endregion

        #region Key
        /// <summary>
        ///     Find the the key of an item in the dictionary
        /// </summary>
        public static TKey GetKey<TKey, TValue>(this IDictionary<TKey, TValue> dict, TValue value) {
            if(TryGetKey(dict, value, out var key))
                return key;
            throw new KeyNotFoundException();
        }

        /// <summary>
        ///     Tries to find the the key of an item in the dictionary
        /// </summary>
        public static bool TryGetKey<TKey, TValue>(this IDictionary<TKey, TValue> dict, TValue value, out TKey key) {
            using(var enumerator = dict.Where(p => Equals(p.Value, value)).GetEnumerator()) {
                if(enumerator.MoveNext()) {
                    key = enumerator.Current.Key;
                    return true;
                }
            }

            key = default;
            return false;
        }
        #endregion

        #region Merge
        /// <summary>
        ///     Find the the key of an item in the dictionary
        /// </summary>
        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> dict, IDictionary<TKey, TValue> other, bool replaceExisting = true, IEqualityComparer<TKey> comparer = null) {
            return Merge(new[] {dict, other}, replaceExisting, comparer);
        }

        /// <summary>
        ///     Find the the key of an item in the dictionary
        /// </summary>
        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(this IEnumerable<IDictionary<TKey, TValue>> dicts, bool replaceExisting = true, IEqualityComparer<TKey> comparer = null) {
            Dictionary<TKey, TValue> dictionary = null;
            var enumerator = dicts.GetEnumerator();
            try {
                if(enumerator.MoveNext()) {
                    dictionary = new Dictionary<TKey, TValue>(enumerator.Current, comparer ?? EqualityComparer<TKey>.Default);
                    while(enumerator.MoveNext()) {
                        if(replaceExisting) {
                            foreach(var kv in enumerator.Current)
                                dictionary[kv.Key] = kv.Value;
                        } else {
                            foreach(var kv in enumerator.Current)
                                dictionary.TryAdd(kv.Key, kv.Value);
                        }
                    }
                }
            } finally {
                enumerator.Dispose();
            }

            return dictionary;
        }
        #endregion

        #region Get
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default) {
            return dict.TryGetValue(key, out var value) ? value : defaultValue;
        }

        public static TValue GetValueAs<TKey, TValue>(this IDictionary<TKey, object> dict, TKey key) {
            if(dict.TryGetValue(key, out var obj) == false)
                throw new KeyNotFoundException();
            if(obj.TryConvertTo(out TValue value) == false)
                throw new InvalidCastException($"Type {obj.GetType().Name} can not be converted to target type {typeof(TValue).Name}");
            return value;
        }

        public static TValue GetValueAs<TKey, TValue>(this IDictionary<TKey, object> dict, TKey key, TValue defaultValue) {
            return dict.TryGetValue(key, out var obj) && obj.TryConvertTo(out TValue value) ? value : defaultValue;
        }


        public static object GetValueAs<TKey>(this IDictionary<TKey, object> dict, TKey key, Type targetType, object defaultValue = default) {
            return dict.TryGetValue(key, out var obj) && obj.TryConvertTo(targetType, out var value) ? value : defaultValue;
        }

        public static bool TryGetValueAs<TKey, TValue>(this IDictionary<TKey, object> dict, TKey key, out TValue value) {
            if(dict.TryGetValue(key, out var obj) && obj.TryConvertTo(out value))
                return true;
            value = default;
            return false;
        }

        public static bool TryGetValueAs<TKey>(this IDictionary<TKey, object> dict, TKey key, Type targetType, out object value) {
            if(dict.TryGetValue(key, out var obj) && obj.TryConvertTo(targetType, out value))
                return true;
            value = default;
            return false;
        }
        #endregion

        #region Add
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value) {
            if(dict.TryGetValue(key, out var existing))
                return existing;
            dict[key] = value;
            return value;
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TKey, TValue> valueFunc) {
            if(dict.TryGetValue(key, out var existing))
                return existing;
            var value = valueFunc.Invoke(key);
            dict[key] = value;
            return value;
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TValue> valueFunc) {
            if(dict.TryGetValue(key, out var existing))
                return existing;
            var value = valueFunc.Invoke();
            dict[key] = value;
            return value;
        }

        public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value) {
            if(dict.ContainsKey(key))
                return false;
            dict[key] = value;
            return true;
        }
        #endregion

        #region Pop
        /// <summary>Gets and removes the value associated with the specified key.</summary>
        /// <param name="dict">The dictionary</param>
        /// <param name="key">The key of the value to get and remove.</param>
        /// <returns>
        ///     The value associated with the specified key. If the specified key is not found, a get operation throws a
        ///     <see cref="T:System.Collections.Generic.KeyNotFoundException"></see>, and a set operation creates a new element
        ///     with the specified key.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key">key</paramref> is null.</exception>
        /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">
        ///     The property is retrieved and
        ///     <paramref name="key">key</paramref> does not exist in the collection.
        /// </exception>
        public static TValue PopValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key) {
            if(TryPopValue(dict, key, out var value))
                return value;
            throw new KeyNotFoundException();
        }

        /// <summary>Gets and removes the value associated with the specified key.</summary>
        /// <param name="dict">The dictionary</param>
        /// <param name="key">The key of the value to get and remove.</param>
        /// <returns>
        ///     The value associated with the specified key. If the specified key is not found, a get operation returns the
        ///     default value
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key">key</paramref> is null.</exception>
        /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">
        ///     The property is retrieved and
        ///     <paramref name="key">key</paramref> does not exist in the collection.
        /// </exception>
        public static TValue PopValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue defaultValue) {
            return TryPopValue(dict, key, out var value) ? value : defaultValue;
        }


        public static TValue PopValueAs<TKey, TValue>(this IDictionary<TKey, object> dict, TKey key) {
            if(dict.TryGetValue(key, out var obj) == false)
                throw new KeyNotFoundException();
            if(obj.TryConvertTo(out TValue value) == false)
                throw new InvalidCastException($"Type {obj.GetType().Name} can not be converted to target type {typeof(TValue).Name}");
            if(dict.Remove(key) == false)
                throw new KeyNotFoundException();
            return value;
        }

        /// <summary>Gets and removes the value associated with the specified key.</summary>
        /// <param name="dict">The dictionary</param>
        /// <param name="key">The key of the value to get and remove.</param>
        /// <param name="value">
        ///     When this method returns, contains the value associated with the specified key, if the key is
        ///     found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.
        /// </param>
        /// <returns>
        ///     true if the <see cref="T:System.Collections.Generic.Dictionary`2"></see> contains an element with the
        ///     specified key; otherwise, false.
        /// </returns>
        public static bool TryPopValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, out TValue value) {
            return dict.TryGetValue(key, out value) && dict.Remove(key);
        }

        public static bool TryPopValue<TKey, TValue>(this IDictionary<TKey, object> dict, TKey key, out TValue value) {
            if(dict.TryGetValue(key, out var obj) && obj.TryConvertTo(out value) && dict.Remove(key))
                return true;
            value = default;
            return false;
        }
        #endregion
    }
}
