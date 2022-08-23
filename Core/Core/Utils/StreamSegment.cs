using System;
using System.IO;
using System.Runtime.CompilerServices;
using DotLogix.Core.Extensions;

namespace DotLogix.Core.Utils {
    /// <summary>
    ///     A segment of a stream
    /// </summary>
    public class StreamSegment : Stream {
        private long _position;

        /// <summary>
        ///     The underlying stream
        /// </summary>
        public Stream Stream { get; }

        /// <summary>
        ///     The offset to the start of the stream
        /// </summary>
        public long Offset { get; }


        /// <inheritdoc />
        public override bool CanRead => Stream.CanRead;

        /// <inheritdoc />
        public override bool CanSeek => Stream.CanSeek;

        /// <inheritdoc />
        public override bool CanWrite => Stream.CanWrite;

        /// <inheritdoc />
        public override long Length { get; }

        /// <inheritdoc />
        public override long Position {
            get => _position;
            set => Seek(value, SeekOrigin.Begin);
        }

        /// <inheritdoc />
        public StreamSegment(Stream stream, long offset, long length) {
            if(stream == null)
                throw new ArgumentNullException(nameof(stream));
            if(stream.CanSeek == false)
                throw new ArgumentException("Stream needs to support seeking to work properly");
            if(offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), "Offset must be greater or equal to zero");
            if(length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), "Offset must be greater or equal to zero");
            if((stream.Length - offset) < length)
                throw new ArgumentException("Stream length is insufficient (length < offset + count)");

            Stream = stream;
            Offset = offset;
            Length = length;
        }


        /// <inheritdoc />
        public override void Flush() { Stream.Flush(); }


        /// <inheritdoc />
        public override int Read(byte[] buffer, int offset, int count) {
            EnsureParamsValid(buffer, offset, count);
            EnsurePosition();
            var read = Stream.Read(buffer, offset, count.Clamp(0, (int)(Length - _position)));
            _position += read;
            return read;
        }


        /// <inheritdoc />
        public override long Seek(long offset, SeekOrigin origin) {
            var absolutePosition = origin switch {
                SeekOrigin.Begin => offset,
                SeekOrigin.Current => _position + offset,
                SeekOrigin.End => Length - offset,
                _ => throw new ArgumentOutOfRangeException(nameof(origin), origin, null)
            };

            if((absolutePosition < 0L) || (absolutePosition > Length))
                throw new IOException("Seek to position outside of segment bounds");
            _position = absolutePosition;
            return absolutePosition;
        }


        /// <inheritdoc />
        public override void SetLength(long value) { throw new NotSupportedException("Segment bounds are fixed"); }


        /// <inheritdoc />
        public override void Write(byte[] buffer, int offset, int count) {
            EnsureParamsValid(buffer, offset, count);
            EnsurePosition();

            if((_position + count) > Length)
                throw new IOException("Write outside of segment bounds");

            Stream.Write(buffer, offset, count);
            _position += count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void EnsureParamsValid(byte[] buffer, int offset, int count) {
            if(buffer == null)
                throw new ArgumentNullException(nameof(buffer), "Buffer is null");
            if(offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), offset, "Offset must be greater or equal to zero");
            if(count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), count, "Offset must be greater or equal to zero");
            if((buffer.Length - offset) < count)
                throw new ArgumentException("Buffer length is insufficient (length < offset + count)");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsurePosition() {
            if(Stream.Position == _position)
                return;
            Stream.Seek(_position + Offset, SeekOrigin.Begin);
        }


        /// <inheritdoc />
        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);
            Stream.Dispose();
        }
    }
}