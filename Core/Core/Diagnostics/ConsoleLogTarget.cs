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
    public class ConsoleLogTarget : LogTargetBase {
        public int BufferHeight { get; set; } = Console.BufferHeight;
        public int ConsoleHeight { get; set; } = Console.WindowHeight;
        public int ConsoleWidth { get; set; } = Console.WindowWidth;
        public ILogMessageFormatter Formatter { get; set; } = new LogMessageFormatter();

        /// <summary>
        /// Creates a new instance of <see cref="ConsoleLogTarget"/>
        /// </summary>
        public ConsoleLogTarget() {
        }

        /// <inheritdoc />
        protected override void Initialize() {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                Console.SetWindowSize(ConsoleWidth, ConsoleHeight);
                Console.SetBufferSize(ConsoleWidth, BufferHeight);
            }
        }

        /// <inheritdoc />
        protected override void Log(LogMessage message) {
            var currentColor = Console.ForegroundColor;
            var errorColor = GetErrorColor(message);

            Console.ForegroundColor = errorColor;
            Console.Write(Formatter.Format(message));
            Console.ForegroundColor = currentColor;
        }

        protected virtual ConsoleColor GetErrorColor(LogMessage message) {
            return message.LogLevel switch {
                LogLevels.Trace => ConsoleColor.Gray,
                LogLevels.Debug => ConsoleColor.Cyan,
                LogLevels.Info => ConsoleColor.White,
                LogLevels.Warning => ConsoleColor.Yellow,
                LogLevels.Error => ConsoleColor.Red,
                LogLevels.Critical => ConsoleColor.Magenta,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}