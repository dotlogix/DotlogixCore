using System.IO;
using DotLogix.Core.Rest.Server.Http.Context;
using DotLogix.Core.Rest.Server.Http.Mime;

namespace DotLogix.Core.Rest.Services.Writer {
    public class WebRequestStreamResult {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public WebRequestStreamResult(Stream outputStream, MimeType contentType, bool sendInChunks = false, int chunkSize = AsyncHttpResponse.DefaultChunkSize, TransportModes transportMode=TransportModes.Raw) {
            OutputStream = outputStream;
            ContentType = contentType;
            ChunkSize = chunkSize;
            TransportMode = transportMode;
            SendInChunks = sendInChunks;
        }
        public Stream OutputStream { get; }
        public MimeType ContentType { get; }
        public int ChunkSize { get; }
        public bool SendInChunks { get; }
        public TransportModes TransportMode { get; }
    }

    public enum TransportModes {
        Raw,
        Base64
    }
}