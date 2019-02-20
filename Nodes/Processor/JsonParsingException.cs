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

        public JsonParsingException(string message, int position, int line, int linePosition, string near) : base(message) {
            Position = position;
            Line = line;
            LinePosition = linePosition;
            Near = near;
        }
    }
}
