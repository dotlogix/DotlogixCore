using System;
using System.Collections.Generic;

namespace DotLogix.Core.Extensions {
    public static partial class EnumerableExtensions {
        public static void Deconstruct<T>(this IEnumerable<T> enumerable, out T item1) {
            using var enumerator = enumerable.GetEnumerator();
            item1 = EnsureNext(enumerator, 1, 1);
        }
        public static void Deconstruct<T>(this IEnumerable<T> enumerable, out T item1, out T item2) {
            using var enumerator = enumerable.GetEnumerator();
            item1 = EnsureNext(enumerator, 1, 2);
            item2 = EnsureNext(enumerator, 2, 2);
        }
        public static void Deconstruct<T>(this IEnumerable<T> enumerable, out T item1, out T item2, out T item3) {
            using var enumerator = enumerable.GetEnumerator();
            item1 = EnsureNext(enumerator, 1, 3);
            item2 = EnsureNext(enumerator, 2, 3);
            item3 = EnsureNext(enumerator, 3, 3);
        }
        public static void Deconstruct<T>(this IEnumerable<T> enumerable, out T item1, out T item2, out T item3, out T item4) {
            using var enumerator = enumerable.GetEnumerator();
            item1 = EnsureNext(enumerator, 1, 4);
            item2 = EnsureNext(enumerator, 2, 4);
            item3 = EnsureNext(enumerator, 3, 4);
            item4 = EnsureNext(enumerator, 4, 4);
        }
        public static void Deconstruct<T>(this IEnumerable<T> enumerable, out T item1, out T item2, out T item3, out T item4, out T item5) {
            using var enumerator = enumerable.GetEnumerator();
            item1 = EnsureNext(enumerator, 1, 5);
            item2 = EnsureNext(enumerator, 2, 5);
            item3 = EnsureNext(enumerator, 3, 5);
            item4 = EnsureNext(enumerator, 4, 5);
            item5 = EnsureNext(enumerator, 5, 5);
        }
        public static void Deconstruct<T>(this IEnumerable<T> enumerable, out T item1, out T item2, out T item3, out T item4, out T item5, out T item6) {
            using var enumerator = enumerable.GetEnumerator();
            item1 = EnsureNext(enumerator, 1, 6);
            item2 = EnsureNext(enumerator, 2, 6);
            item3 = EnsureNext(enumerator, 3, 6);
            item4 = EnsureNext(enumerator, 4, 6);
            item5 = EnsureNext(enumerator, 5, 6);
            item6 = EnsureNext(enumerator, 6, 6);
        }
        public static void Deconstruct<T>(this IEnumerable<T> enumerable, out T item1, out T item2, out T item3, out T item4, out T item5, out T item6, out T item7) {
            using var enumerator = enumerable.GetEnumerator();
            item1 = EnsureNext(enumerator, 1, 7);
            item2 = EnsureNext(enumerator, 2, 7);
            item3 = EnsureNext(enumerator, 3, 7);
            item4 = EnsureNext(enumerator, 4, 7);
            item5 = EnsureNext(enumerator, 5, 7);
            item6 = EnsureNext(enumerator, 6, 7);
            item7 = EnsureNext(enumerator, 7, 7);
        }
        public static void Deconstruct<T>(this IEnumerable<T> enumerable, out T item1, out T item2, out T item3, out T item4, out T item5, out T item6, out T item7, out T item8) {
            using var enumerator = enumerable.GetEnumerator();
            item1 = EnsureNext(enumerator, 1, 8);
            item2 = EnsureNext(enumerator, 2, 8);
            item3 = EnsureNext(enumerator, 3, 8);
            item4 = EnsureNext(enumerator, 4, 8);
            item5 = EnsureNext(enumerator, 5, 8);
            item6 = EnsureNext(enumerator, 6, 8);
            item7 = EnsureNext(enumerator, 7, 8);
            item8 = EnsureNext(enumerator, 8, 8);
        }
        public static void Deconstruct<T>(this IEnumerable<T> enumerable, out T item1, out T item2, out T item3, out T item4, out T item5, out T item6, out T item7, out T item8, out T item9) {
            using var enumerator = enumerable.GetEnumerator();
            item1 = EnsureNext(enumerator, 1, 9);
            item2 = EnsureNext(enumerator, 2, 9);
            item3 = EnsureNext(enumerator, 3, 9);
            item4 = EnsureNext(enumerator, 4, 9);
            item5 = EnsureNext(enumerator, 5, 9);
            item6 = EnsureNext(enumerator, 6, 9);
            item7 = EnsureNext(enumerator, 7, 9);
            item8 = EnsureNext(enumerator, 8, 9);
            item9 = EnsureNext(enumerator, 9, 9);
        }
        public static void Deconstruct<T>(this IEnumerable<T> enumerable, out T item1, out T item2, out T item3, out T item4, out T item5, out T item6, out T item7, out T item8, out T item9, out T item10) {
            using var enumerator = enumerable.GetEnumerator();
            item1 = EnsureNext(enumerator, 1, 10);
            item2 = EnsureNext(enumerator, 2, 10);
            item3 = EnsureNext(enumerator, 3, 10);
            item4 = EnsureNext(enumerator, 4, 10);
            item5 = EnsureNext(enumerator, 5, 10);
            item6 = EnsureNext(enumerator, 6, 10);
            item7 = EnsureNext(enumerator, 7, 10);
            item8 = EnsureNext(enumerator, 8, 10);
            item9 = EnsureNext(enumerator, 9, 10);
            item10 = EnsureNext(enumerator, 10, 10);
        }
        private static T EnsureNext<T>(IEnumerator<T> enumerator, int count, int required){
            if(enumerator.MoveNext() == false) {
                throw new IndexOutOfRangeException($"Can not deconstruct an enumerable with {count} elements to a tuple with {required} items");          
            }
            return enumerator.Current;
        }
    }
}