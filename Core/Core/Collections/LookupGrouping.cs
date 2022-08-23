// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  LookupGrouping.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 22.08.2020 13:51
// LastEdited:  26.09.2021 22:27
// ==================================================

#region
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace DotLogix.Core.Collections; 

/// <summary>
/// A grouping class for lookups
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public struct LookupGrouping<TKey, TValue> : IGrouping<TKey, TValue> {
    private readonly IEnumerable<TValue> _values;

    /// <summary>Gets the key of the <see cref="T:System.Linq.IGrouping`2"></see>.</summary>
    /// <returns>The key of the <see cref="T:System.Linq.IGrouping`2"></see>.</returns>
    public TKey Key { get; }

    /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
    public LookupGrouping(TKey key, IEnumerable<TValue> values) {
        _values = values;
        Key = key;
    }

    /// <summary>Returns an enumerator that iterates through the collection.</summary>
    /// <returns>An enumerator that can be used to iterate through the collection.</returns>
    public IEnumerator<TValue> GetEnumerator() {
        return _values.GetEnumerator();
    }

    /// <summary>Returns an enumerator that iterates through a collection.</summary>
    /// <returns>
    ///     An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the
    ///     collection.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator() {
        return ((IEnumerable)_values).GetEnumerator();
    }
}