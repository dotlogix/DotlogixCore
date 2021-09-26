// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  ConsoleLogger.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 22.08.2020 13:51
// LastEdited:  26.09.2021 22:27
// ==================================================

#region
using System;
using System.Runtime.InteropServices;
#endregion

namespace DotLogix.Core.Diagnostics {
    /// <summary>
    /// A console logger implementation
    /// </summary>
    public class ConsoleLogger : LoggerBase {
        public int BufferHeight { get; set; } = Console.BufferHeight;
        public int ConsoleHeight { get; set; } = Console.WindowHeight;
        public int ConsoleWidth { get; set; } = Console.WindowWidth;
        public ILogMessageFormatter Formatter { get; set; } = new LogMessageFormatter();

        /// <summary>
        /// Creates a new instance of <see cref="ConsoleLogger"/>
        /// </summary>
        public ConsoleLogger() : base("ConsoleLogger") {
        }

        /// <inheritdoc />
        public override bool Initialize() {
#if NETSTANDARD
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                Console.SetWindowSize(ConsoleWidth, ConsoleHeight);
                Console.SetBufferSize(ConsoleWidth, BufferHeight);
            }
#else
            Console.SetWindowSize(ConsoleWidth, ConsoleHeight);
            Console.SetBufferSize(ConsoleWidth, BufferHeight);
#endif
            return true;
        }

        /// <inheritdoc />
        public override bool Log(LogMessage message) {
            var currentColor = Console.ForegroundColor;
            var errorColor = GetErrorColor(message);

            Console.ForegroundColor = errorColor;
            Formatter.Write(Console.Out, message);
            Console.ForegroundColor = currentColor;
            return true;
        }

        protected virtual ConsoleColor GetErrorColor(LogMessage message) {
            switch(message.LogLevel) {
                case LogLevels.Trace:
                    return ConsoleColor.Gray;
                case LogLevels.Debug:
                    return ConsoleColor.Cyan;
                case LogLevels.Info:
                    return ConsoleColor.White;
                case LogLevels.Warning:
                    return ConsoleColor.Yellow;
                case LogLevels.Error:
                    return ConsoleColor.Red;
                case LogLevels.Critical:
                    return ConsoleColor.Magenta;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
