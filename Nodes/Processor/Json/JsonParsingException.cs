// ==================================================
// Copyright 2018(C) , DotLogix
// File:  JsonParsingException.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  04.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Nodes.Processor {
    /// <summary>
    /// An exception thrown by a json parser
    /// </summary>
    public class JsonParsingException : Exception {
        /// <summary>
        /// The current character position
        /// </summary>
        public int Position { get; }
        /// <summary>
        /// The line number
        /// </summary>
        public int Line { get; }
        /// <summary>
        /// The position in line
        /// </summary>
        public int LinePosition { get; }
        /// <summary>
        /// The context characters in front of the error
        /// </summary>
        public string Near { get; }

        /// <summary>
        /// Creates a new instance of <see cref="JsonParsingException"/>
        /// </summary>
        public JsonParsingException(string message, int position, int line, int linePosition, string near) : base(message) {
            Position = position;
            Line = line;
            LinePosition = linePosition;
            Near = near;
        }
    }
}
