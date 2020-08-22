using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DotLogix.Core.Extensions;

namespace DotLogix.Core.Nodes.Processor {
    public class CharBuffer : IReadOnlyList<char>, IDisposable
    {
        private char[] _buffer;
        private int _capacity;
        private int _count;

        public int Count => _count;
        public int Capacity => _capacity;

        public char this[int index] =>
        index.LaysBetween(0, _count)
        ? _buffer[index]
        : throw new ArgumentOutOfRangeException(nameof(index));

        public CharBuffer(char[] buffer, int count = -1)
        {
            if (count < 0)
                count = buffer.Length;

            _buffer = buffer;
            _count = count;
            _capacity = buffer.Length;
        }

        public CharBuffer(string buffer, int offset = 0, int count = -1)
        {
            if (count < 0)
                count = buffer.Length - offset;

            _buffer = JsonStrings.RentBuffer(count);
            _capacity = _buffer.Length;
            Append(buffer, offset, count);
        }

        public CharBuffer(int minCapacity)
        {
            _buffer = JsonStrings.RentBuffer(minCapacity);
            _capacity = _buffer.Length;
        }

        public CharBuffer Append(char chr)
        {
            EnsureCapacity();
            _buffer[_count] = chr;
            _count++;

            return this;
        }

        public CharBuffer Append(char chr, int repeat)
        {
            EnsureCapacity(repeat);
            var end = _count + repeat;
            for (var i = _count; i < end; i++) {
                _buffer[i] = chr;
            }

            _count += repeat;

            return this;
        }

        public CharBuffer Append(char[] buffer)
        {
            return Append(buffer, 0, buffer.Length);
        }

        public CharBuffer Append(char[] buffer, int offset, int count)
        {
            EnsureCapacity(count);
            Array.Copy(buffer, offset, _buffer, _count, count);
            _count += count;
            return this;
        }

        public CharBuffer Append(string buffer)
        {
            return Append(buffer, 0, buffer.Length);
        }

        public CharBuffer Append(string buffer, int offset, int count)
        {
            EnsureCapacity(count);
            buffer.CopyTo(offset, _buffer, _count, count);
            _count += count;
            return this;
        }

        public void EnsureCapacity(int minRemaining = 1)
        {
            var remaining = _capacity - _count;
            if (remaining >= minRemaining)
                return;

            var minCapacity = _capacity + Math.Max(_buffer.Length, minRemaining);
            var array = JsonStrings.RentBuffer(minCapacity);
            
            Array.Copy(_buffer, 0, array, 0, _count);

            JsonStrings.ReturnBuffer(_buffer);
            _buffer = array;
            _capacity = array.Length;
        }

        public CharBuffer Clear()
        {
            _count = 0;
            return this;
        }

        public ArraySegment<char> GetBuffer()
        {
            return new ArraySegment<char>(_buffer, 0, _count);
        }

        public bool Equals(char[] other)
        {
            if (other == null || _count != other.Length)
                return false;

            for (var i = 0; i < _count; i++)
            {
                if (_buffer[i] != other[i])
                    return false;
            }
            return true;
        }

        public override string ToString()
        {
            return new string(_buffer, 0, _count);
        }

        public IEnumerator<char> GetEnumerator()
        {
            return _buffer.Take(_count).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            JsonStrings.ReturnBuffer(_buffer);
            _buffer = null;
        }
    }
}