// ==================================================
// Copyright 2016(C) , DotLogix
// File:  StringExtension.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.09.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace DotLogix.Core.Extensions {
    public static class StringExtension {
        public static int OccurancesOf(this string source, char character) {
            var chars = source.ToCharArray();
            var occurances = 0;
            for(var i = 0; i < chars.Length; i++) {
                if(chars[i] == character)
                    occurances++;
            }
            return occurances;
        }

        public static string ExchangeIfDefaultOrEmpty(this string source, string exchangeValue = default(string)) {
            return string.IsNullOrEmpty(source) ? exchangeValue : source;
        }

        public static string ExchangeIfDefaultOrWhitespace(this string source, string exchangeValue = default(string)) {
            return string.IsNullOrWhiteSpace(source) ? exchangeValue : source;
        }

        public static string SetLength(this string value, char padding, int length, bool padLeft = true) {
            if(value.Length == length)
                return value;

            if(value.Length < length)
                return padLeft ? value.PadLeft(length, padding) : value.PadRight(length, padding);
            return value.Substring(0, length);
        }

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

        public static string ToBase64String(this string plain, Encoding encoding = null) {
            var bytes = (encoding ?? Encoding.UTF8).GetBytes(plain);
            return Convert.ToBase64String(bytes);
        }

        public static string FromBase64String(this string base64string, Encoding encoding = null) {
            var bytes = Convert.FromBase64String(base64string);
            return (encoding ?? Encoding.UTF8).GetString(bytes);
        }

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

        public static IEnumerable<string> SplitAndKeep(this string value, params char[] delimiters) {
            return SplitAndKeep(value, int.MaxValue, StringSplitOptions.None, delimiters);
        }
        public static IEnumerable<string> SplitAndKeep(this string value, StringSplitOptions options, params char[] delimiters) {
            return SplitAndKeep(value, int.MaxValue, options, delimiters);
        }

        public static IEnumerable<string> SplitAndKeep(this string value, int maxCount, params char[] delimiters)
        {
            return SplitAndKeep(value, maxCount, StringSplitOptions.None, delimiters);
        }

        public static IEnumerable<string> SplitAndKeep(this string value, int maxCount, StringSplitOptions options, params char[] delimiters) {
            int nextSplitAt;
            var startIndex = 0;
            var count = 1;
            while(count < maxCount && (nextSplitAt = value.IndexOfAny(delimiters, startIndex)) != -1) {
                var substring = value.Substring(startIndex, nextSplitAt - startIndex);
                if(options != StringSplitOptions.RemoveEmptyEntries || string.IsNullOrEmpty(substring) == false)
                    yield return substring;
                yield return value[nextSplitAt].ToString();
                startIndex = nextSplitAt+1;
                count++;
            }
            yield return value.Substring(startIndex);
        }
    }
}
