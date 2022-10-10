// ==================================================
// Copyright 2018(C) , DotLogix
// File:  FileLogger.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  21.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.IO;
#endregion

namespace DotLogix.Core.Diagnostics {
    /// <summary>
    /// A file logger implementation
    /// </summary>
    public class FileLogger : LoggerBase {
        private readonly DateTime _dateTime = DateTime.Now;

        private bool _isErrorLog;
        private string _logFileName;
        private StreamWriter _logFileWriter;
        /// <summary>
        /// The log directory
        /// </summary>
        public string Directory { get; }
        /// <summary>
        /// The log file name %prefix%%dd-MM-yyyy HH-mm-ss%
        /// </summary>
        public string LogFile { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="FileLogger"/>
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="prefix">The prefix of the date in the log message %prefix%%dd-MM-yyyy HH-mm-ss%</param>
        public FileLogger(string directory, string prefix = null) : base("FileLogger") {
            Directory = directory;
            _logFileName = $"{_dateTime:dd-MM-yyyy HH-mm-ss}";

            if(string.IsNullOrWhiteSpace(prefix) == false)
                _logFileName = prefix + _logFileName;

            LogFile = Path.Combine(directory, $"{_logFileName}.log");
        }

        /// <inheritdoc />
        public override bool Initialize() {
            if(_logFileWriter != null)
                return true;
            if(System.IO.Directory.Exists(Directory) == false)
                System.IO.Directory.CreateDirectory(Directory);

            _logFileWriter = new StreamWriter(LogFile);
            return true;
        }

        /// <inheritdoc />
        public override bool Shutdown() {
            if(_logFileWriter == null)
                return true;
            _logFileWriter.Close();
            _logFileWriter.Dispose();
            _logFileWriter = null;
            return true;
        }

        /// <inheritdoc />
        public override bool Log(LogMessage message) {
            if((_isErrorLog == false) && (message.LogLevel >= LogLevels.Error))
                ToErrorLogFile();

            try {
                return LogMessageFormatter.Default.Format(message, _logFileWriter);
            } catch {
                // omit any errors while logging
                return false;
            }
        }

        private void ToErrorLogFile() {
            _isErrorLog = true;
            var current = LogFile;
            _logFileName += " - Error";
            LogFile = Path.Combine(Directory, $"{_logFileName}.log");
            _logFileWriter.Close();
            _logFileWriter.Dispose();
            File.Move(current, LogFile);
            _logFileWriter = new StreamWriter(LogFile, true);
        }
    }
}
