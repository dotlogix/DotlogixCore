// ==================================================
// Copyright 2018(C) , DotLogix
// File:  TextIoExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  01.08.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace DotLogix.Core.Extensions {
    public static class TextIoExtensions {
        public static IEnumerable<ValueTask<char>> AsAsyncSequence(this TextReader reader, int bufferSize, Action<char> action = null) {
            var buffer = new char[bufferSize];
            var count = 0;
            async Task<char> ReadFirstCharAsync() {
                count = await reader.ReadAsync(buffer, 0, buffer.Length);


                if(count == 0)
                    return '\0';

                var chr = buffer[0];
                action?.Invoke(chr);
                return chr;

            }

            do {
                var firstCharAsync = ReadFirstCharAsync();
                if(firstCharAsync.IsCompleted) {
                    if(count == 0)
                        yield break;
                    yield return new ValueTask<char>(buffer[0]);
                } else {
                    yield return new ValueTask<char>(firstCharAsync);
                }

                if(count == 0)
                    yield break;

                for(var i = 1; i < count; i++) {
                    var chr = buffer[i];
                    action?.Invoke(chr);
                    yield return new ValueTask<char>(chr);
                }
            } while(true);
        }

        public static IEnumerable<char> AsSequence(this TextReader reader, int bufferSize, Action<char> action = null) {
            var buffer = new char[bufferSize];
            var count = 0;
            while((count = reader.Read(buffer, 0, buffer.Length)) > 0) {
                for(int i = 0; i < count; i++) {
                    var chr = buffer[i];
                    action?.Invoke(chr);
                    yield return chr;
                }
            }
        }

        public static void Write(this TextWriter writer, char value, int count) {
            var buffer = new char[count].Initialize(value);
            writer.Write(buffer);
        }

        public static Task WriteAsync(this TextWriter writer, char value, int count) {
            var buffer = new char[count].Initialize(value);
            return writer.WriteAsync(buffer);
        }


        public static void Skip(this TextReader reader, params char[] chars) {
            int current;
            while(((current = reader.Peek()) != -1) && (Array.IndexOf(chars, (char)current) != -1))
                reader.Read();
        }

        public static void Skip(this TextReader reader, char chr) {
            int intChr = chr;
            while(reader.Peek() == intChr)
                reader.Read();
        }

        public static void SkipWhitespace(this TextReader reader) {
            Skip(reader, ' ');
        }

        public static string ReadUntil(this TextReader reader, params char[] chars) {
            var sb = new StringBuilder();
            int current;
            while(((current = reader.Peek()) != -1) && (Array.IndexOf(chars, (char)current) == -1))
                sb.Append((char)reader.Read());
            return sb.ToString();
        }

        public static string ReadUntil(this TextReader reader, char chr) {
            var sb = new StringBuilder();
            int intChr = chr;
            while(reader.Peek() != intChr)
                sb.Append((char)reader.Read());
            return sb.ToString();
        }

        public static string ReadUntilWhitespace(this TextReader reader) {
            return ReadUntil(reader, ' ');
        }
    }
}
