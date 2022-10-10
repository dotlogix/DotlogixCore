// ==================================================
// Copyright 2019(C) , DotLogix
// File:  ILayeredDictionary.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.12.2018
// LastEdited:  07.02.2019
// ==================================================

#region
using System.Collections.Generic;
#endregion

namespace DotLogix.Core.Collections {
    /// <summary>
    /// A collection with multiple dictionary layers, useful for cascading dictionaries
    /// </summary>
    /// <typeparam name="TKey">The key type</typeparam>
    /// <typeparam name="TValue">The value type</typeparam>
    /// <typeparam name="TDictionary">The dictionary type</typeparam>
    public interface ILayeredDictionary<TKey, TValue, TDictionary> : ILayeredCollection<KeyValuePair<TKey, TValue>, TDictionary>, IDictionary<TKey, TValue> where TDictionary : IDictionary<TKey, TValue> { }
}
