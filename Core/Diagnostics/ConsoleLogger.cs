// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ConsoleLogger.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  21.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Diagnostics {
    /// <summary>
    /// A console logger implementation
    /// </summary>
    public class ConsoleLogger : LoggerBase {
        private readonly int _bufferHeight;
        private readonly int _consoleHeight;
        private readonly int _consoleWidth;

        private readonly ConsoleLogMessageFormatter _formatter;

        /// <summary>
        /// Creates a new instance of <see cref="ConsoleLogger"/>
        /// </summary>
        /// <param name="consoleWidth"></param>
        /// <param name="consoleHeight"></param>
        /// <param name="bufferHeight"></param>
        public ConsoleLogger(int consoleWidth, int consoleHeight, int bufferHeight = 2000) : base("ConsoleLogger") {
            _bufferHeight = bufferHeight;
            _consoleWidth = Math.Min(consoleWidth, Console.LargestWindowWidth);
            _consoleHeight = Math.Min(consoleHeight, Console.LargestWindowHeight);
            _formatter = new ConsoleLogMessageFormatter(_consoleWidth - 1);
        }

        /// <inheritdoc />
        public override bool Initialize() {
            if((Console.WindowWidth == _consoleWidth)
               && (Console.WindowHeight == _consoleHeight)
               && (Console.BufferHeight == _bufferHeight)
               && (Console.BufferWidth == _consoleWidth))
                return true;

            Console.SetWindowSize(_consoleWidth, _consoleHeight);
            Console.SetBufferSize(_consoleWidth, _bufferHeight);
            return true;
        }

        /// <inheritdoc />
        public override bool Log(LogMessage message) {
            var currentColor = Console.ForegroundColor;
            switch(message.LogLevel) {
                case LogLevels.Trace:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogLevels.Debug:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case LogLevels.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogLevels.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevels.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogLevels.Critical:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            var msg = _formatter.Format(message);
            Console.WriteLine(msg);
            Console.ForegroundColor = currentColor;
            return true;
        }
    }
}
