// ==================================================
// Copyright 2018(C) , DotLogix
// File:  StringExtension.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  04.06.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Text;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Extensions {
    /// <summary>
    /// A static class providing extension methods for <see cref="string"/>
    /// </summary>
    public static class StringExtensions {
        /// <summary>
        ///     Counts the occurrences of a character in a string.
        /// </summary>
        /// <param name="str">The string</param>
        /// <param name="character">The character to count</param>
        public static int OccurrencesOf(this string str, char character) {
            return OccurrencesOf(str.ToCharArray(), character, 0, str.Length, out _);
        }

        /// <summary>
        ///     Counts the occurrences of a character in a string.
        /// </summary>
        /// <param name="str">The string</param>
        /// <param name="character">The character to count</param>
        /// <param name="start">The start index in the array</param>
        /// <param name="count">The maximum length to count</param>
        public static int OccurrencesOf(this string str, char character, int start, int count) {
            return OccurrencesOf(str.ToCharArray(), character, start, count, out _);
        }

        /// <summary>
        ///     Counts the occurrences of a character in a char array.
        /// </summary>
        /// <param name="chars">The char array</param>
        /// <param name="character">The character to count</param>
        public static int OccurrencesOf(this char[] chars, char character) {
            return OccurrencesOf(chars, character, 0, chars.Length, out _);
        }

        /// <summary>
        ///     Counts the occurrences of a character in a char array.
        /// </summary>
        /// <param name="chars">The char array</param>
        /// <param name="character">The character to count</param>
        /// <param name="start">The start index in the array</param>
        /// <param name="count">The maximum length to count</param>
        /// <returns></returns>
        public static int OccurrencesOf(this char[] chars, char character, int start, int count) {
            return OccurrencesOf(chars, character, start, count, out _);
        }

        /// <summary>
        ///     Counts the occurrences of a character in a char array. Also outputs the position of the last occurance of the
        ///     character
        /// </summary>
        /// <param name="chars">The char array</param>
        /// <param name="character">The character to count</param>
        /// <param name="start">The start index in the array</param>
        /// <param name="count">The maximum length to count</param>
        /// <param name="lastOccurence">The position of the last occurance of the character</param>
        /// <returns></returns>
        public static int OccurrencesOf(this char[] chars, char character, int start, int count, out int lastOccurence) {
            lastOccurence = -1;

            var occurrences = 0;
            for(var i = start; i < (start + count); i++) {
                if(chars[i] != character)
                    continue;

                occurrences++;
                lastOccurence = i;
            }
            return occurrences;
        }

        /// <summary>
        ///     exchanges the value of source with another value if it is null, empty
        /// </summary>
        /// <param name="source">The string value</param>
        /// <param name="exchangeValue">The value to exchange</param>
        /// <returns></returns>
        public static string ExchangeIfDefaultOrEmpty(this string source, string exchangeValue = default) {
            return string.IsNullOrEmpty(source) ? exchangeValue : source;
        }

        /// <summary>
        ///     exchanges the value of source with another value if it is null, empty or only contains whitespaces
        /// </summary>
        /// <param name="source">The string value</param>
        /// <param name="exchangeValue">The value to exchange</param>
        /// <returns></returns>
        public static string ExchangeIfDefaultOrWhitespace(this string source, string exchangeValue = default) {
            return string.IsNullOrWhiteSpace(source) ? exchangeValue : source;
        }

        /// <summary>
        ///     Set the length of string to a specified length. If the string is longer than the string is cut in the middle, else
        ///     it will be filled with the padding character
        /// </summary>
        /// <param name="value">The string value</param>
        /// <param name="padding">The padding character</param>
        /// <param name="length">The target length of the string</param>
        /// <param name="padLeft">Determines if padding should be applied left or right</param>
        /// <returns></returns>
        public static string SetLength(this string value, char padding, int length, bool padLeft = true) {
            if(value.Length == length)
                return value;

            if(value.Length < length)
                return padLeft ? value.PadLeft(length, padding) : value.PadRight(length, padding);
            return value.Substring(0, length);
        }

        /// <summary>
        /// Wraps a line of text after at a maximum amount of characters.<br></br>
        /// Line breaks will be inserted after the last possible word
        /// </summary>
        /// <returns></returns>
        public static string[] WordWrap(this string value, int wrapAfter) {
            var result = new List<string>();
            var lines = value.Split('\n');

            foreach(var line in lines) {
                if(line.Length <= wrapAfter) {
                    result.Add(line);
                    continue;
                }

                var words = line.Split(' ');
                var buffer = "";

                var bufferLength = 0;
                var idx = 0;
                while(idx < words.Length) {
                    var word = words[idx];
                    var wordLength = word.Length;
                    if(bufferLength == 0) {
                        while(wordLength > wrapAfter) {
                            buffer = word.Substring(0, wrapAfter);
                            result.Add(buffer);
                            word = word.Substring(wrapAfter);
                            buffer = "";
                            wordLength = word.Length;
                        }
                        buffer += word;
                        bufferLength += wordLength;
                        idx++;
                        continue;
                    }

                    if((bufferLength + 1 + wordLength) > wrapAfter) {
                        result.Add(buffer);
                        buffer = "";
                        bufferLength = 0;
                    } else {
                        buffer += " " + word;
                        bufferLength += 1 + wordLength;
                        idx++;
                    }
                }
                result.Add(buffer);
            }
            return result.ToArray();
        }

        #region Join
        /// <summary>
        /// Appends some values to a <see cref="StringBuilder"/> and places a separator between each item
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static StringBuilder AppendJoin<T>(this StringBuilder sb, string separator, IEnumerable<T> values) {
            if(values == null)
                throw new ArgumentNullException(nameof(values));

            separator ??= string.Empty;
            using var enumerator = values.GetEnumerator();

            if(!enumerator.MoveNext())
                return sb;

            if(enumerator.Current != null)
                sb.Append(enumerator.Current);
            while(enumerator.MoveNext()) {
                sb.Append(separator);
                if(enumerator.Current != null)
                    sb.Append(enumerator.Current);
            }
            return sb;
        }
        
        /// <summary>
        /// Appends some values to a <see cref="StringBuilder"/> and places a separator between each item
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static StringBuilder AppendJoin<T>(this StringBuilder sb, char separator, IEnumerable<T> values) {
            if(values == null)
                throw new ArgumentNullException(nameof(values));

            using var enumerator = values.GetEnumerator();

            if(!enumerator.MoveNext())
                return sb;

            if(enumerator.Current != null)
                sb.Append(enumerator.Current);
            while(enumerator.MoveNext()) {
                sb.Append(separator);
                if(enumerator.Current != null)
                    sb.Append(enumerator.Current);
            }
            return sb;
        }
        #endregion

        #region Base64
        /// <summary>
        ///     Converts a string to its base64 encoded version
        /// </summary>
        /// <param name="plain">The string to convert</param>
        /// <param name="encoding">The encoding used to transform the string into bytes (default is UTF8)</param>
        /// <returns></returns>
        public static string EncodeBase64(string plain, Encoding encoding = null) {
            if(plain == null)
                throw new ArgumentNullException(nameof(plain));
            var bytes = (encoding ?? Encoding.UTF8).GetBytes(plain);
            return EncodeBase64(bytes);
        }

        /// <summary>
        ///     Converts a byte[] to its base64 encoded version
        /// </summary>
        /// <param name="bytes">The bytes to convert</param>
        /// <returns></returns>
        public static string EncodeBase64(byte[] bytes) {
            if(bytes == null)
                throw new ArgumentNullException(nameof(bytes));
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        ///     Converts a string back of its base64 encoded version
        /// </summary>
        /// <param name="base64String">The base64 encoded string</param>
        /// <param name="encoding">The encoding used to transform the string into bytes (default is UTF8)</param>
        /// <returns></returns>
        public static string DecodeBase64(string base64String, Encoding encoding = null) {
            if(base64String == null)
                throw new ArgumentNullException(nameof(base64String));
            var bytes = DecodeBase64ToArray(base64String);
            return (encoding ?? Encoding.UTF8).GetString(bytes);
        }

        /// <summary>
        ///     Converts a string back of its base64 encoded version
        /// </summary>
        /// <param name="base64String">The base64 encoded string</param>
        /// <returns></returns>
        public static byte[] DecodeBase64ToArray(string base64String) {
            if(base64String == null)
                throw new ArgumentNullException(nameof(base64String));
            return Convert.FromBase64String(base64String);
        }
        #endregion

        #region Base64Url

        /// <summary>
        ///     Converts a string to its base64 url encoded version
        /// </summary>
        /// <param name="plain">The string to convert</param>
        /// <param name="encoding">The encoding used to transform the string into bytes (default is UTF8)</param>
        /// <returns></returns>
        public static string EncodeBase64Url(string plain, Encoding encoding = null) {
            if(plain == null)
                throw new ArgumentNullException(nameof(plain));
            var bytes = (encoding ?? Encoding.UTF8).GetBytes(plain);
            return EncodeBase64Url(bytes);
        }

        /// <summary>
        ///     Converts a byte[] to its base64 url encoded version
        /// </summary>
        /// <param name="bytes">The bytes to convert</param>
        /// <returns></returns>
        public static string EncodeBase64Url(byte[] bytes) {
            if(bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            var arraySize = ((bytes.Length + 2) / 3) * 4;
            var chrArray = new char[arraySize];

            // Start with default Base64 encoding.
            var count = Convert.ToBase64CharArray(bytes, 0, bytes.Length, chrArray, 0);

            // Fix up '+' -> '-' and '/' -> '_'. Drop padding characters.
            for (var i = 0; i < count; i++) {
                var ch = chrArray[i];
                switch(ch) {
                    case '+':
                        chrArray[i] = '-';
                        break;
                    case '/':
                        chrArray[i] = '_';
                        break;
                    case '=':
                        return new string(chrArray, 0, i);
                    default:
                        continue;
                }
            }
            return new string(chrArray);
        }

        /// <summary>
        ///     Converts a string back of its base64 url encoded version
        /// </summary>
        /// <param name="base64String">The base64 url encoded string</param>
        /// <param name="encoding">The encoding used to transform the string into bytes (default is UTF8)</param>
        /// <returns></returns>
        public static string DecodeBase64Url(string base64String, Encoding encoding = null) {
            if(base64String == null)
                throw new ArgumentNullException(nameof(base64String));
            var bytes = DecodeBase64UrlToArray(base64String);
            return (encoding ?? Encoding.UTF8).GetString(bytes);
        }

        /// <summary>
        ///     Converts a string back of its base64 url encoded version
        /// </summary>
        /// <param name="base64String">The base64 url encoded string</param>
        /// <returns></returns>
        public static byte[] DecodeBase64UrlToArray(string base64String) {
            if(base64String == null)
                throw new ArgumentNullException(nameof(base64String));

            var arraySize = base64String.Length;
            switch (base64String.Length % 4)
            {
                case 0:
                    break;
                case 2:
                    arraySize += 2;
                    break;
                case 3:
                    arraySize += 1;
                    break;
            }

            var chrArray = new char[arraySize];

            for (var i = 0; i < base64String.Length; i++)
            {
                var ch = base64String[i];
                if (ch == '-')
                {
                    chrArray[i] = '+';
                }
                else if (ch == '_')
                {
                    chrArray[i] = '/';
                }
                else
                {
                    chrArray[i] = ch;
                }
            }

            for (var i = base64String.Length; i < chrArray.Length; i++)
            {
                chrArray[i] = '=';
            }
            
            // Decode.
            // If the caller provided invalid base64 chars, they'll be caught here.
            return Convert.FromBase64CharArray(chrArray, 0, chrArray.Length);

        }

        #endregion

        #region SplitAndKeep
        /// <summary>
        ///     Same as string.Split, but keeps the delimiter as an own entry in the resulting enumerable
        /// </summary>
        /// <param name="value">The string to split</param>
        /// <param name="delimiters">The delimiters</param>
        /// <returns></returns>
        public static IEnumerable<string> SplitAndKeep(this string value, params char[] delimiters) {
            return SplitAndKeep(value, int.MaxValue, StringSplitOptions.None, delimiters);
        }

        /// <summary>
        ///     Same as string.Split, but keeps the delimiter as an own entry in the resulting enumerable
        /// </summary>
        /// <param name="value">The string to split</param>
        /// <param name="options">The options used for splitting</param>
        /// <param name="delimiters">The delimiters</param>
        /// <returns></returns>
        public static IEnumerable<string> SplitAndKeep(this string value, StringSplitOptions options, params char[] delimiters) {
            return SplitAndKeep(value, int.MaxValue, options, delimiters);
        }

        /// <summary>
        ///     Same as string.Split, but keeps the delimiter as an own entry in the resulting enumerable
        /// </summary>
        /// <param name="value">The string to split</param>
        /// <param name="maxCount">The maxmimum count of splits</param>
        /// <param name="delimiters">The delimiters</param>
        public static IEnumerable<string> SplitAndKeep(this string value, int maxCount, params char[] delimiters) {
            return SplitAndKeep(value, maxCount, StringSplitOptions.None, delimiters);
        }

        /// <summary>
        ///     Same as string.Split, but keeps the delimiter as an own entry in the resulting enumerable
        /// </summary>
        /// <param name="value">The string to split</param>
        /// <param name="maxCount">The maxmimum count of splits</param>
        /// <param name="options">The options used for splitting</param>
        /// <param name="delimiters">The delimiters</param>
        /// <returns></returns>
        public static IEnumerable<string> SplitAndKeep(this string value, int maxCount, StringSplitOptions options, params char[] delimiters) {
            int nextSplitAt;
            var startIndex = 0;
            var count = 1;
            while((count < maxCount) && ((nextSplitAt = value.IndexOfAny(delimiters, startIndex)) != -1)) {
                var substring = value.Substring(startIndex, nextSplitAt - startIndex);
                if((options != StringSplitOptions.RemoveEmptyEntries) || (string.IsNullOrEmpty(substring) == false))
                    yield return substring;
                yield return value[nextSplitAt].ToString();
                startIndex = nextSplitAt + 1;
                count++;
            }
            yield return value.Substring(startIndex);
        }
        #endregion

        #region Parse
        /// <summary>
        ///     Convert a string to another type. Only works for the conversion of a string to primitives
        /// </summary>
        /// <param name="targetType">The target type</param>
        /// <param name="value">The string representation of the type</param>
        /// <returns>The result value if the conversion succeed</returns>
        public static object ParseTo(this string value, Type targetType) {
            if(TryParseTo(value, targetType, out var convertedValue) == false)
                throw new NotSupportedException($"Conversion between {value.GetType()} and {targetType} is not supported");
            return convertedValue;
        }

        /// <summary>
        ///     Tries to convert a string to another type. Only works for the conversion of a string to primitives
        /// </summary>
        /// <param name="targetType">The target type</param>
        /// <param name="value">The string representation of the type</param>
        /// <param name="target">The result value if the conversion succeed</param>
        /// <returns></returns>
        public static bool TryParseTo(this string value, Type targetType, out object target) {
            target = null;
            var dataType = targetType.ToDataType();
            if ((dataType.Flags & DataTypeFlags.Primitive) == 0)
                return false;

            bool result;
            switch (dataType.Flags & DataTypeFlags.PrimitiveMask) {
                case DataTypeFlags.Guid:
                    result = Guid.TryParse(value, out var guid);
                    target = guid;
                    break;
                case DataTypeFlags.Bool:
                    result = bool.TryParse(value, out var bo);
                    target = bo;
                    break;
                case DataTypeFlags.Char:
                    result = char.TryParse(value, out var c);
                    target = c;
                    break;
                case DataTypeFlags.Enum:
                    try {
                        target = Enum.Parse(targetType, value);
                        result = true;
                    } catch {
                        try {
                            target = Enum.Parse(targetType, value, true);
                            result = true;
                        } catch {
                            target = null;
                            result = false;
                        }
                    }
                    break;
                case DataTypeFlags.SByte:
                    result = sbyte.TryParse(value, out var sb);
                    target = sb;
                    break;
                case DataTypeFlags.Byte:
                    result = byte.TryParse(value, out var b);
                    target = b;
                    break;
                case DataTypeFlags.Short:
                    result = short.TryParse(value, out var s);
                    target = s;
                    break;
                case DataTypeFlags.UShort:
                    result = ushort.TryParse(value, out var us);
                    target = us;
                    break;
                case DataTypeFlags.Int:
                    result = int.TryParse(value, out var i);
                    target = i;
                    break;
                case DataTypeFlags.UInt:
                    result = uint.TryParse(value, out var ui);
                    target = ui;
                    break;
                case DataTypeFlags.Long:
                    result = long.TryParse(value, out var l);
                    target = l;
                    break;
                case DataTypeFlags.ULong:
                    result = ulong.TryParse(value, out var ul);
                    target = ul;
                    break;
                case DataTypeFlags.Float:
                    result = float.TryParse(value, out var f);
                    target = f;
                    break;
                case DataTypeFlags.Double:
                    result = double.TryParse(value, out var d);
                    target = d;
                    break;
                case DataTypeFlags.Decimal:
                    result = decimal.TryParse(value, out var dec);
                    target = dec;
                    break;
                case DataTypeFlags.DateTime:
                    result = DateTime.TryParse(value, out var dt);
                    target = dt;
                    break;
                case DataTypeFlags.DateTimeOffset:
                    result = DateTimeOffset.TryParse(value, out var dto);
                    target = dto;
                    break;
                case DataTypeFlags.TimeSpan:
                    result = TimeSpan.TryParse(value, out var ts);
                    target = ts;
                    break;
                case DataTypeFlags.String:
                    target = value;
                    result = true;
                    break;
                default:
                    return false;
            }
            return result;
        }

        /// <summary>
        ///     Convert a string to another type. Only works for the conversion of a string to primitives
        /// </summary>
        /// <typeparam name="TTarget">The target type</typeparam>
        /// <param name="value">The string representation of the type</param>
        /// <returns>The result value if the conversion succeed</returns>
        public static TTarget ParseTo<TTarget>(this string value) {
            var targetType = typeof(TTarget);
            return (TTarget)ParseTo(value, targetType);
        }

        /// <summary>
        ///     Tries to convert a string to another type. Only works for the conversion of a string to primitives
        /// </summary>
        /// <typeparam name="TTarget">The target type</typeparam>
        /// <param name="value">The string representation of the type</param>
        /// <param name="target">The result value if the conversion succeed</param>
        /// <returns></returns>
        public static bool TryParseTo<TTarget>(this string value, out TTarget target) {
            var targetType = typeof(TTarget);
            if(TryParseTo(value, targetType, out var convertedValue)) {
                target = (TTarget)convertedValue;
                return true;
            }
            target = default;
            return false;
        }

        /// <summary>
        ///     Try to convert a string to the provided enum type
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum</typeparam>
        /// <param name="enumValue">The string representation of a enum value</param>
        /// <param name="ignoreCase">Determines if the parsing should be case sensitive</param>
        /// <returns>The parsed enum value</returns>
        public static TEnum ConvertToEnum<TEnum>(this string enumValue, bool ignoreCase = true) where TEnum : struct {
            if(enumValue == null)
                throw new ArgumentNullException(nameof(enumValue));

            var enumType = typeof(TEnum);
            if(!enumType.IsEnum)
                throw new ArgumentException("Type must be an enum", nameof(TEnum));
            if(Enum.TryParse(enumValue, ignoreCase, out TEnum value))
                return value;
            throw new InvalidCastException($"{enumValue} is not a valid value of enum {enumType.GetFriendlyName()}");
        }
        #endregion

        #region SubStringUntil
        /// <summary>
        /// Get the value between two <see cref="string"/> delimiters
        /// </summary>
        /// <returns></returns>
        public static string SubstringBetween(this string value, string startValue, string endValue, int startIndex=0, int count=-1, StringComparison comparison = StringComparison.OrdinalIgnoreCase) {
            if(count < 0)
                count = value.Length-startIndex;

            startIndex = value.IndexOf(startValue, startIndex, count, comparison);
            if(startIndex == -1)
                return null;

            startIndex += startValue.Length;
            var endIndex = value.IndexOf(endValue, startIndex + 1, count - startIndex - 1, comparison);
            if(endIndex == -1)
                return null;
            return value.Substring(startIndex, endIndex - startIndex);
        }

        /// <summary>
        /// Get the value between before a any of the <see cref="char"/> delimiter
        /// </summary>
        /// <returns></returns>
        public static string SubstringUntil(this string value, int start, int count, params char[] delimiters) {
            var idx = value.IndexOfAny(delimiters, start, count);
            if(idx == -1)
                return (start > 0) || ((count > 0) && (count < value.Length)) ? value.Substring(start, count) : value;
            return value.Substring(start, idx - start);
        }

        /// <summary>
        /// Get the value between before a any of the <see cref="char"/> delimiter
        /// </summary>
        /// <returns></returns>
        public static string SubstringUntil(this string value, params char[] delimiters) {
            var idx = value.IndexOfAny(delimiters);
            if(idx == -1)
                return value;
            return value.Substring(0, idx);
        }
        #endregion
    }
}
