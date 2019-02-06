﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DotLogix.Core.Patterns
{

    public class Pipeline<TIn, TOut> : PipelineStep<TIn, TOut>, IPipeline<TIn, TOut>{
        protected List<IPipelineStep<TIn, TOut>> Steps { get; } = new List<IPipelineStep<TIn, TOut>>();

        public override async Task<Optional<TOut>> Execute(TIn value) {
            var result = Optional<TOut>.Undefined;
            if(Steps.Count > 0) {
                result = await Steps[0].Execute(value);
            }

            return result.IsDefined ? result : await Next(value);
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<IPipelineStep<TIn, TOut>> GetEnumerator() {
            return Steps.GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable)Steps).GetEnumerator();
        }

        /// <summary>Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.</summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.</exception>
        public void Add(IPipelineStep<TIn, TOut> item) {
            Steps.Add(item);
            Sync(Steps.Count-1);
        }

        /// <summary>Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.</summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only. </exception>
        public void Clear() {
            foreach(var step in Steps) {
                step.NextStep = null;
            }
            Steps.Clear();
        }

        /// <summary>Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.</summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />.</returns>
        public bool Contains(IPipelineStep<TIn, TOut> item) {
            return Steps.Contains(item);
        }

        /// <summary>Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.</summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="array" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="arrayIndex" /> is less than 0.</exception>
        /// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1" /> is greater than the available space from <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />.</exception>
        public void CopyTo(IPipelineStep<TIn, TOut>[] array, int arrayIndex) {
            Steps.CopyTo(array, arrayIndex);
        }

        /// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.</summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />. This method also returns <see langword="false" /> if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.</exception>
        public bool Remove(IPipelineStep<TIn, TOut> item) {
            var index = Steps.IndexOf(item);
            if(index == -1)
                return false;
            
            RemoveAt(index);
            return true;
        }

        /// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.</summary>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
        public int Count => Steps.Count;

        /// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.</summary>
        /// <returns>
        /// <see langword="true" /> if the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, <see langword="false" />.</returns>
        public bool IsReadOnly => false;

        /// <summary>Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1" />.</summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1" />.</param>
        /// <returns>The index of <paramref name="item" /> if found in the list; otherwise, -1.</returns>
        public int IndexOf(IPipelineStep<TIn, TOut> item) {
            return Steps.IndexOf(item);
        }

        /// <summary>Inserts an item to the <see cref="T:System.Collections.Generic.IList`1" /> at the specified index.</summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1" />.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="index" /> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1" />.</exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1" /> is read-only.</exception>
        public void Insert(int index, IPipelineStep<TIn, TOut> item) {
            Steps.Insert(index, item);
            Sync(index);
        }

        /// <summary>Removes the <see cref="T:System.Collections.Generic.IList`1" /> item at the specified index.</summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="index" /> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1" />.</exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1" /> is read-only.</exception>
        public void RemoveAt(int index) {
            Steps.RemoveAt(index);
            var current = Steps[index];
            current.NextStep = null;
            Sync(index);
        }

        /// <summary>Gets or sets the element at the specified index.</summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="index" /> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1" />.</exception>
        /// <exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IList`1" /> is read-only.</exception>
        public IPipelineStep<TIn, TOut> this[int index] {
            get => Steps[index];
            set {
                if(value == null)
                    throw new ArgumentNullException(nameof(value));

                var current = Steps[index];
                current.NextStep = null;

                Steps[index] = value;
                Sync(index);
            }
        }

        private void Sync(int index) {
            var current = Steps[index];
            if(index > 0)
                Steps[index-1].NextStep = current;
            if(index < Count-1)
                current.NextStep = Steps[index+1];
        }
    }
}
