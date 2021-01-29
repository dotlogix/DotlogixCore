// ==================================================
// Copyright 2018(C) , DotLogix
// File:  JsonStrings.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  03.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Text;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes.Formats.Json;
using DotLogix.Core.Utils;
#endregion

namespace DotLogix.Core.Nodes.Utils {
    /// <summary>
    ///     A static class providing extension methods for json strings
    /// </summary>
    public static class JsonStrings {
        private const int Char0 = '0';
        private const int Char9 = '9';
        private const int CharA = 'A';
        private const int CharAlower = 'a';
        private const int CharF = 'F';
        private const int CharFlower = 'f';


        private static readonly ArrayPool<char> _charArrayPool = new ArrayPool<char>(initialCount: 10);
        public static string JsonTrue { get; } = "true";
        public static char[] JsonTrueChars { get; } = JsonTrue.ToCharArray();
        public static string JsonFalse { get; } = "false";
        public static char[] JsonFalseChars { get; } = JsonFalse.ToCharArray();
        public static string JsonNull { get; } = "null";
        public static char[] JsonNullChars { get; } = JsonNull.ToCharArray();
        
        public static char[] RentBuffer(int minimumLength) {
            return _charArrayPool.RentOrCreate(minimumLength);
        }
        public static void ReturnBuffer(char[] buffer) {
            _charArrayPool.Return(buffer);
        }


        /// <summary>
        ///     Unescapes a json formatted string
        /// </summary>
        public static string UnescapeJsonString(string value, bool removeQuotes = false) {
            return UnescapeJsonString(value.ToCharArray(), 0, value.Length, removeQuotes);
        }

        /// <summary>
        ///     Unescapes a json formatted string
        /// </summary>
        public static string UnescapeJsonString(char[] json, int startIndex, int count, bool removeQuotes = false) {
            var sb = new StringBuilder();
            var safeCharactersStart = -1;
            var safeCharactersCount = 0;
            if(removeQuotes) {
                startIndex++;
                count--;
            }

            var endIndex = (startIndex + count) - 1;
            for(var i = startIndex; i <= endIndex; i++) {
                var current = json[i];
                if(current == '\\') {
                    var nextChr = json[i + 1];
                    switch(nextChr) {
                        case '"':
                        case '\\':
                        case '/':
                            current = nextChr;
                            break;
                        case 'b':
                            current = '\b';
                            break;
                        case 'f':
                            current = '\f';
                            break;
                        case 'n':
                            current = '\n';
                            break;
                        case 'r':
                            current = '\r';
                            break;
                        case 't':
                            current = '\t';
                            break;
                        case 'u':
                            current = FromCharAsUnicode(json, i + 2);
                            i += 4;
                            break;
                        default:
                            throw new JsonParsingException("Character is not valid at this point, maybe your json string is not escaped correctly", i, 0, 0, new string(json));
                    }

                    i++;
                } else {
                    if(safeCharactersStart == -1)
                        safeCharactersStart = i;
                    safeCharactersCount++;
                    continue;
                }

                if(safeCharactersCount > 0) {
                    sb.Append(json, safeCharactersStart, safeCharactersCount);
                    safeCharactersStart = -1;
                    safeCharactersCount = 0;
                }

                sb.Append(current);
            }

            var length = count - startIndex;
            if(safeCharactersCount == length)
                return new string(json, startIndex, length);

            return safeCharactersCount == 0
                       ? sb.ToString()
                       : sb.Append(json, safeCharactersStart, safeCharactersCount).ToString();
        }

        /// <summary>
        ///     Escapes a json formatted string
        /// </summary>
        public static string EscapeJsonString(string value, bool addQuotes = false) {
            var buffer = new CharBuffer(value.Length + 2);
            AppendJsonString(buffer, value, addQuotes);
            return buffer.ToString();
        }

        /// <summary>
        ///     Escapes a json formatted string and append it to a string builder
        /// </summary>
        public static void AppendJsonString(CharBuffer builder, string value, bool addQuotes = false) {
            builder.EnsureCapacity(value.Length + 2);

            if(addQuotes)
                builder.Append('\"');
            char[] unicodeBuffer = null;

            var safeCharactersCount = 0;
            for(var i = 0; i < value.Length; i++) {
                var current = value[i];
                switch(current) {
                    case '"':
                    case '\\':
                    case '/':
                        break;
                    case '\b':
                        current = 'b';
                        break;
                    case '\f':
                        current = 'f';
                        break;
                    case '\n':
                        current = 'n';
                        break;
                    case '\r':
                        current = 'r';
                        break;
                    case '\t':
                        current = 't';
                        break;
                    default:
                        int currentInt = current;
                        if((currentInt < 0x20) || ((currentInt >= 0x7F) && (currentInt <= 0x9F))) {
                            if(safeCharactersCount > 0) {
                                builder.Append(value, i - safeCharactersCount, safeCharactersCount);
                                safeCharactersCount = 0;
                            }

                            ToCharAsUnicode(current, ref unicodeBuffer);
                            builder.Append(unicodeBuffer);
                        } else
                            safeCharactersCount++;

                        continue;
                }

                if(safeCharactersCount > 0) {
                    builder.Append(value, i - safeCharactersCount, safeCharactersCount);
                    safeCharactersCount = 0;
                }

                builder.Append('\\');
                builder.Append(current);
            }


            if(safeCharactersCount > 0) {
                if(safeCharactersCount == value.Length)
                    builder.Append(value);
                else
                    builder.Append(value, value.Length - safeCharactersCount, safeCharactersCount);
            }

            if(addQuotes)
                builder.Append("\"");
        }

