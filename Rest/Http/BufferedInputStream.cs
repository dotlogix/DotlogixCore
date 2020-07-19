using System;
using System.IO;

namespace DotLogix.Core.Rest.Http {
    public class BufferedInputStream : Stream {
        public Stream SourceStream { get; private set; }
        public Stream BufferStream { get; }
        public long? EstimatedLength { get; }

        private readonly byte[] _buffer;
        private bool _isComplete;


        public BufferedInputStream(Stream sourceStream, Stream bufferStream, long? estimatedLength = null, int bufferSize = 2048) {
            if(SourceStream == null)
                throw new ArgumentNullException(nameof(sourceStream));
            if (bufferStream == null)
                throw new ArgumentNullException(nameof(bufferStream));
            if (sourceStream.CanRead == false)
                throw new ArgumentException($"The source stream of type {sourceStream.GetType()} is not readable");
            if (bufferStream.CanWrite == false)
                throw new ArgumentException($"The buffer stream of type {bufferStream.GetType()} is not writable");
            if (bufferStream.CanRead == false)
                throw new ArgumentException($"The buffer stream of type {bufferStream.GetType()} is not readable");
            if (bufferStream.CanSeek == false)
                throw new ArgumentException($"The buffer stream of type {bufferStream.GetType()} is not seekable");

            SourceStream = sourceStream;
            BufferStream = bufferStream;
            EstimatedLength = estimatedLength;
            _buffer = new byte[bufferSize];
            _isComplete = false;
        }

        /// <inheritdoc />
        public override void Flush() {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public override int Read(byte[] buffer, int offset, int count) {
            count = (int)EnsureBuffered(count);
            return BufferStream.Read(buffer, offset, count);
        }

        /// <inheritdoc />
        public override long Seek(long offset, SeekOrigin origin) {
            long targetPosition;
            switch(origin) {
                case SeekOrigin.Begin:
                    targetPosition = offset;

                    if(targetPosition > BufferStream.Length) {
                        EnsureBuffered(targetPosition - BufferStream.Position);
                    }

                    break;
                case SeekOrigin.Current:
                    targetPosition = Position + offset;
                    if (targetPosition > BufferStream.Length) {
                        EnsureBuffered(offset);
                    }
                    break;
                case SeekOrigin.End:
                    if(_isComplete == false) {
                        EnsureBuffered(long.MaxValue);
                    }
                    targetPosition = BufferStream.Length - offset;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(origin), origin, null);
            }

            var minAvailableBytes = (int)(targetPosition - BufferStream.Position);
            var availableBytes = EnsureBuffered(minAvailableBytes);

            if (availableBytes < minAvailableBytes) {
                targetPosition = BufferStream.Position + availableBytes;
            }

            return BufferStream.Seek(targetPosition, SeekOrigin.Begin);
        }

        /// <inheritdoc />
        public override void SetLength(long value) {
            throw new NotSupportedException("Set length is not supported for a readonly stream");
        }

        /// <inheritdoc />
        public override void Write(byte[] buffer, int offset, int count) {
            throw new NotSupportedException("Write is not supported for a readonly stream");
        }

        /// <inheritdoc />
        public override bool CanRead => true;

        /// <inheritdoc />
        public override bool CanSeek => true;

        /// <inheritdoc />
        public override bool CanWrite => false;

        /// <inheritdoc />
        public override long Length => EstimatedLength ?? BufferStream.Length;

        /// <inheritdoc />
        public override long Position {
            get => BufferStream.Position;
            set => Seek(value, SeekOrigin.Begin);
        }

        private long EnsureBuffered(long minAvailableBytes) {
            var currentLength = BufferStream.Length;
            var currentPosition = Position;
            var availableBytes = currentLength - currentPosition;

            if(availableBytes >= minAvailableBytes)
                return minAvailableBytes;

            if(_isComplete)
                return availableBytes;

            BufferStream.Seek(0, SeekOrigin.End);

            while (availableBytes < minAvailableBytes) {
                var read = SourceStream.Read(_buffer, 0, _buffer.Length);
                if (read > 0) {
                    availableBytes += read;
                    BufferStream.Write(_buffer, 0, read);
                }

                if (read < _buffer.Length) {
                    _isComplete = true;
                    SourceStream?.Dispose();
                    SourceStream = null;
                    break;
                }
            }
            BufferStream.Seek(currentPosition, SeekOrigin.Begin);

            return Math.Min(availableBytes, minAvailableBytes);
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing) {
            SourceStream?.Dispose();
            BufferStream?.Dispose();
            base.Dispose(disposing);
        }
    }
}
