// ==================================================
// Copyright 2018(C) , DotLogix
// File:  FileLogger.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  21.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.IO;
#endregion

namespace DotLogix.Core.Diagnostics {
    /// <summary>
    /// A file logger implementation
    /// </summary>
    public class FileLogger : LoggerBase {
        private StreamWriter _logFileWriter;
        /// <summary>
        /// The log file directory
        /// </summary>
        public string Directory => Path.GetDirectoryName(LogFile);
        
        /// <summary>
        /// The log file name (Default: yyyy-MM-dd HH-mm-ss)
        /// </summary>
        public string LogFile { get; set; }
        
        /// <summary>
        /// The log file formatter
        /// </summary>
        public ILogMessageFormatter Formatter { get; set; } = new LogMessageFormatter();

        /// <summary>
        /// Creates a new instance of <see cref="FileLogger"/>
        /// </summary>
        public FileLogger() : base("FileLogger") {
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
            _logFileWriter.Flush();
            _logFileWriter.Close();
            _logFileWriter.Dispose();
            _logFileWriter = null;
            return true;
        }

        /// <inheritdoc />
        public override bool Log(LogMessage message) {
            try {
                return Formatter.Write(_logFileWriter, message);
            } catch {
                // omit any errors while logging
                return false;
            }
        }
    }
}