        /// <summary>
        ///     Creates a hex encoded version for a unicode character
        /// </summary>
        public static string ToCharAsUnicode(int chr) {
            var unicodeBuffer = new char[6];
            ToCharAsUnicode(chr, unicodeBuffer);
            return new string(unicodeBuffer);
        }

        /// <summary>
        ///     Creates a hex encoded version for a unicode character
        /// </summary>
        public static void ToCharAsUnicode(int chr, char[] buffer) {
            buffer[0] = '\\';
            buffer[1] = 'u';

            for(var i = 5; i > 1; i--) {
                buffer[i] = IntToHex(chr & 15);
                chr >>= 4;
            }
        }

        /// <summary>
        ///     Creates a hex encoded version for a unicode character
        /// </summary>
        public static void ToCharAsUnicode(int chr, ref char[] buffer) {
            buffer ??= new char[6];
            buffer[0] = '\\';
            buffer[1] = 'u';

            for(var i = 5; i > 1; i--) {
                buffer[i] = IntToHex(chr & 15);
                chr >>= 4;
            }
        }

        /// <summary>
        ///     Parses a hex encoded version of a unicode character
        /// </summary>
        public static char FromCharAsUnicode(char[] buffer, int startIndex) {
            var chr = HexToInt(buffer[startIndex]);
            for(var i = startIndex + 1; i < (startIndex + 4); i++)
                chr = (chr << 4) + HexToInt(buffer[i]);
            return (char)chr;
        }

        /// <summary>
        ///     Quick conversion of an int to a hex character
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static char IntToHex(int number) {
            if((number < 0) || (number > 15))
                throw new ArgumentOutOfRangeException(nameof(number), number, $"Number {number} is not in range of hex characters [0-15]");
            if(number <= 9)
                return (char)(number + Char0);
            return (char)((number - 10) + CharAlower);
        }

        /// <summary>
        ///     Quick conversion of an hex character to an int
        /// </summary>
        public static int HexToInt(int hex) {
            if((hex >= Char0) && (hex <= Char9))
                return hex - Char0;

            if((hex >= CharA) && (hex <= CharF))
                return (hex - CharA) + 10;

            if((hex >= CharAlower) && (hex <= CharFlower))
                return (hex - CharAlower) + 10;

            throw new ArgumentOutOfRangeException(nameof(hex), hex, $"Number {hex} is not in range of hex characters [0-9A-Fa-f]");
        }

        /// <summary>
        ///     Check if an integer is a hex value (0-15)
        /// </summary>
        public static bool IsHex(int hex) {
            if((hex >= Char0) && (hex <= Char9))
                return true;

            if((hex >= CharA) && (hex <= CharF))
                return true;

            if((hex >= CharAlower) && (hex <= CharFlower))
                return true;

            return false;
        }

        public static string FormatLong(long value) {
            var neg = value < 0;
            var offset = 0;
            if(neg) {
                offset++;
                value = -value;
            }

            var integerValue = (ulong)value;
            var digitCount = integerValue.DigitCount();
            var chars = new char[digitCount + offset];

            if(neg)
                chars[0] = '-';

            FormatInteger(integerValue, offset, digitCount, chars);

            return new string(chars);
        }

        private static void FormatInteger(ulong value, int startIndex, int count, char[] chars) {
            for(var i = (startIndex + count) - 1; i >= startIndex; i--) {
                chars[i] = (char)((value % 10) + Char0);
                value /= 10;
            }
        }

        public static string FormatULong(ulong value) {
            var digitCount = value.DigitCount();
            var chars = new char[digitCount];
            FormatInteger(value, 0, digitCount, chars);
            return new string(chars);
        }

        public static long ParseInt64(string str) {
            if(string.IsNullOrEmpty(str))
                throw new ArgumentException("Parameter can not be null or empty");

            long value = 0;
            var neg = false;
            for(var i = 0; i < str.Length; i++) {
                var chr = str[i];
                if((chr >= Char0) && (chr <= Char9))
                    value = (value * 10) + (chr - Char0);
                else if((chr == '-') && (i == 0))
                    neg = true;
                else
                    throw new FormatException($"Character {chr} is not valid at this point");
            }

            return neg ? -value : value;
        }

