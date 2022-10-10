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
    /// <summary>
    /// An object to describe hierarchies
    /// </summary>
    /// <typeparam name="TKey">The key type</typeparam>
    /// <typeparam name="TValue">The value type</typeparam>
    public class Hierarchy<TKey, TValue> where TKey : IComparable {
        private readonly Dictionary<TKey, Hierarchy<TKey, TValue>> _children = new Dictionary<TKey, Hierarchy<TKey, TValue>>();

        /// <summary>
        /// Get a child by its key
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns></returns>
        public Hierarchy<TKey, TValue> this[TKey key] => _children[key];

        /// <summary>
        /// The amount of children in this node
        /// </summary>
        public int Count => _children.Count;

        /// <summary>
        /// The root node of the current node
        /// </summary>
        public Hierarchy<TKey, TValue> Root {
            get {
                Hierarchy<TKey, TValue> ancestor;
                var current = this;
                while((ancestor = current.Ancestor) != null)
                    current = ancestor;
                return current;
            }
        }

        /// <summary>
        /// Check if this is the root item
        /// </summary>
        public bool IsRoot => Ancestor == null;

        /// <summary>
        /// The parent node
        /// </summary>
        public Hierarchy<TKey, TValue> Ancestor { get; private set; }

        /// <summary>
        /// Checks if the node has an ancestor
        /// </summary>
        public bool HasAncestor => Ancestor != null;

        /// <summary>
        /// The key
        /// </summary>
        public TKey Key { get; }
        /// <summary>
        /// The value
        /// </summary>
        public TValue Value { get; set; }


        /// <summary>
        /// Creates a new instance of <see cref="Hierarchy{TKey,TValue}"/>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="ancestor"></param>
        public Hierarchy(TKey key, TValue value, Hierarchy<TKey, TValue> ancestor = default) {
            Key = key;
            Value = value;
            Ancestor = ancestor;
        }

        /// <summary>
        /// Enumerates the child nodes
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Hierarchy<TKey, TValue>> Children() {
            return _children.Values.ToList();
        }

        /// <summary>
        /// Enumerates the child values
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TValue> Values() {
            return _children.Values.Select(n => n.Value).ToList();
        }

        /// <summary>
        /// Enumerates all ancestors
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Hierarchy<TKey, TValue>> Ancestors() {
            var current = this;
            while((current = current.Ancestor) != null)
                yield return current;
        }

        /// <summary>
        /// Enumerates all descendants
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Clears all children
        /// </summary>
        public void Clear() {
            foreach(var child in _children.Values)
                child.Ancestor = null;
            _children.Clear();
        }

        #region Remove
        /// <summary>
        /// Removes a child matching the key
        /// </summary>
        /// <param name="key">The key</param>
        public void RemoveChild(TKey key) {
            var child = _children[key];

            child.Ancestor = null;
            _children.Remove(key);
        }
        #endregion

        #region Get
        /// <summary>
        /// Get a child by its key
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Hierarchy<TKey, TValue> GetChild(TKey key) {
            if(key == null)
                throw new ArgumentNullException(nameof(key));

            return _children.TryGetValue(key, out var child) ? child : null;
        }

        /// <summary>
        /// Tries to get a child by its key
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="child">The child if present</param>
        /// <returns></returns>
        public bool TryGetChild(TKey key, out Hierarchy<TKey, TValue> child) {
            if(key == null)
                throw new ArgumentNullException(nameof(key));

            return _children.TryGetValue(key, out child);
        }

        /// <summary>
        /// Get the value of a child or a default value if child does not exist
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public TValue GetValue(TKey key, TValue defaultValue = default) {
            return TryGetChild(key, out var child) ? child.Value : defaultValue;
        }

        /// <summary>
        /// Tries to get the value of a child
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">The value if the child exists</param>
        /// <returns></returns>
        public bool TryGetValue(TKey key, out TValue value) {
            if(TryGetChild(key, out var child))
                value = child.Value;

            value = default;
            return false;
        }
        #endregion

        #region Add
        /// <summary>
        /// Add a child to the node
        /// </summary>
        /// <param name="child"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void AddChild(Hierarchy<TKey, TValue> child) {
            if(child == null)
                throw new ArgumentNullException(nameof(child));

            if(child.Ancestor != null)
                throw new ArgumentException(nameof(Hierarchy<TKey, TValue>) + " already has an ancestor", nameof(child));

            child.Ancestor = this;
            _children.Add(child.Key, child);
        }

        /// <summary>
        /// Creates a new node with the key and value provided
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Hierarchy<TKey, TValue> AddChild(TKey key, TValue value) {
            if(key == null)
                throw new ArgumentNullException(nameof(key));

            var hierarchy = new Hierarchy<TKey, TValue>(key, value, this);
            _children.Add(key, hierarchy);
            return hierarchy;
        }

        /// <summary>
        /// Add multiple children to the node
        /// </summary>
        /// <param name="children"></param>
        public void AddChildren(IEnumerable<Hierarchy<TKey, TValue>> children) {
            foreach(var child in children)
                AddChild(child);
        }
        #endregion
    }
}
