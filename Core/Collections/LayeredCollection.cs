// ==================================================
// Copyright 2019(C) , DotLogix
// File:  LayeredCollection.cs
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
    public class LayeredCollection<TValue, TCollection> : ILayeredCollection<TValue, TCollection> where TCollection : ICollection<TValue> {
        protected Stack<TCollection> LayerStack { get; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public LayeredCollection() {
            LayerStack = new Stack<TCollection>();
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public LayeredCollection(IEnumerable<TCollection> layers) {
            LayerStack = new Stack<TCollection>(layers);
        }

        public LayeredCollection(TCollection layer) : this() {
            PushLayer(layer);
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<TValue> GetEnumerator() {
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
        public void Add(TValue item) {
            CurrentLayer.Add(item);
        }

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
        public bool Contains(TValue item) {
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
        public void CopyTo(TValue[] array, int arrayIndex) {
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
        public bool Remove(TValue item) {
            return LayerStack.Any(c => c.Remove(item));
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

        public IEnumerable<TCollection> Layers => LayerStack;
        public TCollection CurrentLayer => LayerStack.Count > 0 ? LayerStack.Peek() : default;

        public void PushLayer(TCollection collection) {
            LayerStack.Push(collection);
        }

        public TCollection PopLayer() {
            return LayerStack.Pop();
        }

        public TCollection PeekLayer() {
            return LayerStack.Peek();
        }
    }
}
