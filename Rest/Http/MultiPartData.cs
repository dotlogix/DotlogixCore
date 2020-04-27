using System;
using System.Collections.Generic;
using System.IO;

namespace DotLogix.Core.Rest.Http
{
    public class MultiPartData : IDisposable {
        private readonly Stream _underlayingStream;

        public string Boundary { get; }
        public ICollection<MultiPartDataValue> Values { get; } = new List<MultiPartDataValue>();
        public IDictionary<string, ICollection<MultiPartDataValue>> NamedValues { get; } = new Dictionary<string, ICollection<MultiPartDataValue>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public MultiPartData(string boundary, Stream underlayingStream) {
            _underlayingStream = underlayingStream;
            Boundary = boundary;
        }


        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose() {
            _underlayingStream.Dispose();
        }
    }
}
