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
    public class FileLogger : LoggerBase {
        private readonly DateTime _dateTime = DateTime.Now;
        private readonly TextLogMessageFormatter _formatter = new TextLogMessageFormatter();


        private bool _isErrorLog;
        private string _logFileName;
        private StreamWriter _logFileWriter;

        public string Directory { get; }
        public string LogFile { get; private set; }

        public FileLogger(string directory, string prefix = null) : base("FileLogger") {
            Directory = directory;
            _logFileName = $"{_dateTime:dd-MM-yyyy HH-mm-ss}";

            if(string.IsNullOrWhiteSpace(prefix) == false)
                _logFileName = prefix + _logFileName;

            LogFile = Path.Combine(directory, $"{_logFileName}.log");
        }

        public override bool Initialize() {
            if(_logFileWriter != null)
                return true;
            if(System.IO.Directory.Exists(Directory) == false)
                System.IO.Directory.CreateDirectory(Directory);

            _logFileWriter = new StreamWriter(LogFile);
            return true;
        }

        public override bool Shutdown() {
            if(_logFileWriter == null)
                return true;
            _logFileWriter.Close();
            _logFileWriter.Dispose();
            _logFileWriter = null;
            return true;
        }

        public override bool Log(LogMessage message) {
            if((_isErrorLog == false) && (message.LogLevel >= LogLevels.Error))
                ToErrorLogFile();
            var msg = _formatter.Format(message);
            _logFileWriter.WriteLine(msg);
            return true;
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