        public static ulong ParseUInt64(string str) {
            if(string.IsNullOrEmpty(str))
                throw new ArgumentException("Parameter can not be null or empty");

            ulong value = 0;
            for(var i = 0; i < str.Length; i++) {
                var chr = str[i];
                if((chr >= Char0) && (chr <= Char9))
                    value = (value * 10) + (ulong)(chr - Char0);
                else
                    throw new FormatException($"Character {chr} is not valid at this point");
            }

            return value;
        }

        public static double ParseDouble(string str) {
            if(string.IsNullOrEmpty(str))
                throw new ArgumentException("Parameter can not be null or empty");

            long part = 0;
            var chr = '\0';
            var neg = false;
            var index = 0;

            for(; index < str.Length; index++) {
                chr = str[index];
                if((chr >= Char0) && (chr <= Char9))
                    part = (part * 10) + (chr - Char0);
                else if((chr == '-') && (index == 0))
                    neg = true;
                else
                    break;
            }

            double value = part;

            if(chr == '.') {
                index++;
                part = 10;
                for(; index < str.Length; index++) {
                    chr = str[index];
                    if((chr >= Char0) && (chr <= Char9)) {
                        value += (double)(chr - Char0) / part;
                        part *= 10;
                    } else
                        break;
                }
            }

            if(neg)
                value = -value;

            if((chr == 'e') || (chr == 'E')) {
                neg = false;
                part = 0;
                index++;
                var startIndex = index;

                for(; index < str.Length; index++) {
                    chr = str[index];
                    if((chr >= Char0) && (chr <= Char9))
                        part = (part * 10) + (chr - Char0);
                    else if((chr == '-') && (index == startIndex))
                        neg = true;
                    else if((chr != '+') || (index > startIndex))
                        throw new FormatException($"Character {chr} is not valid at this point");
                }

                if(startIndex < index) {
                    if(neg)
                        value /= Math.Pow(10d, part);
                    else
                        value *= Math.Pow(10d, part);
                }
            }

            if(index != str.Length)
                throw new FormatException($"Character {chr} is not valid at this point");

            return value;
        }

        //private static unsafe long ParseLong(char* input, int len) {
        //    int pos = 0;           // read pointer position
        //    long part = 0;          // the current part (int, float and sci parts of the number)
        //    bool neg = false;      // true if part is a negative number

        //    long* ret = stackalloc long[1];

        //    while (pos < len && (*(input + pos) > Char9 || *(input + pos) < Char0) && *(input + pos) != '-')
        //        pos++;

        //    // sign
        //    if (*(input + pos) == '-') {
        //        neg = true;
        //        pos++;
        //    }

        //    // integer part
        //    while (pos < len && !(input[pos] > Char9 || input[pos] < Char0))
        //        part = part * 10 + (input[pos++] - Char0);

        //    *ret = neg ? (part * -1) : part;
        //    return *ret;
        //}

        //private static unsafe double ParseDouble(char* input, int len) {
        //    //float ret = 0f;      // return value
        //    int pos = 0;           // read pointer position
        //    int part = 0;          // the current part (int, float and sci parts of the number)
        //    bool neg = false;      // true if part is a negative number

        //    double* ret = stackalloc double[1];

        //    // find start
        //    while (pos < len && (input[pos] < Char0 || input[pos] > Char9) && input[pos] != '-' && input[pos] != '.')
        //        pos++;

        //    // sign
        //    if (input[pos] == '-') {
        //        neg = true;
        //        pos++;
        //    }

        //    // integer part
        //    while (pos < len && !(input[pos] > Char9 || input[pos] < Char0))
        //        part = part * 10 + (input[pos++] - Char0);

        //    *ret = neg ? part * -1 : part;

        //    // float part
        //    if (pos < len && input[pos] == '.') {
        //        pos++;
        //        double mul = 1;
        //        part = 0;

        //        while (pos < len && !(input[pos] > Char9 || input[pos] < Char0)) {
        //            part = part * 10 + (input[pos] - Char0);
        //            mul *= 10;
        //            pos++;
        //        }

        //        if (neg)
        //            *ret -= part / mul;
        //        else
        //            *ret += part / mul;

        //    }

        //    // scientific part
        //    if (pos < len && (input[pos] == 'e' || input[pos] == 'E')) {
        //        pos++;
        //        neg = (input[pos] == '-');
        //        pos++;
        //        part = 0;
        //        while (pos < len && !(input[pos] > Char9 || input[pos] < Char0)) {
        //            part = part * 10 + (input[pos++] - Char0);
        //        }

        //        if (neg)
        //            *ret /= Math.Pow(10d, part);
        //        else
        //            *ret *= Math.Pow(10d, part);
        //    }

        //    return *ret;
        //}
    }
}
