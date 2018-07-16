// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DictionaryExtension.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Collections.Generic;
using System.Linq;
#endregion

namespace DotLogix.Core.Extensions {
    public static class DictionaryExtension {
        /// <summary>
        /// Reverses a dictionary so values will be the keys and the keys will be the values
        /// </summary>
        public static Dictionary<TValue, TKey> ReverseDictionary<TKey, TValue>(this Dictionary<TKey, TValue> dict) {
            return dict.ToDictionary(k => k.Value, k => k.Key);
        }

        /// <summary>
        /// Reverses a key value pair so value will be the key and the key will be the value
        /// </summary>
        public static KeyValuePair<TValue, TKey> Reverse<TKey, TValue>(this KeyValuePair<TKey, TValue> kvpair) {
            return new KeyValuePair<TValue, TKey>(kvpair.Value, kvpair.Key);
        }

        /// <summary>
        /// Find the the key of an item in the dictionary
        /// </summary>
        public static TKey GetKey<TKey, TValue>(this Dictionary<TKey, TValue> dict, TValue value)
        {
            if (TryGetKey(dict, value, out var key))
                return key;
            throw new KeyNotFoundException();
        }

        /// <summary>
        /// Tries to find the the key of an item in the dictionary
        /// </summary>
        public static bool TryGetKey<TKey, TValue>(this Dictionary<TKey, TValue> dict, TValue value, out TKey key)
        {
            using (var enumerator = dict.Where(p => Equals(p.Value, value)).GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    key = enumerator.Current.Key;
                    return true;
                }
            }

            key = default(TKey);
            return false;
        }
    }
}
