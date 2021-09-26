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
    ///     A static class providing extension methods for <see cref="IDictionary{TKey,TValue}" />
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
        ///     Merges two dictionaries and return the result
        /// </summary>
        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> dict, IDictionary<TKey, TValue> other, bool replaceExisting = true, IEqualityComparer<TKey> comparer = null) {
            return Merge(new[] {dict, other}, replaceExisting, comparer);
        }

        /// <summary>
        ///     Merges a collection of dictionaries and return the result
        /// </summary>
        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(this IEnumerable<IDictionary<TKey, TValue>> dicts, bool replaceExisting = true, IEqualityComparer<TKey> comparer = null) {
            var dictionary = new Dictionary<TKey, TValue>(comparer ?? EqualityComparer<TKey>.Default);
            Union(dictionary, dicts);
            return dictionary;
        }

        /// <summary>
        ///     Merges all keys of the other dictionary to the current dictionary
        /// </summary>
        public static void Union<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IEnumerable<KeyValuePair<TKey, TValue>> pairs, bool replaceExisting = true) {
            if(replaceExisting) {
                foreach (var kv in pairs) {
                    dictionary[kv.Key] = kv.Value;
                }
            } else {
                foreach (var kv in pairs) {
                    if(dictionary.ContainsKey(kv.Key) == false)
                        dictionary[kv.Key] = kv.Value;
                }
            }
        }

        /// <summary>
        ///     Merges all keys of the other dictionary to the current dictionary
        /// </summary>
        public static void Union<TKey, TValue, TEnumerable>(this IDictionary<TKey, TValue> dictionary, IEnumerable<TEnumerable> pairs, bool replaceExisting = true) where TEnumerable : IEnumerable<KeyValuePair<TKey, TValue>> {
            foreach(var values in pairs) {
                Union(dictionary, values);
            }
        }
        #endregion

        #region Get

        /// <summary>
        ///     Tries to get value from an <see cref="IDictionary{TKey,TValue}" /> and convert it to the target type<br></br>
        ///     If the key is not found or the value is not convertible to the target type the method throws an exception.
        /// </summary>
        /// <exception cref="KeyNotFoundException">The key does not exist</exception>
        /// <exception cref="InvalidCastException">The value is not convertible to the target type</exception>
        public static TValue GetValueAs<TKey, TValue>(this IDictionary<TKey, object> dict, TKey key) {
            if(dict.TryGetValue(key, out var obj) == false)
                throw new KeyNotFoundException();
            if(obj.TryConvertTo(out TValue value) == false)
                throw new InvalidCastException($"Type {obj.GetType().Name} can not be converted to target type {typeof(TValue).Name}");
            return value;
        }

        /// <summary>
        ///     Tries to get value from an <see cref="IDictionary{TKey,TValue}" /> and convert it to the target type<br></br>
        ///     If the key is not found or the value is not convertible to the target type the method returns the default value.
        /// </summary>
        public static TValue GetValueOrDefaultAs<TKey, TValue>(this IDictionary<TKey, object> dict, TKey key, TValue defaultValue) {
            return dict.TryGetValue(key, out var obj) && obj.TryConvertTo(out TValue value) ? value : defaultValue;
        }


        /// <summary>
        ///     Tries to get value from an <see cref="IDictionary{TKey,TValue}" /> and convert it to the target type<br></br>
        ///     If the key is not found or the value is not convertible to the target type the method returns the default value.
        /// </summary>
        public static object GetValueAs<TKey>(this IDictionary<TKey, object> dict, TKey key, Type targetType, object defaultValue = default) {
            return dict.TryGetValue(key, out var obj) && obj.TryConvertTo(targetType, out var value) ? value : defaultValue;
        }

        /// <summary>
        ///     Tries to get value from an <see cref="IDictionary{TKey,TValue}" /> and convert it to the target type<br></br>
        ///     If the key is not found or the value is not convertible to the target type the method returns false.
        /// </summary>
        public static bool TryGetValueAs<TKey, TValue>(this IDictionary<TKey, object> dict, TKey key, out TValue value) {
            if(dict.TryGetValue(key, out var obj) && obj.TryConvertTo(out value))
                return true;
            value = default;
            return false;
        }

        /// <summary>
        ///     Tries to get value from an <see cref="IDictionary{TKey,TValue}" /> and convert it to the target type<br></br>
        ///     If the key is not found or the value is not convertible to the target type the method returns false.
        /// </summary>
        public static bool TryGetValueAs<TKey>(this IDictionary<TKey, object> dict, TKey key, Type targetType, out object value) {
            if(dict.TryGetValue(key, out var obj) && obj.TryConvertTo(targetType, out value))
                return true;
            value = default;
            return false;
        }
        #endregion

        #region Add
        /// <summary>
        ///     Tries to add value to an <see cref="IDictionary{TKey,TValue}" /><br></br>
        ///     If the key already exist the value will be rejected and method returns the existing value.
        /// </summary>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value) {
            if(dict.TryGetValue(key, out var existing))
                return existing;
            dict[key] = value;
            return value;
        }

        /// <summary>
        ///     Tries to add value to an <see cref="IDictionary{TKey,TValue}" /><br></br>
        ///     If the key does not exist the callback will be called and the value is added to the dictionary.
        /// </summary>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TKey, TValue> valueFunc) {
            if(dict.TryGetValue(key, out var existing))
                return existing;
            var value = valueFunc.Invoke(key);
            dict[key] = value;
            return value;
        }

        /// <summary>
        ///     Tries to add value to an <see cref="IDictionary{TKey,TValue}" /><br></br>
        ///     If the key does not exist the callback will be called and the value is added to the dictionary.
        /// </summary>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TValue> valueFunc) {
            if(dict.TryGetValue(key, out var existing))
                return existing;
            var value = valueFunc.Invoke();
            dict[key] = value;
            return value;
        }

        /// <summary>
        ///     Tries to add value to an <see cref="IDictionary{TKey,TValue}" /><br></br>
        ///     If the key already exists the value will be rejected and the method returns false.
        /// </summary>
        public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value) {
            if(dict.ContainsKey(key))
                return false;
            dict[key] = value;
            return true;
        }
        #endregion

        #region Pop
        /// <summary>
        ///     Tries to get and remove a value from an <see cref="IDictionary{TKey,TValue}" /><br></br>
        ///     If the key does not exist the method will throw an exception
        /// </summary>
        /// <exception cref="KeyNotFoundException">The key does not exist</exception>
        public static TValue PopValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key) {
            if(TryPopValue(dict, key, out var value))
                return value;
            throw new KeyNotFoundException();
        }

        /// <summary>
        ///     Tries to get and remove a value from an <see cref="IDictionary{TKey,TValue}" /><br></br>
        ///     If the key does not the method returns the default value
        /// </summary>
        /// <exception cref="KeyNotFoundException">The key does not exist</exception>
        public static TValue PopValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue defaultValue) {
            return TryPopValue(dict, key, out var value) ? value : defaultValue;
        }


        /// <summary>
        ///     Tries to get and remove a value from an <see cref="IDictionary{TKey,TValue}" /> and convert it to an other type
        ///     <br></br>
        ///     If the key is not found or the value is not convertible to the target type the method throws an exception.
        /// </summary>
        /// <exception cref="KeyNotFoundException">The key does not exist</exception>
        /// <exception cref="InvalidCastException">The value is not convertible to the target type</exception>
        public static TValue PopValueAs<TKey, TValue>(this IDictionary<TKey, object> dict, TKey key) {
            if(dict.TryGetValue(key, out var obj) == false)
                throw new KeyNotFoundException();
            if(obj.TryConvertTo(out TValue value) == false)
                throw new InvalidCastException($"Type {obj.GetType().Name} can not be converted to target type {typeof(TValue).Name}");
            if(dict.Remove(key) == false)
                throw new KeyNotFoundException();
            return value;
        }

        /// <summary>
        ///     Tries to get and remove a value from an <see cref="IDictionary{TKey,TValue}" /> and convert it to an other type
        ///     <br></br>
        ///     If the key is not found or the value is not convertible to the target type the method returns the default value.
        /// </summary>
        public static TValue PopValueAs<TKey, TValue>(this IDictionary<TKey, object> dict, TKey key, TValue defaultValue) {
            return dict.TryGetValue(key, out var obj)
                   && obj.TryConvertTo(out TValue value)
                   && (dict.Remove(key) == false)
                   ? value
                   : defaultValue;
        }

        /// <summary>
        ///     Tries to get and remove a value from an <see cref="IDictionary{TKey,TValue}" /><br></br>
        ///     If the key is not found the method false.
        /// </summary>
        public static bool TryPopValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, out TValue value) {
            return dict.TryGetValue(key, out value) && dict.Remove(key);
        }

        /// <summary>
        ///     Tries to get and remove a value from an <see cref="IDictionary{TKey,TValue}" /> and convert it to another type
        ///     <br></br>
        ///     If the key is not found or the value is not convertible to the target type the method returns false.
        /// </summary>
        public static bool TryPopValueAs<TKey, TValue>(this IDictionary<TKey, object> dict, TKey key, out TValue value) {
            if(dict.TryGetValue(key, out var obj) && obj.TryConvertTo(out value) && dict.Remove(key))
                return true;
            value = default;
            return false;
        }
        #endregion
    }
}
