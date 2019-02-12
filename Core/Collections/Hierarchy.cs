// ==================================================
// Copyright 2019(C) , DotLogix
// File:  Hierarchy.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  03.09.2018
// LastEdited:  07.02.2019
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Core.Collections {
    public class Hierarchy<TKey, TValue> where TKey : IComparable {
        private readonly Dictionary<TKey, Hierarchy<TKey, TValue>> _children = new Dictionary<TKey, Hierarchy<TKey, TValue>>();
        public Hierarchy<TKey, TValue> this[TKey key] => _children[key];

        public int Count => _children.Count;


        public Hierarchy<TKey, TValue> Root {
            get {
                Hierarchy<TKey, TValue> ancestor;
                var current = this;
                while((ancestor = current.Ancestor) != null)
                    current = ancestor;
                return current;
            }
        }

        public bool IsRoot => Ancestor == null;

        public Hierarchy<TKey, TValue> Ancestor { get; private set; }
        public bool HasAncestor => Ancestor != null;

        public TKey Key { get; }
        public TValue Value { get; set; }


        public Hierarchy(TKey key, TValue value, Hierarchy<TKey, TValue> ancestor = default) {
            Key = key;
            Value = value;
            Ancestor = ancestor;
        }


        public IEnumerable<Hierarchy<TKey, TValue>> Children() {
            return _children.Values.ToList();
        }

        public IEnumerable<TValue> Values() {
            return _children.Values.Select(n => n.Value).ToList();
        }

        public IEnumerable<Hierarchy<TKey, TValue>> Ancestors() {
            var current = this;
            while((current = current.Ancestor) != null)
                yield return current;
        }

        public IEnumerable<Hierarchy<TKey, TValue>> Descendants() {
            var currentLevel = new List<IEnumerable<Hierarchy<TKey, TValue>>> {Children()};
            var nextLevel = new List<IEnumerable<Hierarchy<TKey, TValue>>>();

            do {
                foreach(var child in currentLevel.Balance()) {
                    yield return child;
                    nextLevel.Add(child.Children());
                }

                var temp = currentLevel;
                currentLevel = nextLevel;
                nextLevel = temp;
                nextLevel.Clear();
            } while(currentLevel.Count > 0);
        }

        public void Clear() {
            foreach(var child in _children.Values)
                child.Ancestor = null;
            _children.Clear();
        }

        #region Remove
        public void RemoveChild(TKey key) {
            var child = _children[key];

            child.Ancestor = null;
            _children.Remove(key);
        }
        #endregion

        #region Get
        public Hierarchy<TKey, TValue> GetChild(TKey key) {
            if(key == null)
                throw new ArgumentNullException(nameof(key));

            return _children.TryGetValue(key, out var child) ? child : null;
        }

        public bool TryGetChild(TKey key, out Hierarchy<TKey, TValue> child) {
            if(key == null)
                throw new ArgumentNullException(nameof(key));

            return _children.TryGetValue(key, out child);
        }

        public TValue GetValue(TKey key, TValue defaultValue = default) {
            return TryGetChild(key, out var child) ? child.Value : defaultValue;
        }

        public bool TryGetValue(TKey key, out TValue value) {
            if(TryGetChild(key, out var child))
                value = child.Value;

            value = default;
            return false;
        }
        #endregion

        #region Add
        public void AddChild(Hierarchy<TKey, TValue> child) {
            if(child == null)
                throw new ArgumentNullException(nameof(child));

            if(child.Ancestor != null)
                throw new ArgumentException(nameof(Hierarchy<TKey, TValue>) + " already has an ancestor", nameof(child));

            child.Ancestor = this;
            _children.Add(child.Key, child);
        }

        public Hierarchy<TKey, TValue> AddChild(TKey key, TValue value) {
            if(key == null)
                throw new ArgumentNullException(nameof(key));

            var hierarchy = new Hierarchy<TKey, TValue>(key, value, this);
            _children.Add(key, hierarchy);
            return hierarchy;
        }

        public void AddChildren(IEnumerable<Hierarchy<TKey, TValue>> children) {
            foreach(var child in children)
                AddChild(child);
        }
        #endregion
    }
}
