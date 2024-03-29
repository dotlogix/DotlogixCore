// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  NamedCollection.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 22.08.2020 13:51
// LastEdited:  26.09.2021 22:27
// ==================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DotLogix.Core.Extensions;

namespace DotLogix.Core.Collections; 

/// <summary>
///     A class implementing the <see cref="INamedCollection" /> interface
/// </summary>
public class NamedCollection : INamedCollection {
    /// <summary>
    /// The equality comparer used when cloning the dictionary
    /// </summary>
    public IEqualityComparer<string> EqualityComparer { get; protected set; }

    /// <summary>
    ///     The inner dictionary storing the setting values
    /// </summary>
    protected IDictionary<string, object> Values { get; }

    /// <summary>
    ///     Creates a new instance of <see cref="NamedCollection" />
    /// </summary>
    public NamedCollection() : this(null, null) { }

    /// <summary>
    ///     Creates a new instance of <see cref="NamedCollection" />
    /// </summary>
    public NamedCollection(IEqualityComparer<string> comparer = null) : this(null, comparer) { }

    /// <summary>
    ///     Creates a new instance of <see cref="NamedCollection" />
    /// </summary>
    public NamedCollection(IDictionary<string, object> values, IEqualityComparer<string> comparer) {
        EqualityComparer = comparer ?? StringComparer.Ordinal;
        Values = values ?? new Dictionary<string, object>(EqualityComparer);
    }

    /// <inheritdoc />
    public IEnumerable<string> Keys => Values.Keys;

    /// <inheritdoc />
    public int Count => Keys.Count();

    /// <inheritdoc />
    public Optional<object> this[string key] => Get(key);

    /// <inheritdoc />
    public virtual void Set(string key, object value = default) {
        if(key == null)
            throw new ArgumentNullException(nameof(key));
        Values[key] = value;
    }

    /// <inheritdoc />
    public virtual bool Reset(string key) {
        if(key == null)
            throw new ArgumentNullException(nameof(key));
        return Values.Remove(key);
    }

    /// <inheritdoc />
    public virtual Optional<object> Get(string key) {
        return TryGet(key, out var value)
            ? new Optional<object>(value)
            : default;
    }

    /// <inheritdoc />
    public virtual object Get(string key, object defaultValue) {
        return TryGet(key, out var value)
            ? value
            : defaultValue;
    }

    /// <inheritdoc />
    public virtual Optional<T> Get<T>(string key) {
        return TryGet(key, out T value)
            ? new Optional<T>(value)
            : default;
    }

    /// <inheritdoc />
    public virtual T Get<T>(string key, T defaultValue) {
        return TryGet(key, out T value)
            ? value
            : defaultValue;
    }

    /// <inheritdoc />
    public virtual bool TryGet(string key, out object value) {
        if(Values.Count > 0) {
            if(Values.TryGetValue(key, out value))
                return true;
        }

        value = default;
        return false;
    }


    /// <inheritdoc />
    public virtual bool TryGet<T>(string key, out T value) {
        if((Values.Count > 0) && Values.TryGetValue(key, out var objValue) && objValue.TryConvertTo(out value))
            return true;
            
        value = default;
        return false;
    }

    /// <inheritdoc />
    public virtual void Apply(IEnumerable<KeyValuePair<string, object>> values, bool replaceExisting = true) {
        Values.Union(values, replaceExisting);
    }
        
    /// <inheritdoc />
    public virtual IReadOnlyNamedCollection Clone() {
        var values = new Dictionary<string, object>(Values, EqualityComparer);
        return new NamedCollection(values, EqualityComparer);
    }

    /// <summary>Creates a new object that is a copy of the current instance.</summary>
    /// <returns>A new object that is a copy of this instance.</returns>
    object ICloneable.Clone() {
        return Clone();
    }

    /// <summary>Returns an enumerator that iterates through the collection.</summary>
    /// <returns>An enumerator that can be used to iterate through the collection.</returns>
    public IEnumerator<KeyValuePair<string, object>> GetEnumerator() {
        return Values.GetEnumerator();
    }

    /// <summary>Returns an enumerator that iterates through a collection.</summary>
    /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    /// <summary>
    ///     If called by a class member the member name can be omitted
    /// </summary>
    protected virtual void SetWithMemberName(object value, [CallerMemberName] string memberName = null) {
        Set(memberName, value);
    }

    /// <summary>
    ///     If called by a class member the member name can be omitted
    /// </summary>
    protected virtual T GetWithMemberName<T>(T defaultValue = default, [CallerMemberName] string memberName = null) {
        return Get(memberName, defaultValue);
    }
}