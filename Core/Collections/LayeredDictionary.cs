// ==================================================
// Copyright 2019(C) , DotLogix
// File:  LayeredDictionary.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.12.2018
// LastEdited:  07.02.2019
// ==================================================

#region
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace DotLogix.Core.Collections {
    public class LayeredDictionary<TKey, TValue, TDictionary> : ILayeredDictionary<TKey, TValue, TDictionary> where TDictionary : IDictionary<TKey, TValue> {
        protected Stack<TDictionary> LayerStack { get; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public LayeredDictionary() {
            LayerStack = new Stack<TDictionary>();
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public LayeredDictionary(IEnumerable<TDictionary> dictionary) {
            LayerStack = new Stack<TDictionary>(dictionary);
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public LayeredDictionary(TDictionary dictionary) : this() {
            PushLayer(dictionary);
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            foreach(var layer in LayerStack) {
                foreach(var value in layer)
                    yield return value;
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
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) { }

        /// <summary>Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</summary>
        /// <exception cref="T:System.NotSupportedException">
        ///     The <see cref="T:System.Collections.Generic.ICollection`1"></see> is
        ///     read-only.
        /// </exception>
        public void Clear() {
            LayerStack.Clear();
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
            foreach(var layer in LayerStack) {
                layer.CopyTo(array, arrayIndex);
                arrayIndex += layer.Count;
            }
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
        public int Count => LayerStack.Sum(l => l.Count);

        /// <summary>
        ///     Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is
        ///     read-only.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only; otherwise, false.</returns>
        public bool IsReadOnly => false;

        public IEnumerable<TDictionary> Layers => LayerStack;
        public TDictionary CurrentLayer => LayerStack.Count > 0 ? LayerStack.Peek() : default;

        public void PushLayer(TDictionary collection) {
            LayerStack.Push(collection);
        }

        public TDictionary PopLayer() {
            return LayerStack.Pop();
        }

        public TDictionary PeekLayer() {
            return LayerStack.Peek();
        }

        /// <summary>
        ///     Adds an element with the provided key and value to the
        ///     <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key">key</paramref> is null.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///     An element with the same key already exists in the
        ///     <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     The <see cref="T:System.Collections.Generic.IDictionary`2"></see> is
        ///     read-only.
        /// </exception>
        public void Add(TKey key, TValue value) {
            CurrentLayer.Add(key, value);
        }

        /// <summary>
        ///     Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2"></see> contains an element with
        ///     the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</param>
        /// <returns>
        ///     true if the <see cref="T:System.Collections.Generic.IDictionary`2"></see> contains an element with the key;
        ///     otherwise, false.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key">key</paramref> is null.</exception>
        public bool ContainsKey(TKey key) {
            return LayerStack.Any(l => l.ContainsKey(key));
        }

        /// <summary>
        ///     Removes the element with the specified key from the
        ///     <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>
        ///     true if the element is successfully removed; otherwise, false.  This method also returns false if
        ///     <paramref name="key">key</paramref> was not found in the original
        ///     <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key">key</paramref> is null.</exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     The <see cref="T:System.Collections.Generic.IDictionary`2"></see> is
        ///     read-only.
        /// </exception>
        public bool Remove(TKey key) {
            return LayerStack.Any(l => l.Remove(key));
        }

        /// <summary>Gets the value associated with the specified key.</summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">
        ///     When this method returns, the value associated with the specified key, if the key is found;
        ///     otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.
        /// </param>
        /// <returns>
        ///     true if the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"></see> contains an
        ///     element with the specified key; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key">key</paramref> is null.</exception>
        public bool TryGetValue(TKey key, out TValue value) {
            foreach(var layer in LayerStack) {
                if(layer.TryGetValue(key, out value))
                    return true;
            }

            value = default;
            return false;
        }

        /// <summary>Gets or sets the element with the specified key.</summary>
        /// <param name="key">The key of the element to get or set.</param>
        /// <returns>The element with the specified key.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key">key</paramref> is null.</exception>
        /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">
        ///     The property is retrieved and
        ///     <paramref name="key">key</paramref> is not found.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     The property is set and the
        ///     <see cref="T:System.Collections.Generic.IDictionary`2"></see> is read-only.
        /// </exception>
        public TValue this[TKey key] {
            get => TryGetValue(key, out var value) ? value : throw new KeyNotFoundException();
            set { CurrentLayer[key] = value; }
        }

        /// <summary>
        ///     Gets an <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the keys of the
        ///     <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the keys of the object that
        ///     implements <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </returns>
        public ICollection<TKey> Keys => LayerStack.SelectMany(l => l.Keys).ToList();

        /// <summary>
        ///     Gets an <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the values in the
        ///     <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the values in the object that
        ///     implements <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </returns>
        public ICollection<TValue> Values => LayerStack.SelectMany(l => l.Values).ToList();

        public TDictionary Reduce(TDictionary targetDictionary) {
            foreach(var layer in LayerStack) {
                foreach(var value in layer)
                    targetDictionary[value.Key] = value.Value;
            }

            return targetDictionary;
        }
    }
}
