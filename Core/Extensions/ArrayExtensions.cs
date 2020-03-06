using System;
using System.Collections.Generic;

namespace DotLogix.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ArrayExtensions
    {

        /// <summary>
        /// Splits an array of bytes using another byte array.
        /// </summary>
        /// <param name="searchWithin">The byte array to search within</param>
        /// <param name="searchFor">The byte array to search</param>
        /// <param name="startIndex">The offset to start searching</param>
        /// <param name="count">The maximum amount bytes to search</param>
        /// <returns>The array segments</returns>
        public static IEnumerable<ArraySegment<T>> Split<T>(this T[] searchWithin, T searchFor, int startIndex = 0, int count = -1)
        {
            if (count == -1)
                count = searchWithin.Length - startIndex;

            var endIndex = startIndex + count;
            for(int i = startIndex; i < endIndex; i++) {
                if(Equals(searchWithin[i], searchFor) == false)
                    continue;

                yield return new ArraySegment<T>(searchWithin, startIndex, i - startIndex);
                startIndex = i+1;
            }

            if (endIndex - startIndex > 0)
                yield return new ArraySegment<T>(searchWithin, startIndex, endIndex - startIndex);
        }
    }
}
