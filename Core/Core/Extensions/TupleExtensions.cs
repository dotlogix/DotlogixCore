namespace DotLogix.Core.Extensions {
    public static partial class TupleExtensions {
        public static T[] ToArray<T>(this (T, T) tuple) {
            var array = new T[2];
            (array[0], array[1]) = tuple;
            return array;
        }

        public static T[] ToArray<T>(this (T, T, T) tuple) {
            var array = new T[3];
            (array[0], array[1], array[2]) = tuple;
            return array;
        }

        public static T[] ToArray<T>(this (T, T, T, T) tuple) {
            var array = new T[4];
            (array[0], array[1], array[2], array[3]) = tuple;
            return array;
        }

        public static T[] ToArray<T>(this (T, T, T, T, T) tuple) {
            var array = new T[5];
            (array[0], array[1], array[2], array[3], array[4]) = tuple;
            return array;
        }

        public static T[] ToArray<T>(this (T, T, T, T, T, T) tuple) {
            var array = new T[6];
            (array[0], array[1], array[2], array[3], array[4], array[5]) = tuple;
            return array;
        }

        public static T[] ToArray<T>(this (T, T, T, T, T, T, T) tuple) {
            var array = new T[7];
            (array[0], array[1], array[2], array[3], array[4], array[5], array[6]) = tuple;
            return array;
        }

        public static T[] ToArray<T>(this (T, T, T, T, T, T, T, T) tuple) {
            var array = new T[8];
            (array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7]) = tuple;
            return array;
        }

        public static T[] ToArray<T>(this (T, T, T, T, T, T, T, T, T) tuple) {
            var array = new T[9];
            (array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7], array[8]) = tuple;
            return array;
        }

        public static T[] ToArray<T>(this (T, T, T, T, T, T, T, T, T, T) tuple) {
            var array = new T[10];
            (array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7], array[8], array[9]) = tuple;
            return array;
        }

        public static object[] ToObjectArray<T1, T2>(this (T1, T2) tuple) {
            var array = new object[2];
            (array[0], array[1]) = tuple;
            return array;
        }

        public static object[] ToObjectArray<T1, T2, T3>(this (T1, T2, T3) tuple) {
            var array = new object[3];
            (array[0], array[1], array[2]) = tuple;
            return array;
        }

        public static object[] ToObjectArray<T1, T2, T3, T4>(this (T1, T2, T3, T4) tuple) {
            var array = new object[4];
            (array[0], array[1], array[2], array[3]) = tuple;
            return array;
        }

        public static object[] ToObjectArray<T1, T2, T3, T4, T5>(this (T1, T2, T3, T4, T5) tuple) {
            var array = new object[5];
            (array[0], array[1], array[2], array[3], array[4]) = tuple;
            return array;
        }

        public static object[] ToObjectArray<T1, T2, T3, T4, T5, T6>(this (T1, T2, T3, T4, T5, T6) tuple) {
            var array = new object[6];
            (array[0], array[1], array[2], array[3], array[4], array[5]) = tuple;
            return array;
        }

        public static object[] ToObjectArray<T1, T2, T3, T4, T5, T6, T7>(this (T1, T2, T3, T4, T5, T6, T7) tuple) {
            var array = new object[7];
            (array[0], array[1], array[2], array[3], array[4], array[5], array[6]) = tuple;
            return array;
        }

        public static object[] ToObjectArray<T1, T2, T3, T4, T5, T6, T7, T8>(this (T1, T2, T3, T4, T5, T6, T7, T8) tuple) {
            var array = new object[8];
            (array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7]) = tuple;
            return array;
        }

        public static object[] ToObjectArray<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9) tuple) {
            var array = new object[9];
            (array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7], array[8]) = tuple;
            return array;
        }

        public static object[] ToObjectArray<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) tuple) {
            var array = new object[10];
            (array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7], array[8], array[9]) = tuple;
            return array;
        }

    }
}