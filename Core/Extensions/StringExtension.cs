// ==================================================
// Copyright 2018(C) , DotLogix
// File:  StringExtension.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Text;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Extensions {
    public static class StringExtension {
        public static int OccurancesOf(this string str, char character) {
            return OccurancesOf(str.ToCharArray(), character, 0, str.Length, out _);
        }

        public static int OccurancesOf(this string str, char character, int start, int count) {
            return OccurancesOf(str.ToCharArray(), character, start, count, out _);
        }

        public static int OccurancesOf(this char[] chars, char character) {
            return OccurancesOf(chars, character, 0, chars.Length, out _);
        }

        public static int OccurancesOf(this char[] chars, char character, int start, int count) {
            return OccurancesOf(chars, character, start, count, out _);
        }

        public static int OccurancesOf(this char[] chars, char character, int start, int count, out int lastOccurence) {
            lastOccurence = -1;

            var occurances = 0;
            for(var i = start; i < (start + count); i++) {
                if(chars[i] != character)
                    continue;

                occurances++;
                lastOccurence = i;
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

        #region Base64
        public static string ToBase64String(this string plain, Encoding encoding = null) {
            var bytes = (encoding ?? Encoding.UTF8).GetBytes(plain);
            return Convert.ToBase64String(bytes);
        }

        public static string FromBase64String(this string base64string, Encoding encoding = null) {
            var bytes = Convert.FromBase64String(base64string);
            return (encoding ?? Encoding.UTF8).GetString(bytes);
        }
        #endregion

        #region SplitAndKeep
        public static IEnumerable<string> SplitAndKeep(this string value, params char[] delimiters) {
            return SplitAndKeep(value, int.MaxValue, StringSplitOptions.None, delimiters);
        }

        public static IEnumerable<string> SplitAndKeep(this string value, StringSplitOptions options, params char[] delimiters) {
            return SplitAndKeep(value, int.MaxValue, options, delimiters);
        }

        public static IEnumerable<string> SplitAndKeep(this string value, int maxCount, params char[] delimiters) {
            return SplitAndKeep(value, maxCount, StringSplitOptions.None, delimiters);
        }

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
        public static object ParseTo(this string value, Type targetType) {
            if(TryParseTo(value, targetType, out var convertedValue) == false) {
                throw new
                NotSupportedException($"Conversion between {value.GetType()} and {targetType} is not supported");
            }
            return convertedValue;
        }

        public static bool TryParseTo(this string value, Type targetType, out object target) {
            target = null;
            var dataType = targetType.ToDataType();
            if((dataType.Flags & DataTypeFlags.Primitive) == 0)
                return false;

            bool result;
            switch(dataType.Flags & DataTypeFlags.PrimitiveMask) {
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
                    target = Enum.Parse(targetType, value);
                    result = true;
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

        public static TTarget ParseTo<TTarget>(this string value) {
            var targetType = typeof(TTarget);
            return (TTarget)ParseTo(value, targetType);
        }

        public static bool TryParseTo<TTarget>(this string value, out TTarget target) {
            var targetType = typeof(TTarget);
            if(TryParseTo(value, targetType, out var convertedValue)) {
                target = (TTarget)convertedValue;
                return true;
            }
            target = default(TTarget);
            return false;
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
        #endregion
    }
}
