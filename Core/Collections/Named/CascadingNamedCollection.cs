// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  CascadingNamedCollection.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 22.08.2020 13:51
// LastEdited:  26.09.2021 22:27
// ==================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DotLogix.Core.Collections {
    public class CascadingNamedCollection : IReadOnlyNamedCollection {
        /// <summary>
        /// The settings key equality comparer used to cascade
        /// </summary>
        protected IEqualityComparer<string> NameComparer { get; set; }

        /// <summary>
        /// The source settings collection
        /// </summary>
        protected IReadOnlyCollection<IReadOnlyNamedCollection> Values { get; }

        /// <inheritdoc />
        public CascadingNamedCollection(IReadOnlyCollection<IReadOnlyNamedCollection> values, IEqualityComparer<string> nameComparer = null) {
            Values = values;
            NameComparer = nameComparer ?? StringComparer.Ordinal;
        }

        /// <inheritdoc />
        public CascadingNamedCollection(IEqualityComparer<string> nameComparer = null) : this(new List<IReadOnlyNamedCollection>(), nameComparer) {

        }

        /// <inheritdoc />
        public IEnumerable<string> Keys {
            get {
                var keys = new HashSet<string>(NameComparer);
                foreach(var setting in Values) {
                    keys.UnionWith(setting.Keys);
                }
                return keys;
            }
        }

        /// <inheritdoc />
        public int Count => Keys.Count();

        /// <inheritdoc />
        public Optional<object> this[string key] => Get(key);
        
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
            foreach(var settings in Values) {
                if (settings != null && settings.TryGet(key, out value))
                    return true;
            }

            value = default;
            return false;
        }


        /// <inheritdoc />
        public virtual bool TryGet<T>(string key, out T value) {
            foreach (var settings in Values) {
                if (settings != null && settings.TryGet(key, out value))
                    return true;
            }

            value = default;
            return false;
        }

        /// <inheritdoc />
        public virtual IReadOnlyNamedCollection Clone() {
            return new CascadingNamedCollection(new List<IReadOnlyNamedCollection>(Values), NameComparer);
        }

        /// <inheritdoc />
        object ICloneable.Clone() {
            return Clone();
        }


        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() {
            var settingKeys = new HashSet<string>(NameComparer);

            foreach(var settings in Values) {
                foreach(var kv in settings) {
                    if(settingKeys.Add(kv.Key))
                        yield return kv;
                }
            }
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
        
        /// <summary>
        ///     If called by a class member the member name can be omitted
        /// </summary>
        protected virtual T GetWithMemberName<T>([CallerMemberName] string memberName = null, T defaultValue = default) {
            return Get(memberName, defaultValue);
        }
    }
}
