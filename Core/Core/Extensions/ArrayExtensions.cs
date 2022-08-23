using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotLogix.Core.Extensions; 

/// <summary>
/// 
/// </summary>
public static partial class ArrayExtensions
{
    /// <summary>
    /// Splits an array of bytes using another byte array.
    /// </summary>
    /// <param name="searchWithin">The byte array to search within</param>
    /// <param name="searchFor">The byte array to search</param>
    /// <param name="startIndex">The offset to start searching</param>
    /// <param name="count">The maximum amount bytes to search</param>
    /// <returns>The array segments</returns>
    public static IEnumerable<ArraySegment<T>> Split<T>(this T[] searchWithin, T searchFor, int startIndex = 0, int count = -1) {
        var array = new[] {1, 2, 3, 4, 5, 6};
        var tuple = (1, 2, 3, 4, 5, 6);
        var (a, b, c, d, e, f) = tuple.ToArray();

        (a, b, c, d, e, f) = array;
            
            
        if (count == -1)
            count = searchWithin.Length - startIndex;

        var endIndex = startIndex + count;
        for(var i = startIndex; i < endIndex; i++) {
            if(Equals(searchWithin[i], searchFor) == false)
                continue;

            yield return new ArraySegment<T>(searchWithin, startIndex, i - startIndex);
            startIndex = i+1;
        }

        if (endIndex - startIndex > 0)
            yield return new ArraySegment<T>(searchWithin, startIndex, endIndex - startIndex);
    }

    /// <summary>
    ///     Initializes every element of the <see cref="T:System.Array"></see> with the provided value.
    /// </summary>
    /// <param name="array">The array</param>
    /// <param name="value">The value</param>
    /// <returns></returns>
    public static T[] Initialize<T>(this T[] array, T value) {
        return Initialize(array, value, 0, array.Length);
    }

    /// <summary>
    ///     Initializes every element of the <see cref="T:System.Array"></see> with the provided value.
    /// </summary>
    /// <param name="array">The array</param>
    /// <param name="value">The value</param>
    /// <param name="index">The start index</param>
    /// <param name="count">The amount of elements to set</param>
    /// <returns></returns>
    public static T[] Initialize<T>(this T[] array, T value, int index, int count) {
        var currentCount = Math.Min(count, 8);
        var offset = index;
        for (; offset < currentCount; offset++)
            array[offset] = value;
			
        while(offset < array.Length) {
            Array.Copy(array, index, array, offset, Math.Min(currentCount, array.Length - offset));
            offset += currentCount;
            currentCount <<= 1;
        }
        return array;
    }

		
    /// <summary>
    ///     Creates a <see cref="Array"></see> by repeating the value n times
    /// </summary>
    /// <param name="value">The value</param>
    /// <param name="count">The amount of elements in the array</param>
    /// <returns></returns>
    public static T[] CreateArray<T>(this T value, int count = 1) {
        return new T[count].Initialize(value);
    }

    /// <summary>
    ///     Creates a <see cref="IEnumerable{T}" /> by repeating the value n times
    /// </summary>
    /// <param name="value">The value</param>
    /// <param name="count">The amount of elements in the list</param>
    /// <returns></returns>
    public static Task<T[]> CreateArray<T>(this Task<T> value, int count = 1) {
        return value.TransformAsync(v => v.Result.CreateArray(count));
    }
}