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
    /// <summary>
    /// A static class providing extension methods for <see cref="string"/>
    /// </summary>
    public static class TextIoExtensions {
        /// <summary>
        /// Reads the characters of a <see cref="TextReader"/> async
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="bufferSize"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IEnumerable<ValueTask<char>> AsAsyncSequence(this TextReader reader, int bufferSize, Action<char> action = null) {
            var buffer = new char[bufferSize];
            var count = 0;

            do
            {
                var firstCharAsync = reader.ReadAsync(buffer, 0, buffer.Length)
                    .ContinueWith(task =>
                    {
                        count = task.Result;

                        if (count == 0)
                            return '\0';

                        var chr = buffer[0];
                        action?.Invoke(chr);
                        return chr;
                    }, TaskContinuationOptions.ExecuteSynchronously);

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


        /// <summary>
        /// Reads the characters of a <see cref="TextReader"/>
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="bufferSize"></param>
        /// <param name="action"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Write a char to a <see cref="TextWriter"/> multiple times
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="count"></param>
        public static void Write(this TextWriter writer, char value, int count) {
            var buffer = new char[count].Initialize(value);
            writer.Write(buffer);
        }

        /// <summary>
        /// Write a char to a <see cref="TextWriter"/> multiple times async
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="count"></param>
        public static Task WriteAsync(this TextWriter writer, char value, int count) {
            var buffer = new char[count].Initialize(value);
            return writer.WriteAsync(buffer);
        }

        /// <summary>
        /// Skips the characters of a <see cref="TextReader"/> while the character is in a blacklist
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="chars"></param>
        public static void Skip(this TextReader reader, params char[] chars) {
            int current;
            while(((current = reader.Peek()) != -1) && (Array.IndexOf(chars, (char)current) != -1))
                reader.Read();
        }

        /// <summary>
        /// Skips the characters of a <see cref="TextReader"/> while the character equal to the one provided
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="chr"></param>
        public static void Skip(this TextReader reader, char chr) {
            int intChr = chr;
            while(reader.Peek() == intChr)
                reader.Read();
        }

        /// <summary>
        /// Skips whitespace characters of a <see cref="TextReader"/>
        /// </summary>
        /// <param name="reader"></param>
        public static void SkipWhitespace(this TextReader reader) {
            Skip(reader, ' ');
        }

        /// <summary>
        /// Read a string until one of the characters is hit
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static string ReadUntil(this TextReader reader, params char[] chars) {
            var sb = new StringBuilder();
            int current;
            while(((current = reader.Peek()) != -1) && (Array.IndexOf(chars, (char)current) == -1))
                sb.Append((char)reader.Read());
            return sb.ToString();
        }

        /// <summary>
        /// Read a string until the character is hit
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="chr"></param>
        /// <returns></returns>
        public static string ReadUntil(this TextReader reader, char chr) {
            var sb = new StringBuilder();
            int intChr = chr;
            while(reader.Peek() != intChr)
                sb.Append((char)reader.Read());
            return sb.ToString();
        }

        /// <summary>
        /// Read a string until a whitespace character
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static string ReadUntilWhitespace(this TextReader reader) {
            return ReadUntil(reader, ' ');
        }
    }
}
