#region
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Core.Nodes.Utils {
    public class CharBuffer : IReadOnlyList<char>, IDisposable {
        private char[] _buffer;

        public int Count { get; private set; }

        public char this[int index] =>
            index.LaysBetween(0, Count)
                ? _buffer[index]
                : throw new ArgumentOutOfRangeException(nameof(index));

        public int Capacity { get; private set; }

        public CharBuffer(char[] buffer, int count = -1) {
            if(count < 0)
                count = buffer.Length;

            _buffer = buffer;
            Count = count;
            Capacity = buffer.Length;
        }

        public CharBuffer(string buffer, int offset = 0, int count = -1) {
            if(count < 0)
                count = buffer.Length - offset;

            _buffer = JsonStrings.RentBuffer(count);
            Capacity = _buffer.Length;
            Append(buffer, offset, count);
        }

        public CharBuffer(int minCapacity) {
            _buffer = JsonStrings.RentBuffer(minCapacity);
            Capacity = _buffer.Length;
        }

        public void Dispose() {
            JsonStrings.ReturnBuffer(_buffer);
            _buffer = null;
        }

        public IEnumerator<char> GetEnumerator() {
            return _buffer.Take(Count).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public CharBuffer Append(char chr) {
            EnsureCapacity();
            _buffer[Count] = chr;
            Count++;

            return this;
        }

        public CharBuffer Append(char chr, int repeat) {
            EnsureCapacity(repeat);
            var end = Count + repeat;
            for(var i = Count; i < end; i++)
                _buffer[i] = chr;

            Count += repeat;

            return this;
        }

        public CharBuffer Append(char[] buffer) {
            return Append(buffer, 0, buffer.Length);
        }

        public CharBuffer Append(char[] buffer, int offset, int count) {
            EnsureCapacity(count);
            Array.Copy(buffer, offset, _buffer, Count, count);
            Count += count;
            return this;
        }

        public CharBuffer Append(string buffer) {
            return Append(buffer, 0, buffer.Length);
        }

        public CharBuffer Append(string buffer, int offset, int count) {
            EnsureCapacity(count);
            buffer.CopyTo(offset, _buffer, Count, count);
            Count += count;
            return this;
        }

        public void EnsureCapacity(int minRemaining = 1) {
            var remaining = Capacity - Count;
            if(remaining >= minRemaining)
                return;

            var minCapacity = Capacity + Math.Max(_buffer.Length, minRemaining);
            var array = JsonStrings.RentBuffer(minCapacity);

            Array.Copy(_buffer, 0, array, 0, Count);

            JsonStrings.ReturnBuffer(_buffer);
            _buffer = array;
            Capacity = array.Length;
        }

        public CharBuffer Clear() {
            Count = 0;
            return this;
        }

        public ArraySegment<char> GetBuffer() {
            return new ArraySegment<char>(_buffer, 0, Count);
        }

        public bool Equals(char[] other) {
            if((other == null) || (Count != other.Length))
                return false;

            for(var i = 0; i < Count; i++) {
                if(_buffer[i] != other[i])
                    return false;
            }

            return true;
        }

        public override string ToString() {
            return new string(_buffer, 0, Count);
        }
    }
}
