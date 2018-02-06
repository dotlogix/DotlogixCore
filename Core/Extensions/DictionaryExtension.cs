// ==================================================
// Copyright 2016(C) , DotLogix
// File:  DictionaryExtension.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.09.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System.Collections.Generic;
using System.Linq;
#endregion

namespace DotLogix.Core.Extensions {
    public static class DictionaryExtension {
        public static Dictionary<TValue, TKey> ReverseDictionary<TKey, TValue>(this Dictionary<TKey, TValue> dict) {
            return dict.ToDictionary(k => k.Value, k => k.Key);
        }

        public static KeyValuePair<TValue, TKey> Reverse<TKey, TValue>(this KeyValuePair<TKey, TValue> kvpair) {
            return new KeyValuePair<TValue, TKey>(kvpair.Value, kvpair.Key);
        }
    }
}
