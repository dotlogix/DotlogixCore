using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DotLogix.Core.Extensions;
using DotLogix.Core.Rest.Server.Http.Headers;
using DotLogix.Core.Utils;

namespace DotLogix.Core.Rest.Server.Http {
    public class MultiPartParser {
        public static MultiPartParser Instance { get; } = new MultiPartParser();
        private MultiPartParser(){}

        public MultiPartData Parse(Stream stream, string boundary, Encoding encoding = null) {
            if(encoding == null)
                encoding = Encoding.UTF8;

            var boundaryBytes = encoding.GetBytes("--"+boundary);
            var headerSplitBytes = encoding.GetBytes("\r\n\r\n");
            var parts = stream.SplitByArray(boundaryBytes, boundaryBytes.Length+2, stream.Length-2-boundaryBytes.Length).ToArray();

            var partData = new MultiPartData(boundary, stream);
            var headerSeparators = new[]{"\r\n", ": "};
            for(var index = 0; index < parts.Length-1; index++) {
                var part = parts[index];
                var headerEnd = part.IndexOfArray(headerSplitBytes, 0, part.Length);
                var headerData = (new StreamSegment(stream, part.Offset, headerEnd)).ToByteArray();
                var headerStr = encoding.GetString(headerData).Split(headerSeparators, StringSplitOptions.RemoveEmptyEntries);

                ContentDisposition disposition = null;
                MimeType contentType = null;

                for(int i = 0; i < headerStr.Length; i += 2) {
                    switch(headerStr[i].ToLower()) {
                        case "content-disposition":
                            disposition = ContentDisposition.Parse(headerStr[1]);
                            break;
                        case "content-type":
                            contentType = MimeType.Parse(headerStr[1]);
                            break;
                    }
                }

                var data = new StreamSegment(stream, part.Offset + headerEnd + 4, part.Length - headerEnd - 6);
                var value = ParseValue(disposition, contentType, data);
                partData.Values.Add(value);
                if(value.Name != null) {
                    if(partData.NamedValues.TryGetValue(value.Name, out var collection) == false) {
                        collection = new List<MultiPartDataValue>();
                        partData.NamedValues.Add(value.Name, collection);
                    }
                    collection.Add(value);
                }
                    
            }

            return partData;
        }

        public MultiPartData Parse(byte[] data, string boundary, Encoding encoding = null) {
            var memoryStream = new MemoryStream(data);
            return Parse(memoryStream, boundary, encoding);
        }

        private MultiPartDataValue ParseValue(ContentDisposition disposition, MimeType contentType, StreamSegment data) {
            object dataValue = null;
            if(disposition.HasAttribute("filename", true)) {
                dataValue = data;
            }else if(disposition.HasAttribute("boundary", true)) {
                dataValue = Parse(data, disposition.GetAttribute("boundary").Value);
            } else {
                using(var reader = new StreamReader(data, Encoding.UTF8, false, 1024, false)) {
                    dataValue = reader.ReadToEnd();
                }
            }

            return new MultiPartDataValue(disposition, contentType, dataValue);
        }
    }
}