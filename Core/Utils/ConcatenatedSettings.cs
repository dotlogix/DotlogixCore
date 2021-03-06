// ==================================================
// Copyright 2019(C) , DotLogix
// File:  ConcatenatedSettings.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  ..
// LastEdited:  12.12.2019
// ==================================================

// ==================================================
// Copyright 2019(C) , DotLogix
// File:  ConcatenatedSettings.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  ..
// LastEdited:  12.12.2019
// ==================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DotLogix.Core.Extensions;

namespace DotLogix.Core.Utils {
    public class ConcatenatedSettings : IReadOnlySettings {
        public IEqualityComparer<string> EqualityComparer { get; protected set; }

        /// <summary>
        /// The source settings collection
        /// </summary>
        public ICollection<IReadOnlySettings> Settings { get; }

        /// <inheritdoc />
        public ConcatenatedSettings(ICollection<IReadOnlySettings> settings, IEqualityComparer<string> comparer = null) {
            Settings = settings;
            EqualityComparer = comparer ?? StringComparer.Ordinal;
        }

        /// <inheritdoc />
        public ConcatenatedSettings(IEqualityComparer<string> comparer = null) : this(new List<IReadOnlySettings>(), comparer) {

        }

        /// <inheritdoc />
        public IReadOnlySettings Inherits => null;

        /// <inheritdoc />
        public IReadOnlySettings OwnSettings => new Settings();

        /// <inheritdoc />
        public IEnumerable<string> OwnKeys => Enumerable.Empty<string>();

        /// <inheritdoc />
        public int OwnCount => 0;

        /// <inheritdoc />
        public IEnumerable<string> Keys {
            get {
                return Settings.SkipNull()
                               .SelectMany(s => s.Keys)
                               .Distinct();
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
            foreach(var settings in Settings) {
                if (settings != null && settings.TryGet(key, out value))
                    return true;
            }

            value = default;
            return false;
        }


        /// <inheritdoc />
        public virtual bool TryGet<T>(string key, out T value) {
            foreach (var settings in Settings) {
                if (settings != null && settings.TryGet(key, out value))
                    return true;
            }

            value = default;
            return false;
        }
        
        /// <inheritdoc />
        public virtual ISettings Reduce() {
            var values = new Dictionary<string, object>(EqualityComparer);

            foreach(var settings in Settings.Reverse()) {
                if(settings != null)
                    values.Union(settings);
            }

            return new Settings(values, EqualityComparer);
        }

        /// <inheritdoc />
        public virtual ISettings Inherit() {
            return new Settings { Inherits = this };
        }

        /// <inheritdoc />
        public IReadOnlySettings Clone() {
            return new ConcatenatedSettings(new List<IReadOnlySettings>(Settings), EqualityComparer);
        }

        /// <inheritdoc />
        object ICloneable.Clone() {
            return Clone();
        }


        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() {
            var settingKeys = new HashSet<string>(EqualityComparer);

            IReadOnlySettings current = this;
            do {
                if (current.OwnCount > 0) {
                    foreach (var kv in current) {
                        if (settingKeys.Add(kv.Key))
                            yield return kv;
                    }
                }

                current = current.Inherits;
            } while (current != null);
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
