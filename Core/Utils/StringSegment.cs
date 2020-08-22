using System;
using System.Collections;
using System.Collections.Generic;
using DotLogix.Core.Extensions;

namespace DotLogix.Core.Utils {
    public readonly struct StringSegment : IReadOnlyList<char> {
        public int Offset { get; }
        public string Buffer { get; }

        /// <inheritdoc />
        public int Count { get; }

        /// <inheritdoc />
        public char this[int index] => Buffer[Offset + index];

        public StringSegment(string buffer, int offset, int count) {
            Buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
            if(offset.LaysBetween(0, buffer.Length) == false)
                throw new ArgumentOutOfRangeException(nameof(offset), offset, $"Offset is out of range {{ Offset: {offset}, Total: {buffer.Length} }}");

            if(count.LaysBetween(0, buffer.Length - offset) == false)
                throw new ArgumentOutOfRangeException(nameof(offset), offset, $"Not enough characters left {{ Offset: {offset}, Total: {buffer.Length}, Available: {buffer.Length-offset}, Requested: {count} }}");

            Offset = offset;
            Count = count;
        }

        /// <inheritdoc />
        public IEnumerator<char> GetEnumerator() {
            var end = Offset + Count;
            for(var i = Offset; i < end; i++)
                yield return Buffer[i];
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public bool Equals(StringSegment other)
        {
            return Offset == other.Offset
                && Count == other.Count
                && Buffer == other.Buffer;
        }

        public override bool Equals(object obj)
        {
            return obj is StringSegment other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Offset;
                hashCode = (hashCode * 397) ^ Count;
                hashCode = (hashCode * 397) ^ (Buffer != null ? Buffer.GetHashCode() : 0);
                return hashCode;
            }
        }

        public override string ToString()
        {
            return Buffer.Substring(Offset, Count);
        }
    }
}
