// ==================================================
// Copyright 2018(C) , DotLogix
// File:  TextIoExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  01.08.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.IO;
using System.Text;
#endregion

namespace DotLogix.Core.Extensions {
    public static class TextIoExtensions {
        public static void Write(this TextWriter writer, char value, int count) {
            var buffer = new char[count].Initialize(value);
            writer.Write(buffer);
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
