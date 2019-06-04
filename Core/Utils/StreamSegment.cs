using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using DotLogix.Core.Extensions;

namespace DotLogix.Core.Utils
{
    /// <summary>
    /// A segment of a stream
    /// </summary>
    public class StreamSegment : Stream {
        /// <summary>
        /// The underlying stream
        /// </summary>
        public Stream Stream { get; }
        /// <summary>
        /// The offset to the start of the stream
        /// </summary>
        public long Offset { get; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public StreamSegment(Stream stream, long offset, long length) {
            if(stream == null)
                throw new ArgumentNullException(nameof(stream));
            if(stream.CanSeek == false)
                throw new ArgumentException("Stream needs to support seeking to work properly");
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof (offset), "Offset must be greater or equal to zero");
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof (length), "Offset must be greater or equal to zero");
            if (stream.Length - offset < length)
                throw new ArgumentException("Stream length is insufficient (length < offset + count)");

            Stream = stream;
            Offset = offset;
            Length = length;

        }

        /// <summary>When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device.</summary>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
        public override void Flush() { Stream.Flush(); }

        /// <summary>When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.</summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between offset and (offset + count - 1) replaced by the bytes read from the current source.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.</returns>
        /// <exception cref="T:System.ArgumentException">The sum of <paramref name="offset">offset</paramref> and <paramref name="count">count</paramref> is larger than the buffer length.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="buffer">buffer</paramref> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset">offset</paramref> or <paramref name="count">count</paramref> is negative.</exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
        /// <exception cref="T:System.NotSupportedException">The stream does not support reading.</exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
        public override int Read(byte[] buffer, int offset, int count) {
            EnsureParamsValid(buffer, offset, count);
            EnsurePosition();
            var read = Stream.Read(buffer, offset, count.Clamp(0, (int)(Length - _position)));
            _position += read;
            return read;
        }

        /// <summary>When overridden in a derived class, sets the position within the current stream.</summary>
        /// <param name="offset">A byte offset relative to the origin parameter.</param>
        /// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin"></see> indicating the reference point used to obtain the new position.</param>
        /// <returns>The new position within the current stream.</returns>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
        /// <exception cref="T:System.NotSupportedException">The stream does not support seeking, such as if the stream is constructed from a pipe or console output.</exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
        public override long Seek(long offset, SeekOrigin origin) {
            long absolutePosition;
            switch(origin) {
                case SeekOrigin.Begin:
                    absolutePosition = offset;
                    break;
                case SeekOrigin.Current:
                    absolutePosition = _position+offset;
                    break;
                case SeekOrigin.End:
                    absolutePosition = Length - offset;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(origin), origin, null);
            }
            if(absolutePosition < 0L || absolutePosition > Length)
                throw new IOException("Seek to position outside of segment bounds");
            _position = absolutePosition;
            return absolutePosition;
        }

        /// <summary>When overridden in a derived class, sets the length of the current stream.</summary>
        /// <param name="value">The desired length of the current stream in bytes.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
        /// <exception cref="T:System.NotSupportedException">The stream does not support both writing and seeking, such as if the stream is constructed from a pipe or console output.</exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
        public override void SetLength(long value) { throw new NotSupportedException("Segment bounds are fixed");}

        /// <summary>When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.</summary>
        /// <param name="buffer">An array of bytes. This method copies count bytes from buffer to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        /// <exception cref="T:System.ArgumentException">The sum of <paramref name="offset">offset</paramref> and <paramref name="count">count</paramref> is greater than the buffer length.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="buffer">buffer</paramref> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset">offset</paramref> or <paramref name="count">count</paramref> is negative.</exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occured, such as the specified file cannot be found.</exception>
        /// <exception cref="T:System.NotSupportedException">The stream does not support writing.</exception>
        /// <exception cref="T:System.ObjectDisposedException"><see cref="M:System.IO.Stream.Write(System.Byte[],System.Int32,System.Int32)"></see> was called after the stream was closed.</exception>
        public override void Write(byte[] buffer, int offset, int count) {
            EnsureParamsValid(buffer, offset, count);
            EnsurePosition();

            if(_position + count > Length)
                throw new IOException("Write outside of segment bounds");

            Stream.Write(buffer, offset, count);
            _position += count;
        }

        /// <summary>When overridden in a derived class, gets a value indicating whether the current stream supports reading.</summary>
        /// <returns>true if the stream supports reading; otherwise, false.</returns>
        public override bool CanRead => Stream.CanRead;

        /// <summary>When overridden in a derived class, gets a value indicating whether the current stream supports seeking.</summary>
        /// <returns>true if the stream supports seeking; otherwise, false.</returns>
        public override bool CanSeek => Stream.CanSeek;

        /// <summary>When overridden in a derived class, gets a value indicating whether the current stream supports writing.</summary>
        /// <returns>true if the stream supports writing; otherwise, false.</returns>
        public override bool CanWrite => Stream.CanWrite;

        /// <summary>When overridden in a derived class, gets the length in bytes of the stream.</summary>
        /// <returns>A long value representing the length of the stream in bytes.</returns>
        /// <exception cref="T:System.NotSupportedException">A class derived from Stream does not support seeking.</exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
        public override long Length { get; }

        private long _position;

        /// <summary>When overridden in a derived class, gets or sets the position within the current stream.</summary>
        /// <returns>The current position within the stream.</returns>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
        /// <exception cref="T:System.NotSupportedException">The stream does not support seeking.</exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
        public override long Position {
            get => _position;
            set => Seek(value, SeekOrigin.Begin);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void EnsureParamsValid(byte[] buffer, int offset, int count) {
            if(buffer == null)
                throw new ArgumentNullException(nameof(buffer), "Buffer is null");
            if(offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), offset, "Offset must be greater or equal to zero");
            if(count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), count, "Offset must be greater or equal to zero");
            if(buffer.Length - offset < count)
                throw new ArgumentException("Buffer length is insufficient (length < offset + count)");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsurePosition() {
            if(Stream.Position == _position)
                return;
            Stream.Seek(_position + Offset, SeekOrigin.Begin);
        }

        /// <summary>Releases the unmanaged resources used by the <see cref="T:System.IO.Stream"></see> and optionally releases the managed resources.</summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);
            Stream.Dispose();
        }
    }
}
