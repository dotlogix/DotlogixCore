// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ParameterCollection.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
using System.Linq;
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Core.Rest.Server.Http.Parameters {
    public class ParameterCollection {
        private readonly Dictionary<string, List<object>> _entries = new Dictionary<string, List<object>>();

        public int Count => _entries.Count;

        public IEnumerable<KeyValuePair<string, IEnumerable<object>>> Entries {
            get { return _entries.Select(e => new KeyValuePair<string, IEnumerable<object>>(e.Key, e.Value)); }
        }

        public object GetValue(string name) {
            return TryGetValue(name, out var value) ? value : null;
        }

        public IEnumerable<object> GetValues(string name) {
            return TryGetValues(name, out var values) ? values : null;
        }

        public bool TryGetValue(string name, out object value) {
            if(_entries.TryGetValue(name, out var list) && (list.Count > 0)) {
                value = list[0];
                return true;
            }

            value = null;
            return false;
        }

        public bool TryGetValues(string name, out IEnumerable<object> values) {
            if(_entries.TryGetValue(name, out var list)) {
                values = list;
                return true;
            }

            values = null;
            return false;
        }

        public TValue GetValue<TValue>(string name) {
            return TryGetValue(name, out TValue value) ? value : default(TValue);
        }

        public IEnumerable<TValue> GetValues<TValue>(string name) {
            return TryGetValues(name, out IEnumerable<TValue> values) ? values : null;
        }

        public bool TryGetValue<TValue>(string name, out TValue value) {
            if(_entries.TryGetValue(name, out var list) && (list.Count > 0))
                return list[0].TryConvertTo(out value);

            value = default(TValue);
            return false;
        }

        public bool TryGetValues<TValue>(string name, out IEnumerable<TValue> values) {
            if(_entries.TryGetValue(name, out var list)) {
                values = list.Select(value => value.TryConvertTo<TValue>(out var cvalue) ? cvalue : default(TValue)).ToList();
                return true;
            }

            values = null;
            return false;
        }

        public void SetValue(string name, object value, bool merge = false) {
            if(_entries.TryGetValue(name, out var existing) == false) {
                existing = new List<object>();
                _entries.Add(name, existing);
            } else if(merge == false)
                existing.Clear();

            existing.Add(value);
        }


        public void SetValues(string name, IEnumerable<object> values, bool merge = false) {
            if(_entries.TryGetValue(name, out var existing) == false) {
                existing = new List<object>();
                _entries.Add(name, existing);
            } else if(merge == false)
                existing.Clear();

            existing.AddRange(values);
        }

        public bool RemoveValue(string name, object value) {
            return _entries.TryGetValue(name, out var existing) && existing.Remove(value);
        }

        public bool RemoveValues(string name) {
            return _entries.Remove(name);
        }

        public bool ContainsValue(string name, object value) {
            return _entries.TryGetValue(name, out var existing) && existing.Contains(value);
        }

        public void Merge(ParameterCollection parameters) {
            foreach(var entry in parameters._entries)
                SetValues(entry.Key, entry.Value, true);
        }

        public void Clear() {
            _entries.Clear();
        }
    }
}
