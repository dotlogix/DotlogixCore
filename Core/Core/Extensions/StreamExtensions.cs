using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DotLogix.Core.Utils;

namespace DotLogix.Core.Extensions
{
    /// <summary>
    /// A static class providing extension methods for <see cref="Stream"/>
    /// </summary>
    public static class StreamExtensions
    {

        /// <summary>
        /// Returns the array data segment of unsigned bytes from which this stream was created.
        /// </summary>
        /// <returns></returns>
        public static ArraySegment<byte> GetBufferSegment(this MemoryStream memoryStream) {
            var buffer = memoryStream.GetBuffer();
            return new ArraySegment<byte>(buffer, 0, (int)memoryStream.Length);
        }

        /// <summary>
        /// Search the first occurence of a byte in a stream
        /// </summary>
        /// <param name="searchWithin">A seekable stream</param>
        /// <param name="serachFor">The byte to search for</param>
        /// <param name="startIndex">The offset to the start of the stream or 0 to start at the current position</param>
        /// <param name="count">The maximum amount of bytes to search</param>
        /// <returns></returns>
        public static long IndexOf(this Stream searchWithin, byte serachFor, long startIndex = 0L, long count=-1L) {
            if(count == -1L)
                count = searchWithin.Length - startIndex;

            var endIndex = startIndex + count;

            var currentPos = searchWithin.Position;
            if(startIndex != 0L)
                searchWithin.Seek(startIndex, SeekOrigin.Begin);

            var buffer = new byte[1024];
            int read;
            while((read = searchWithin.Read(buffer, 0, buffer.Length)) > 0) {
                long index = Array.IndexOf(buffer, serachFor, 0, read);
                if(index >= 0L) {
                    index = searchWithin.Position - read + index;
                    searchWithin.Seek(currentPos, SeekOrigin.Begin);
                    return index > endIndex ? -1L : index;
                }
            }
            searchWithin.Seek(currentPos, SeekOrigin.Begin);
            return -1;
        }

        /// <summary>
        /// Search the first occurence of a sequence of bytes in a stream
        /// </summary>
        /// <param name="searchWithin">A seekable stream</param>
        /// <param name="serachFor">The sequence to search for</param>
        /// <param name="startIndex">The offset to the start of the stream or 0 to start at the current position</param>
        /// <param name="count">The maximum amount of bytes to search</param>
        /// <returns></returns>
        public static long IndexOfArray(this Stream searchWithin, byte[] serachFor, long startIndex = 0L, long count=-1L) {
            if(count == -1L)
                count = searchWithin.Length - startIndex;

            var endIndex = startIndex + count;
            while((startIndex = searchWithin.IndexOf(serachFor[0], startIndex, endIndex-startIndex-serachFor.Length)) != -1) {
                if(EqualBytesLongUnrolled(searchWithin, serachFor, startIndex, 0, serachFor.Length))
                    return startIndex;
                startIndex++;
            }

            return -1;
        }

        /// <summary>
        /// Splits a stream into multiple sections with an array as delimiter
        /// </summary>
        /// <param name="searchWithin">A seekable stream</param>
        /// <param name="serachFor">The sequence to split</param>
        /// <param name="startIndex">The offset to the start of the stream or 0 to start at the current position</param>
        /// <param name="count">The maximum amount of bytes to search</param>
        /// <returns></returns>
        public static IEnumerable<StreamSegment> SplitByArray(this Stream searchWithin, byte[] serachFor, long startIndex = 0L, long count=-1L) {
            if(count == -1)
                count = searchWithin.Length - startIndex;

            var endIndex = startIndex + count;
            long segmentEnd;
            while(endIndex-startIndex > serachFor.Length &&  (segmentEnd = IndexOfArray(searchWithin, serachFor, startIndex, endIndex - startIndex)) != -1) {
                yield return new StreamSegment(searchWithin, startIndex, segmentEnd-startIndex);
                startIndex = segmentEnd + serachFor.Length;
            }

            if(endIndex - startIndex > 0)
                yield return new StreamSegment(searchWithin, startIndex, endIndex - startIndex);
        }

        /// <summary>
        /// Reads a stream to an array of bytes
        /// </summary>
        /// <param name="stream">A readable stream</param>
        /// <returns></returns>
        public static byte[] ToByteArray(this Stream stream)
        {
            if(stream is MemoryStream mem)
                return mem.ToArray();
            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            return ms.ToArray();
        }

        /// <summary>
        /// Reads a stream to an array of bytes asynchronously
        /// </summary>
        /// <param name="stream">A readable stream</param>
        /// <returns></returns>
        public static async Task<byte[]> ToByteArrayAsync(this Stream stream) {
            if(stream is MemoryStream mem)
                return mem.ToArray();
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            return ms.ToArray();
        }

        /// <summary>
        /// Checks if a stream is equal to a sequence of bytes<br></br>
        /// Uses long pointer comparison instead of byte by byte
        /// </summary>
        /// <param name="data1">The stream</param>
        /// <param name="data2">The array</param>
        /// <returns></returns>
        public static bool EqualBytesLongUnrolled(this Stream data1, byte[] data2) {
            if (data1.Length != data2.Length)
                return false;
            return EqualBytesLongUnrolled(data1, data2, 0, 0, (int)data1.Length);
        }

        /// <summary>
        /// Checks if a stream is equal to a sequence of bytes<br></br>
        /// Uses long pointer comparison instead of byte by byte
        /// </summary>
        /// <param name="data1">The seekable stream</param>
        /// <param name="data2">The array</param>
        /// <param name="startIndex">The offset to the start of the stream</param>
        /// <param name="startIndex2">The offset in the array</param>
        /// <param name="count">The count of bytes to check</param>
        /// <returns></returns>
        public static bool EqualBytesLongUnrolled (this Stream data1, byte[] data2, long startIndex, int startIndex2, int count) {
            var currentPos = data1.Position;
            data1.Seek(startIndex, SeekOrigin.Begin);

            var data1A = new byte[count];
            var read = data1.Read(data1A, 0, count);
            data1.Seek(currentPos, SeekOrigin.Begin);
            return read == count && data1A.EqualBytesLongUnrolled(data2, 0, startIndex2, count);
        }
    }
}
