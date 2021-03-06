// ==================================================
// Copyright 2018(C) , DotLogix
// File:  XmlLogger.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.IO;
using System.Threading;
using System.Xml.Linq;
#endregion

namespace DotLogix.Core.Diagnostics {
    /// <summary>
    /// A xml logger implementation
    /// </summary>
    public class XmlLogger : LoggerBase, IDisposable {
        private const int Delay = 30000;
        private readonly DateTime _dateTime = DateTime.Now;
        private readonly Timer _saveTimer;
        private readonly XDocument _xDocument;
        private readonly XElement _xRoot;
        private bool _docChanged;

        /// <summary>
        /// The log directory
        /// </summary>
        public string Directory { get; }
        /// <summary>
        /// The log file name
        /// </summary>
        public string LogFile { get; }
        /// <summary>
        /// The sync root
        /// </summary>
        public object SyncRoot { get; }

        /// <inheritdoc />
        public XmlLogger(string directory, string prefix = null) : base("XmlLogger") {
            SyncRoot = new object();
            Directory = directory;
            var logFileName = $"{_dateTime:dd-MM-yyyy HH-mm-ss}";
            if(string.IsNullOrWhiteSpace(prefix) == false)
                logFileName = prefix + logFileName;
            LogFile = Path.Combine(directory, $"{logFileName}.xlog");
            _xRoot = new XElement("logs");
            _xDocument = new XDocument(_xRoot);
            _saveTimer = new Timer(obj => Save(), null, Delay, Delay);
        }

        /// <inheritdoc />
        public void Dispose() {
            Shutdown();
        }

        /// <inheritdoc />
        public override bool Log(LogMessage message) {
            var xLog = new XElement("log");
            xLog.SetElementValue("utc", message.UtcTimeStamp.ToString("G"));
            xLog.SetElementValue("loglevel", message.LogLevel);
            xLog.SetElementValue("method", message.MethodName);
            xLog.SetElementValue("thread", message.ThreadName);
            xLog.SetElementValue("context", message.ClassName);
            xLog.SetElementValue("message", message.Message);
            lock(SyncRoot) {
                _docChanged = true;
                _xRoot.Add(xLog);
            }
            return true;
        }

        /// <inheritdoc />
        public override bool Shutdown() {
            lock(SyncRoot) {
                if(_docChanged)
                    Save();
            }
            _saveTimer.Dispose();
            return true;
        }

        private void Save() {
            lock(SyncRoot) {
                if(_docChanged)
                    _xDocument.Save(LogFile);
            }
        }
    }
}
