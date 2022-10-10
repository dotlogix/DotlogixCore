// ==================================================
// Copyright 2019(C) , DotLogix
// File:  LayeredDictionary.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.12.2018
// LastEdited:  07.02.2019
// ==================================================

#region
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Core.Collections {
    public class LayeredDictionary<TKey, TValue> : LayeredDictionary<TKey, TValue, Dictionary<TKey, TValue>> {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public LayeredDictionary(IEqualityComparer<TKey> equalityComparer) : base((eq) => new Dictionary<TKey, TValue>(eq), equalityComparer) { }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public LayeredDictionary() : this(null) { }
    }

    /// <inheritdoc />
    public class LayeredDictionary<TKey, TValue, TDictionary> : ILayeredDictionary<TKey, TValue, TDictionary> where TDictionary : class, IDictionary<TKey, TValue> {
        public Func<IEqualityComparer<TKey>, TDictionary> DictionaryFactoryFunc { get; }
        public IEqualityComparer<TKey> EqualityComparer { get; }

        /// <summary>
        /// The internal stack of dictionaries
        /// </summary>
        protected Stack<TDictionary> LayerStack { get; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public LayeredDictionary(Func<IEqualityComparer<TKey>, TDictionary> dictionaryFactoryFunc, IEqualityComparer<TKey> equalityComparer = null) {
            DictionaryFactoryFunc = dictionaryFactoryFunc;
            EqualityComparer = equalityComparer ?? EqualityComparer<TKey>.Default;
            LayerStack = new Stack<TDictionary>();
            PushLayer();
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            var keySet = new HashSet<TKey>();

            foreach(var layer in LayerStack) {
                foreach(var kv in layer) {
                    if(keySet.Add(kv.Key))
                        yield return kv;
                }
            }
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the
        ///     collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        /// <summary>Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <exception cref="T:System.NotSupportedException">
        ///     The <see cref="T:System.Collections.Generic.ICollection`1"></see> is
        ///     read-only.
        /// </exception>
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) {
            CurrentLayer.Add(item);
        }

        /// <summary>Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</summary>
        /// <exception cref="T:System.NotSupportedException">
        ///     The <see cref="T:System.Collections.Generic.ICollection`1"></see> is
        ///     read-only.
        /// </exception>
        public void Clear() {
            LayerStack.Clear();
            PushLayer();
        }

        /// <summary>
        ///     Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific
        ///     value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <returns>
        ///     true if <paramref name="item">item</paramref> is found in the
        ///     <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false.
        /// </returns>
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) {
            return LayerStack.Any(l => l.Contains(item));
        }

        /// <summary>
        ///     Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"></see> to an
        ///     <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
        /// </summary>
        /// <param name="array">
        ///     The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements
        ///     copied from <see cref="T:System.Collections.Generic.ICollection`1"></see>. The <see cref="T:System.Array"></see>
        ///     must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="array">array</paramref> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///     <paramref name="arrayIndex">arrayIndex</paramref> is less than
        ///     0.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     The number of elements in the source
        ///     <see cref="T:System.Collections.Generic.ICollection`1"></see> is greater than the available space from
        ///     <paramref name="arrayIndex">arrayIndex</paramref> to the end of the destination
        ///     <paramref name="array">array</paramref>.
        /// </exception>
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
            this.ToList().CopyTo(array, arrayIndex);
        }

        /// <summary>
        ///     Removes the first occurrence of a specific object from the
        ///     <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <returns>
        ///     true if <paramref name="item">item</paramref> was successfully removed from the
        ///     <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false. This method also returns false if
        ///     <paramref name="item">item</paramref> is not found in the original
        ///     <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
        ///     The <see cref="T:System.Collections.Generic.ICollection`1"></see> is
        ///     read-only.
        /// </exception>
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) {
            return LayerStack.Any(l => l.Remove(item));
        }

        /// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</summary>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</returns>
        public int Count => Keys.Count;

        /// <summary>
        ///     Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is
        ///     read-only.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only; otherwise, false.</returns>
        public bool IsReadOnly => false;

        /// <inheritdoc />
        public IEnumerable<TDictionary> Layers => LayerStack;

        /// <inheritdoc />
        public TDictionary CurrentLayer => LayerStack.Count > 0 ? LayerStack.Peek() : default;


        /// <inheritdoc />
        public TDictionary PushLayer() {
            var dictionary = CreateDictionary();
            LayerStack.Push(dictionary);
            return dictionary;
        }

        /// <inheritdoc />
        public TDictionary PushLayer(TDictionary dictionary) {
            LayerStack.Push(dictionary);
            return dictionary;
        }

        /// <inheritdoc />
        public TDictionary PopLayer() {
            return LayerStack.Pop();
        }

        /// <inheritdoc />
        public TDictionary PeekLayer() {
            return LayerStack.Peek();
        }


        /// <inheritdoc />
        public void Add(TKey key, TValue value) {
            CurrentLayer.Add(key, value);
        }

        /// <inheritdoc />
        public bool ContainsKey(TKey key) {
            return LayerStack.Any(l => l.ContainsKey(key));
        }

        /// <inheritdoc />
        public bool Remove(TKey key) {
            return LayerStack.Any(l => l.Remove(key));
        }


        /// <inheritdoc />
        public bool TryGetValue(TKey key, out TValue value) {
            foreach(var layer in LayerStack) {
                if(layer.TryGetValue(key, out value))
                    return true;
            }

            value = default;
            return false;
        }

        /// <inheritdoc />
        public TValue this[TKey key] {
            get => TryGetValue(key, out var value) ? value : throw new KeyNotFoundException();
            set => CurrentLayer[key] = value;
        }

        /// <inheritdoc />
        public ICollection<TKey> Keys => LayerStack.SelectMany(l => l.Keys).ToHashSet(EqualityComparer);

        /// <inheritdoc />
        public ICollection<TValue> Values => this.Select(kv => kv.Value).ToList();

        /// <summary>
        /// Merges all layers to a new dictionary
        /// </summary>
        /// <returns></returns>
        public TDictionary Reduce() {
            return Reduce(CreateDictionary());
        }

        /// <summary>
        /// Merges all layers to the target dictionary
        /// </summary>
        /// <returns></returns>
        public TDictionary Reduce(TDictionary targetDictionary) {
            targetDictionary.Union(LayerStack);
            return targetDictionary;
        }

        protected TDictionary CreateDictionary() => DictionaryFactoryFunc.Invoke(EqualityComparer);
    }
}
