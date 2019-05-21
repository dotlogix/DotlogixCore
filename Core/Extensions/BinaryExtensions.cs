using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DotLogix.Core.Extensions
{
    public static class BinaryExtensions
    {
        public static int IndexOfArray(this byte[] searchWithin, byte[] serachFor, int startIndex = 0, int count=-1) {
            if(count == -1)
                count = searchWithin.Length - startIndex;

            var endIndex = startIndex + count;
            while((startIndex = Array.IndexOf(searchWithin, serachFor[0], startIndex, endIndex-startIndex-serachFor.Length)) != -1) {
                if(EqualBytesLongUnrolled(searchWithin, serachFor, startIndex, 0, serachFor.Length))
                    return startIndex;
                startIndex++;
            }

            return -1;
        }

        public static IEnumerable<ArraySegment<byte>> SplitByArray(this byte[] searchWithin, byte[] serachFor, int startIndex = 0, int count=-1) {
            if(count == -1)
                count = searchWithin.Length - startIndex;

            var endIndex = startIndex + count;
            int segmentEnd;
            while(endIndex-startIndex > serachFor.Length &&  (segmentEnd = IndexOfArray(searchWithin, serachFor, startIndex, endIndex - startIndex)) != -1) {
                yield return new ArraySegment<byte>(searchWithin, startIndex, segmentEnd-startIndex);
                startIndex = segmentEnd + serachFor.Length;
            }

            if(endIndex - startIndex > 0)
                yield return new ArraySegment<byte>(searchWithin, startIndex, endIndex - startIndex);
        }

        public static bool EqualBytesLongUnrolled(this byte[] data1, byte[] data2) {
            if (data1.Length != data2.Length)
                return false;
            return EqualBytesLongUnrolled(data1, data2, 0, 0, data1.Length);
        }

        public static unsafe bool EqualBytesLongUnrolled (this byte[] data1, byte[] data2, int startIndex, int startIndex2, int count)
        {
            if (data1 == data2 && startIndex == startIndex2)
                return true;
            if (data1.Length < startIndex+count || data2.Length < startIndex2+count)
                return false;

            int len = count;
            int rem = len % (sizeof(long) * 16);
            fixed (byte* bytes1 = data1, bytes2 = data2) {
                long* b1 = (long*)(bytes1 + startIndex);
                long* b2 = (long*)(bytes2 + startIndex2);
                long* e1 = (long*)(bytes1 + startIndex + len - rem);

                while (b1 < e1) {
                    if (*(b1) != *(b2) || *(b1 + 1) != *(b2 + 1) || 
                        *(b1 + 2) != *(b2 + 2) || *(b1 + 3) != *(b2 + 3) ||
                        *(b1 + 4) != *(b2 + 4) || *(b1 + 5) != *(b2 + 5) || 
                        *(b1 + 6) != *(b2 + 6) || *(b1 + 7) != *(b2 + 7) ||
                        *(b1 + 8) != *(b2 + 8) || *(b1 + 9) != *(b2 + 9) || 
                        *(b1 + 10) != *(b2 + 10) || *(b1 + 11) != *(b2 + 11) ||
                        *(b1 + 12) != *(b2 + 12) || *(b1 + 13) != *(b2 + 13) || 
                        *(b1 + 14) != *(b2 + 14) || *(b1 + 15) != *(b2 + 15))
                        return false;
                    b1 += 16;
                    b2 += 16;
                }

                len = rem;
                rem = rem % sizeof(long);
                e1 = (long*)(bytes1 + len - rem);

                while (b1 < e1) {
                    if (*(b1) != *(b2))
                        return false;
                    b1++;
                    b2++;
                }
            }

            for (var i = 0; i < rem; i++)
                if (data1 [startIndex + len - 1 - i] != data2 [startIndex2 + len - 1 - i])
                    return false;
            return true;
        }
    }
}
