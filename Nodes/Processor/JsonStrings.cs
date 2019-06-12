// ==================================================
// Copyright 2018(C) , DotLogix
// File:  JsonStrings.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  03.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Runtime.CompilerServices;
using System.Text;
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Core.Nodes.Processor {
    /// <summary>
    /// A static class providing extension methods for json strings
    /// </summary>
    public static class JsonStrings {
        /// <summary>
        /// Unescapes a json formatted string
        /// </summary>
        public static string UnescapeJsonString(string value, bool removeQuotes = false) {
            return UnescapeJsonString(value.ToCharArray(), 0, value.Length, removeQuotes);
        }

        /// <summary>
        /// Unescapes a json formatted string
        /// </summary>
        public static string UnescapeJsonString(char[] json, int startIndex, int count, bool removeQuotes = false) {
            var sb = new StringBuilder();
            var safeCharactersStart = -1;
            var safeCharactersCount = 0;
            if(removeQuotes) {
                startIndex++;
                count--;
            }

            var endIndex = startIndex + count-1;
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
                            throw new JsonParsingException("Character is not valid at this point, maybe your json string is not escaped correctly", i, 0, 0, null);
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
        /// Escapes a json formatted string
        /// </summary>
        public static string EscapeJsonString(string value, bool addQuotes = false) {
            var sb = new StringBuilder();
            AppendJsonString(sb, value, addQuotes);
            return sb.ToString();
        }

        /// <summary>
        /// Escapes a json formatted string and append it to a string builder
        /// </summary>
        public static void AppendJsonString(StringBuilder builder, string value, bool addQuotes = false) {
            if(addQuotes)
                builder.Append("\"");
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
                        if (currentInt < 0x20 || (currentInt >= 0x7F && currentInt <= 0x9F)) {
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
        /// Creates a hex encoded version for a unicode character
        /// </summary>
        public static string ToCharAsUnicode(int chr)
        {
            var unicodeBuffer = new char[6];
            ToCharAsUnicode(chr, unicodeBuffer);
            return new string(unicodeBuffer);
        }

        /// <summary>
        /// Creates a hex encoded version for a unicode character
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
        /// Creates a hex encoded version for a unicode character
        /// </summary>
        public static void ToCharAsUnicode(int chr, ref char[] buffer) {
            if(buffer == null)
                buffer = new char[6];
            buffer[0] = '\\';
            buffer[1] = 'u';

            for(var i = 5; i > 1; i--) {
                buffer[i] = IntToHex(chr & 15);
                chr >>= 4;
            }
        }

        /// <summary>
        /// Parses a hex encoded version of a unicode character
        /// </summary>
        public static char FromCharAsUnicode(char[] buffer, int startIndex) {
            var chr = HexToInt(buffer[startIndex]);
            for(var i = startIndex + 1; i < (startIndex + 4); i++)
                chr = (chr << 4) + HexToInt(buffer[i]);
            return (char)chr;
        }

        /// <summary>
        /// Quick conversion of an int to a hex character
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private static char IntToHex(int number) {
            if(number <= 9)
                return (char)(number + 48); // + '0'
            return (char)(number + 87); // - 10 + 'a'
        }

        /// <summary>
        /// Quick conversion of an hex character to an int
        /// </summary>
        private static int HexToInt(int hex) {
            if(hex <= 57) // <= '9'
                return hex - 48; // - '0'
            return hex - 87; // - 10 + 'a'
        }

        /// <summary>
        /// Check if an integer is a hex value (0-15)
        /// </summary>
        public static bool IsHex(int hex) {
            return HexToInt(hex).LaysBetween(0, 15);
        }
    }
}
