// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Hierarchy.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  10.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
using System.Linq;
#endregion

namespace DotLogix.Core.Collections {
    public class HierarchyCollection<TKey, TValue> : KeyedCollection<TKey, Hierarchy<TKey, TValue>> {
        public HierarchyCollection() : base(i => i.Key) { }
    }


    public class Hierarchy<TKey, TValue> {
        private readonly Dictionary<TKey, Hierarchy<TKey, TValue>> _children = new Dictionary<TKey, Hierarchy<TKey, TValue>>();

        public TKey Key { get; }
        public TValue Value { get; set; }
        public bool IsLeaf => _children.Count == 0;
        public bool IsNode => _children.Count > 0;
        public Hierarchy<TKey, TValue> this[TKey key] => _children[key];
        public IEnumerable<Hierarchy<TKey, TValue>> Children => _children.Values;

        public IEnumerable<Hierarchy<TKey, TValue>> ChildrenRecursive {
            get {
                var list = new List<Hierarchy<TKey, TValue>>();
                GetChildrenRecursive(list);
                return list;
            }
        }


        public Hierarchy(TKey key, TValue value = default) {
            Key = key;
            Value = value;
        }

        private void GetChildrenRecursive(List<Hierarchy<TKey, TValue>> children) {
            children.AddRange(_children.Values);
            foreach(var child in _children.Values)
                child.GetChildrenRecursive(children);
        }

        public void Clear() {
            _children.Clear();
        }

        public Hierarchy<TKey, TValue> Add(TKey key, TValue value) {
            var hierarchy = new Hierarchy<TKey, TValue>(key, value);
            _children.Add(key, hierarchy);
            return hierarchy;
        }

        public bool Remove(TKey key) {
            return _children.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value) {
            if(TryGetChild(key, out var child)) {
                value = child.Value;
                return true;
            }

            value = default;
            return true;
        }

        public bool TryGetChild(TKey key, out Hierarchy<TKey, TValue> child) {
            return _children.TryGetValue(key, out child);
        }

        public bool ContainsKey(TKey key) {
            return _children.ContainsKey(key);
        }

        public void AddRecursive(TKey parentKey, TKey key, TValue value) {
            if(TryGetChildRecursive(parentKey, out var child) == false)
                throw new KeyNotFoundException();

            child.Add(key, value);
        }

        public bool RemoveRecursive(TKey key) {
            return _children.Remove(key) || _children.Values.Any(child => child.RemoveRecursive(key));
        }

        public bool TryGetValueRecursive(TKey key, out TValue value) {
            if(TryGetChildRecursive(key, out var child)) {
                value = child.Value;
                return true;
            }

            if(_children.Values.Any(childHierarchy => childHierarchy.TryGetChildRecursive(key, out child))) {
                value = child.Value;
                return true;
            }

            value = default;
            return true;
        }

        public bool TryGetChildRecursive(TKey key, out Hierarchy<TKey, TValue> child) {
            if(_children.TryGetValue(key, out child))
                return true;

            foreach(var childHierarchy in _children.Values) {
                if(childHierarchy.TryGetChildRecursive(key, out child))
                    return true;
            }

            return false;
        }

        public bool ContainsKeyRecursive(TKey key) {
            return TryGetChildRecursive(key, out _);
        }
    }
}
