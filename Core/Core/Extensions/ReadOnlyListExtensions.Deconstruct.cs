using System;
using System.Collections.Generic;

namespace DotLogix.Core.Extensions {
    public static partial class ReadOnlyListExtensions {
        public static void Deconstruct<T>(this IReadOnlyList<T> values, out T item1) {
            EnsureLength(values, 1);
            item1 = values[0];
        }

        public static void Deconstruct<T>(this IReadOnlyList<T> values, out T item1, out T item2) {
            EnsureLength(values, 2);
            item1 = values[0];
            item2 = values[1];
        }

        public static void Deconstruct<T>(this IReadOnlyList<T> values, out T item1, out T item2, out T item3) {
            EnsureLength(values, 3);
            item1 = values[0];
            item2 = values[1];
            item3 = values[2];
        }

        public static void Deconstruct<T>(this IReadOnlyList<T> values, out T item1, out T item2, out T item3, out T item4) {
            EnsureLength(values, 4);
            item1 = values[0];
            item2 = values[1];
            item3 = values[2];
            item4 = values[3];
        }

        public static void Deconstruct<T>(this IReadOnlyList<T> values, out T item1, out T item2, out T item3, out T item4, out T item5) {
            EnsureLength(values, 5);
            item1 = values[0];
            item2 = values[1];
            item3 = values[2];
            item4 = values[3];
            item5 = values[4];
        }

        public static void Deconstruct<T>(this IReadOnlyList<T> values, out T item1, out T item2, out T item3, out T item4, out T item5, out T item6) {
            EnsureLength(values, 6);
            item1 = values[0];
            item2 = values[1];
            item3 = values[2];
            item4 = values[3];
            item5 = values[4];
            item6 = values[5];
        }

        public static void Deconstruct<T>(this IReadOnlyList<T> values, out T item1, out T item2, out T item3, out T item4, out T item5, out T item6, out T item7) {
            EnsureLength(values, 7);
            item1 = values[0];
            item2 = values[1];
            item3 = values[2];
            item4 = values[3];
            item5 = values[4];
            item6 = values[5];
            item7 = values[6];
        }

        public static void Deconstruct<T>(this IReadOnlyList<T> values, out T item1, out T item2, out T item3, out T item4, out T item5, out T item6, out T item7, out T item8) {
            EnsureLength(values, 8);
            item1 = values[0];
            item2 = values[1];
            item3 = values[2];
            item4 = values[3];
            item5 = values[4];
            item6 = values[5];
            item7 = values[6];
            item8 = values[7];
        }

        public static void Deconstruct<T>(this IReadOnlyList<T> values, out T item1, out T item2, out T item3, out T item4, out T item5, out T item6, out T item7, out T item8, out T item9) {
            EnsureLength(values, 9);
            item1 = values[0];
            item2 = values[1];
            item3 = values[2];
            item4 = values[3];
            item5 = values[4];
            item6 = values[5];
            item7 = values[6];
            item8 = values[7];
            item9 = values[8];
        }

        public static void Deconstruct<T>(this IReadOnlyList<T> values, out T item1, out T item2, out T item3, out T item4, out T item5, out T item6, out T item7, out T item8, out T item9, out T item10) {
            EnsureLength(values, 10);
            item1 = values[0];
            item2 = values[1];
            item3 = values[2];
            item4 = values[3];
            item5 = values[4];
            item6 = values[5];
            item7 = values[6];
            item8 = values[7];
            item9 = values[8];
            item10 = values[9];
        }

        private static void EnsureLength<T>(IReadOnlyList<T> values, int required){
            if(values.Count < required) {
                throw new IndexOutOfRangeException($"Can not deconstruct an values with {values.Count} elements to a tuple with {required} items");          
            }
        }
    }
}