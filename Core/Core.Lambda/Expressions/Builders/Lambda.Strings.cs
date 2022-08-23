

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace DotLogix.Core.Expressions {
    [SuppressMessage("ReSharper", "RedundantTypeArgumentsOfMethod")]
    public static partial class Lambdas {
        private const string DummyInstance = "";
        /// <inheritdoc cref="string.Concat(System.Collections.Generic.IEnumerable{string})"/>
        public static Lambda<string> Concat(this Lambda<IEnumerable<string>> values) {
            return CallStatic(string.Concat, values);
        }

        /// <inheritdoc cref="string.Concat(System.String[])"/>
        public static Lambda<string> Concat(this Lambda<String[]> values) {
            return CallStatic(string.Concat, values);
        }

        /// <inheritdoc cref="string.Concat(string, string)"/>
        public static Lambda<string> Concat(this Lambda<string> str0, Lambda<string> str1) {
            return CallStatic(string.Concat, str0, str1);
        }

        /// <inheritdoc cref="string.Concat(string, string, string)"/>
        public static Lambda<string> Concat(this Lambda<string> str0, Lambda<string> str1, Lambda<string> str2) {
            return CallStatic(string.Concat, str0, str1, str2);
        }

        /// <inheritdoc cref="string.Concat(string, string, string, string)"/>
        public static Lambda<string> Concat(this Lambda<string> str0, Lambda<string> str1, Lambda<string> str2, Lambda<string> str3) {
            return CallStatic(string.Concat, str0, str1, str2, str3);
        }

        /// <inheritdoc cref="string.Concat{T}(System.Collections.Generic.IEnumerable{T})"/>
        public static Lambda<string> Concat<T>(this Lambda<IEnumerable<T>> values) {
            return CallStatic(string.Concat<T>, values);
        }

        /// <inheritdoc cref="string.Contains(string)"/>
        public static Lambda<bool> Contains(this Lambda<string> instance, Lambda<string> value) {
            return Call(instance, DummyInstance.Contains, value);
        }

        /// <inheritdoc cref="string.EndsWith(string)"/>
        public static Lambda<bool> EndsWith(this Lambda<string> instance, Lambda<string> value) {
            return Call(instance, DummyInstance.EndsWith, value);
        }

        /// <inheritdoc cref="string.EndsWith(string, System.StringComparison)"/>
        public static Lambda<bool> EndsWith(this Lambda<string> instance, Lambda<string> value, Lambda<StringComparison> comparisonType) {
            return Call(instance, DummyInstance.EndsWith, value, comparisonType);
        }

        /// <inheritdoc cref="string.EndsWith(string, bool, System.Globalization.CultureInfo)"/>
        public static Lambda<bool> EndsWith(this Lambda<string> instance, Lambda<string> value, Lambda<bool> ignoreCase, Lambda<CultureInfo> culture) {
            return Call(instance, DummyInstance.EndsWith, value, ignoreCase, culture);
        }

        /// <inheritdoc cref="string.Equals(string, string)"/>
        public static Lambda<bool> Equals(this Lambda<string> a, Lambda<string> b) {
            return CallStatic(string.Equals, a, b);
        }

        /// <inheritdoc cref="string.Equals(string, string, System.StringComparison)"/>
        public static Lambda<bool> Equals(this Lambda<string> a, Lambda<string> b, Lambda<StringComparison> comparisonType) {
            return CallStatic(string.Equals, a, b, comparisonType);
        }

        /// <inheritdoc cref="string.IndexOf(char)"/>
        public static Lambda<int> IndexOf(this Lambda<string> instance, Lambda<char> value) {
            return Call(instance, DummyInstance.IndexOf, value);
        }

        /// <inheritdoc cref="string.IndexOf(string)"/>
        public static Lambda<int> IndexOf(this Lambda<string> instance, Lambda<string> value) {
            return Call(instance, DummyInstance.IndexOf, value);
        }

        /// <inheritdoc cref="string.IndexOf(char, int)"/>
        public static Lambda<int> IndexOf(this Lambda<string> instance, Lambda<char> value, Lambda<int> startIndex) {
            return Call(instance, DummyInstance.IndexOf, value, startIndex);
        }

        /// <inheritdoc cref="string.IndexOf(string, int)"/>
        public static Lambda<int> IndexOf(this Lambda<string> instance, Lambda<string> value, Lambda<int> startIndex) {
            return Call(instance, DummyInstance.IndexOf, value, startIndex);
        }

        /// <inheritdoc cref="string.IndexOf(string, System.StringComparison)"/>
        public static Lambda<int> IndexOf(this Lambda<string> instance, Lambda<string> value, Lambda<StringComparison> comparisonType) {
            return Call(instance, DummyInstance.IndexOf, value, comparisonType);
        }

        /// <inheritdoc cref="string.IndexOf(string, int, int)"/>
        public static Lambda<int> IndexOf(this Lambda<string> instance, Lambda<string> value, Lambda<int> startIndex, Lambda<int> count) {
            return Call(instance, DummyInstance.IndexOf, value, startIndex, count);
        }

        /// <inheritdoc cref="string.IndexOf(string, int, System.StringComparison)"/>
        public static Lambda<int> IndexOf(this Lambda<string> instance, Lambda<string> value, Lambda<int> startIndex, Lambda<StringComparison> comparisonType) {
            return Call(instance, DummyInstance.IndexOf, value, startIndex, comparisonType);
        }

        /// <inheritdoc cref="string.IndexOf(char, int, int)"/>
        public static Lambda<int> IndexOf(this Lambda<string> instance, Lambda<char> value, Lambda<int> startIndex, Lambda<int> count) {
            return Call(instance, DummyInstance.IndexOf, value, startIndex, count);
        }

        /// <inheritdoc cref="string.IndexOf(string, int, int, System.StringComparison)"/>
        public static Lambda<int> IndexOf(this Lambda<string> instance, Lambda<string> value, Lambda<int> startIndex, Lambda<int> count, Lambda<StringComparison> comparisonType) {
            return Call(instance, DummyInstance.IndexOf, value, startIndex, count, comparisonType);
        }

        /// <inheritdoc cref="string.IsNullOrEmpty(string)"/>
        public static Lambda<bool> IsNullOrEmpty(this Lambda<string> value) {
            return CallStatic(string.IsNullOrEmpty, value);
        }

        /// <inheritdoc cref="string.IsNullOrWhiteSpace(string)"/>
        public static Lambda<bool> IsNullOrWhiteSpace(this Lambda<string> value) {
            return CallStatic(string.IsNullOrWhiteSpace, value);
        }

        /// <inheritdoc cref="string.Join(string, System.String[])"/>
        public static Lambda<string> Join(this Lambda<string> separator, Lambda<String[]> value) {
            return CallStatic(string.Join, separator, value);
        }

        /// <inheritdoc cref="string.Join(string, System.Collections.Generic.IEnumerable{string})"/>
        public static Lambda<string> Join(this Lambda<string> separator, Lambda<IEnumerable<string>> values) {
            return CallStatic(string.Join, separator, values);
        }

        /// <inheritdoc cref="string.Join(string, System.String[], int, int)"/>
        public static Lambda<string> Join(this Lambda<string> separator, Lambda<String[]> value, Lambda<int> startIndex, Lambda<int> count) {
            return CallStatic(string.Join, separator, value, startIndex, count);
        }

        /// <inheritdoc cref="string.Join{T}(string, System.Collections.Generic.IEnumerable{T})"/>
        public static Lambda<string> Join<T>(this Lambda<string> separator, Lambda<IEnumerable<T>> values) {
            return CallStatic(string.Join<T>, separator, values);
        }

        /// <inheritdoc cref="string.LastIndexOf(char)"/>
        public static Lambda<int> LastIndexOf(this Lambda<string> instance, Lambda<char> value) {
            return Call(instance, DummyInstance.LastIndexOf, value);
        }

        /// <inheritdoc cref="string.LastIndexOf(string)"/>
        public static Lambda<int> LastIndexOf(this Lambda<string> instance, Lambda<string> value) {
            return Call(instance, DummyInstance.LastIndexOf, value);
        }

        /// <inheritdoc cref="string.LastIndexOf(char, int)"/>
        public static Lambda<int> LastIndexOf(this Lambda<string> instance, Lambda<char> value, Lambda<int> startIndex) {
            return Call(instance, DummyInstance.LastIndexOf, value, startIndex);
        }

        /// <inheritdoc cref="string.LastIndexOf(string, int)"/>
        public static Lambda<int> LastIndexOf(this Lambda<string> instance, Lambda<string> value, Lambda<int> startIndex) {
            return Call(instance, DummyInstance.LastIndexOf, value, startIndex);
        }

        /// <inheritdoc cref="string.LastIndexOf(string, System.StringComparison)"/>
        public static Lambda<int> LastIndexOf(this Lambda<string> instance, Lambda<string> value, Lambda<StringComparison> comparisonType) {
            return Call(instance, DummyInstance.LastIndexOf, value, comparisonType);
        }

        /// <inheritdoc cref="string.LastIndexOf(string, int, int)"/>
        public static Lambda<int> LastIndexOf(this Lambda<string> instance, Lambda<string> value, Lambda<int> startIndex, Lambda<int> count) {
            return Call(instance, DummyInstance.LastIndexOf, value, startIndex, count);
        }

        /// <inheritdoc cref="string.LastIndexOf(string, int, System.StringComparison)"/>
        public static Lambda<int> LastIndexOf(this Lambda<string> instance, Lambda<string> value, Lambda<int> startIndex, Lambda<StringComparison> comparisonType) {
            return Call(instance, DummyInstance.LastIndexOf, value, startIndex, comparisonType);
        }

        /// <inheritdoc cref="string.LastIndexOf(char, int, int)"/>
        public static Lambda<int> LastIndexOf(this Lambda<string> instance, Lambda<char> value, Lambda<int> startIndex, Lambda<int> count) {
            return Call(instance, DummyInstance.LastIndexOf, value, startIndex, count);
        }

        /// <inheritdoc cref="string.LastIndexOf(string, int, int, System.StringComparison)"/>
        public static Lambda<int> LastIndexOf(this Lambda<string> instance, Lambda<string> value, Lambda<int> startIndex, Lambda<int> count, Lambda<StringComparison> comparisonType) {
            return Call(instance, DummyInstance.LastIndexOf, value, startIndex, count, comparisonType);
        }

        /// <inheritdoc cref="string.PadLeft(int)"/>
        public static Lambda<string> PadLeft(this Lambda<string> instance, Lambda<int> totalWidth) {
            return Call(instance, DummyInstance.PadLeft, totalWidth);
        }

        /// <inheritdoc cref="string.PadLeft(int, char)"/>
        public static Lambda<string> PadLeft(this Lambda<string> instance, Lambda<int> totalWidth, Lambda<char> paddingChar) {
            return Call(instance, DummyInstance.PadLeft, totalWidth, paddingChar);
        }

        /// <inheritdoc cref="string.PadRight(int)"/>
        public static Lambda<string> PadRight(this Lambda<string> instance, Lambda<int> totalWidth) {
            return Call(instance, DummyInstance.PadRight, totalWidth);
        }

        /// <inheritdoc cref="string.PadRight(int, char)"/>
        public static Lambda<string> PadRight(this Lambda<string> instance, Lambda<int> totalWidth, Lambda<char> paddingChar) {
            return Call(instance, DummyInstance.PadRight, totalWidth, paddingChar);
        }

        /// <inheritdoc cref="string.Remove(int)"/>
        public static Lambda<string> Remove(this Lambda<string> instance, Lambda<int> startIndex) {
            return Call(instance, DummyInstance.Remove, startIndex);
        }

        /// <inheritdoc cref="string.Remove(int, int)"/>
        public static Lambda<string> Remove(this Lambda<string> instance, Lambda<int> startIndex, Lambda<int> count) {
            return Call(instance, DummyInstance.Remove, startIndex, count);
        }

        /// <inheritdoc cref="string.Replace(char, char)"/>
        public static Lambda<string> Replace(this Lambda<string> instance, Lambda<char> oldChar, Lambda<char> newChar) {
            return Call(instance, DummyInstance.Replace, oldChar, newChar);
        }

        /// <inheritdoc cref="string.Replace(string, string)"/>
        public static Lambda<string> Replace(this Lambda<string> instance, Lambda<string> oldValue, Lambda<string> newValue) {
            return Call(instance, DummyInstance.Replace, oldValue, newValue);
        }

        /// <inheritdoc cref="string.Split(System.Char[])"/>
        public static Lambda<String[]> Split(this Lambda<string> instance, Lambda<Char[]> separator) {
            return Call(instance, DummyInstance.Split, separator);
        }

        /// <inheritdoc cref="string.Split(System.Char[], int)"/>
        public static Lambda<String[]> Split(this Lambda<string> instance, Lambda<Char[]> separator, Lambda<int> count) {
            return Call(instance, DummyInstance.Split, separator, count);
        }

        /// <inheritdoc cref="string.Split(System.Char[], System.StringSplitOptions)"/>
        public static Lambda<String[]> Split(this Lambda<string> instance, Lambda<Char[]> separator, Lambda<StringSplitOptions> options) {
            return Call(instance, DummyInstance.Split, separator, options);
        }

        /// <inheritdoc cref="string.Split(System.String[], System.StringSplitOptions)"/>
        public static Lambda<String[]> Split(this Lambda<string> instance, Lambda<String[]> separator, Lambda<StringSplitOptions> options) {
            return Call(instance, DummyInstance.Split, separator, options);
        }

        /// <inheritdoc cref="string.Split(System.Char[], int, System.StringSplitOptions)"/>
        public static Lambda<String[]> Split(this Lambda<string> instance, Lambda<Char[]> separator, Lambda<int> count, Lambda<StringSplitOptions> options) {
            return Call(instance, DummyInstance.Split, separator, count, options);
        }

        /// <inheritdoc cref="string.Split(System.String[], int, System.StringSplitOptions)"/>
        public static Lambda<String[]> Split(this Lambda<string> instance, Lambda<String[]> separator, Lambda<int> count, Lambda<StringSplitOptions> options) {
            return Call(instance, DummyInstance.Split, separator, count, options);
        }

        /// <inheritdoc cref="string.StartsWith(string)"/>
        public static Lambda<bool> StartsWith(this Lambda<string> instance, Lambda<string> value) {
            return Call(instance, DummyInstance.StartsWith, value);
        }

        /// <inheritdoc cref="string.StartsWith(string, System.StringComparison)"/>
        public static Lambda<bool> StartsWith(this Lambda<string> instance, Lambda<string> value, Lambda<StringComparison> comparisonType) {
            return Call(instance, DummyInstance.StartsWith, value, comparisonType);
        }

        /// <inheritdoc cref="string.StartsWith(string, bool, System.Globalization.CultureInfo)"/>
        public static Lambda<bool> StartsWith(this Lambda<string> instance, Lambda<string> value, Lambda<bool> ignoreCase, Lambda<CultureInfo> culture) {
            return Call(instance, DummyInstance.StartsWith, value, ignoreCase, culture);
        }

        /// <inheritdoc cref="string.Substring(int)"/>
        public static Lambda<string> Substring(this Lambda<string> instance, Lambda<int> startIndex) {
            return Call(instance, DummyInstance.Substring, startIndex);
        }

        /// <inheritdoc cref="string.Substring(int, int)"/>
        public static Lambda<string> Substring(this Lambda<string> instance, Lambda<int> startIndex, Lambda<int> length) {
            return Call(instance, DummyInstance.Substring, startIndex, length);
        }

        /// <inheritdoc cref="string.ToLower()"/>
        public static Lambda<string> ToLower(this Lambda<string> instance) {
            return Call(instance, DummyInstance.ToLower);
        }

        /// <inheritdoc cref="string.ToLower(System.Globalization.CultureInfo)"/>
        public static Lambda<string> ToLower(this Lambda<string> instance, Lambda<CultureInfo> culture) {
            return Call(instance, DummyInstance.ToLower, culture);
        }

        /// <inheritdoc cref="string.ToUpper()"/>
        public static Lambda<string> ToUpper(this Lambda<string> instance) {
            return Call(instance, DummyInstance.ToUpper);
        }

        /// <inheritdoc cref="string.ToUpper(System.Globalization.CultureInfo)"/>
        public static Lambda<string> ToUpper(this Lambda<string> instance, Lambda<CultureInfo> culture) {
            return Call(instance, DummyInstance.ToUpper, culture);
        }

        /// <inheritdoc cref="string.Trim()"/>
        public static Lambda<string> Trim(this Lambda<string> instance) {
            return Call(instance, DummyInstance.Trim);
        }

        /// <inheritdoc cref="string.Trim(System.Char[])"/>
        public static Lambda<string> Trim(this Lambda<string> instance, Lambda<Char[]> trimChars) {
            return Call(instance, DummyInstance.Trim, trimChars);
        }

        /// <inheritdoc cref="string.TrimEnd(System.Char[])"/>
        public static Lambda<string> TrimEnd(this Lambda<string> instance, Lambda<Char[]> trimChars) {
            return Call(instance, DummyInstance.TrimEnd, trimChars);
        }

        /// <inheritdoc cref="string.TrimStart(System.Char[])"/>
        public static Lambda<string> TrimStart(this Lambda<string> instance, Lambda<Char[]> trimChars) {
            return Call(instance, DummyInstance.TrimStart, trimChars);
        }

    }
}