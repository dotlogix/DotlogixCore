using DotLogix.Core.Extensions;
using DotLogix.Core.Rest.Server.Http.Headers;

namespace DotLogix.Core.Rest.Server.Http {
    public class MultiPartDataValue {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public MultiPartDataValue(ContentDisposition contentDisposition, MimeType contentType, object data) {
            ContentDisposition = contentDisposition;
            ContentType = contentType;
            Data = data;
            Name = contentDisposition.Attributes.GetValue("name").GetValueOrDefault();
            FileName = contentDisposition.Attributes.GetValue("filename").GetValueOrDefault();
        }
        public ContentDisposition ContentDisposition { get; }
        public MimeType ContentType { get; }
        public string Name { get; }
        public string FileName { get; }
        public bool IsFile => FileName != null;
        public object Data { get; }
    }
}