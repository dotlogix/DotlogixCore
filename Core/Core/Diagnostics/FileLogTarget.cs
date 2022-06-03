// ==================================================
// Copyright 2018(C) , DotLogix
// File:  FileLogger.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  21.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.IO;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace DotLogix.Core.Diagnostics {
    /// <summary>
    /// A file logger implementation
    /// </summary>
    public sealed class FileLogTarget : AsyncLogTargetBase {
        private StreamWriter _logFileWriter;
        /// <summary>
        /// The log file directory
        /// </summary>
        public string Directory => Path.GetDirectoryName(LogFile);
        
        /// <summary>
        /// The log file name (Default: yyyy-MM-dd HH-mm-ss)
        /// </summary>
        public string LogFile { get; }
        
        /// <summary>
        /// The log file formatter
        /// </summary>
        public ILogMessageFormatter Formatter { get; set; } = new LogMessageFormatter();

        /// <summary>
        /// Creates a new instance of <see cref="FileLogTarget"/>
        /// </summary>
        public FileLogTarget(string logFile) {
            LogFile = logFile;
        }

        /// <inheritdoc />
        protected override ValueTask InitializeAsync() {
            if(System.IO.Directory.Exists(Directory) == false) {
                System.IO.Directory.CreateDirectory(Directory);
            }

            _logFileWriter = new StreamWriter(LogFile, true, Encoding.UTF8, 2048);
            return ValueTask.CompletedTask;
        }

        /// <inheritdoc />
        protected override async ValueTask FlushAsync() {
            await _logFileWriter.FlushAsync();
        }

        /// <inheritdoc />
        protected override async ValueTask LogAsync(LogMessage message) {
            try {
                var formattedMessage = Formatter.Format(message);
                if(string.IsNullOrEmpty(formattedMessage))
                    return;
                await _logFileWriter.WriteLineAsync(formattedMessage);
            } catch {
                // omit any errors while logging
            }
        }

        /// <inheritdoc />
        protected override async ValueTask DisposeAsync(bool disposing) {
            if(_logFileWriter == null)
                return;
            
            await _logFileWriter.FlushAsync();
            _logFileWriter.Close();
            await _logFileWriter.DisposeAsync();
            _logFileWriter = null;
        }
    }
}
