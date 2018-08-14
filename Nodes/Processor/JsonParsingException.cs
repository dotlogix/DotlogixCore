// ==================================================
// Copyright 2018(C) , DotLogix
// File:  JsonParsingException.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  04.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Core.Nodes.Processor {
    public class JsonParsingException : Exception {
        public int Position { get; }
        public int Line { get; }
        public int LinePosition { get; }
        public string Near { get; }

        public JsonParsingException(int position, char[] json, string message) : base(message) {
            Position = position;
            Line = json.OccurancesOf('\n', 0, position, out var lastNewLine);
            LinePosition = lastNewLine == -1 ? position : position - lastNewLine;

            var nearStart = Math.Max(position - 10, 0);
            var nearEnd = Math.Min(nearStart + 20, json.Length);
            Near = new string(json, nearStart, nearEnd - nearStart);
        }
    }
}
