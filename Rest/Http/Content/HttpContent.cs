using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes;
using DotLogix.Core.Rest.Http.Headers;

namespace DotLogix.Core.Rest.Http.Content {
    public interface IHttpContent : IDisposable {
        public MimeType ContentType { get; }
        public Encoding Encoding { get; }
        public long ContentLength { get; }

        Task<string> GetStringAsync();
        Task<ArraySegment<byte>> GetBytesAsync();
        Task<byte[]> GetByteArrayAsync();
        Stream AsStream();
    }


    public class HttpContent : IHttpContent {
        private Stream _inputStream;
        public MimeType ContentType { get; }
        public Encoding Encoding { get; private set; }
        public long ContentLength { get; }

        public HttpContent(Stream stream, MimeType contentType) {
            ContentType = contentType;
            _inputStream = stream;
        }

        /// <inheritdoc />
        public async Task<string> GetStringAsync() {
            if (_inputStream.CanSeek) {
                _inputStream.Seek(0, SeekOrigin.Begin);
            }

            using var reader = new StreamReader(_inputStream, Encoding, false, 1_024, true);
            return await reader.ReadToEndAsync();
        }

        /// <inheritdoc />
        public async Task<ArraySegment<byte>> GetBytesAsync() {
            if (_inputStream is MemoryStream memoryStream) {
                return memoryStream.GetBufferSegment();
            }

            if (_inputStream.CanSeek) {
                _inputStream.Seek(0, SeekOrigin.Begin);
            }

            var bytes = await _inputStream.ToByteArrayAsync();
            return new ArraySegment<byte>(bytes);
        }

        /// <inheritdoc />
        public Task<byte[]> GetByteArrayAsync() {
            return _inputStream.ToByteArrayAsync();
        }

        /// <inheritdoc />
        public Stream AsStream() {
            return _inputStream;
        }

        public async Task<MemoryStream> BufferToMemoryAsync() {
            if(_inputStream is MemoryStream memoryStream)
                return memoryStream;

            memoryStream = new MemoryStream();
            await _inputStream.CopyToAsync(memoryStream);
            _inputStream.Dispose();
            _inputStream = memoryStream;
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        public async Task<MemoryStream> BufferToStreamAsync(Stream bufferStream) {
            if(_inputStream is MemoryStream memoryStream)
                return memoryStream;

            memoryStream = new MemoryStream();
            await _inputStream.CopyToAsync(memoryStream);
            _inputStream.Dispose();
            _inputStream = memoryStream;
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        /// <inheritdoc />
        public void Dispose() {
            _inputStream?.Dispose();
        }
    }

    public class JsonHttpContent : HttpContent {
        /// <inheritdoc />
        public JsonHttpContent(Stream stream, MimeType contentType) : base(stream, contentType) { }

        /// <inheritdoc />
        public Node AsNodeAsync() {
            return _inputStream;
        }
    }
}
