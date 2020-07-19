// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ParrallelLogger.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  21.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
using System.Linq;
using System.Threading;
#endregion

namespace DotLogix.Core.Diagnostics {
    /// <summary>
    /// A background-threaded hub for log messages
    /// </summary>
    public class ParallelLogger : ILogger {
        private readonly Thread _loggingThread;
        private readonly ManualResetEvent _logWait;
        private readonly Queue<LogMessage> _queuedMessages = new Queue<LogMessage>();
        private readonly HashSet<ILogger> _receivers = new HashSet<ILogger>();
        private readonly ManualResetEvent _receiverWait;
        private ILogger[] _currentReceivers = new ILogger[0];
        private bool _loggersChanged;

        /// <summary>
        /// The singleton instance
        /// </summary>
        public static ParallelLogger Instance { get; } = new ParallelLogger();
        
        /// <summary>
        /// The sync root
        /// </summary>
        public object SyncRoot { get; }

        /// <summary>
        /// A flag if the logger is initialized
        /// </summary>
        public bool Initialized { get; private set; }
        /// <summary>
        /// The current log level
        /// </summary>
        public LogLevels CurrentLogLevel { get; set; } = LogLevels.Off;

        private ParallelLogger() {
            SyncRoot = new object();
            _loggingThread = new Thread(LoggerMain) {
                                                        Name = "LoggerThread",
                                                        Priority = ThreadPriority.BelowNormal,
                                                        IsBackground = true
                                                    };
            _logWait = new ManualResetEvent(false);
            _receiverWait = new ManualResetEvent(false);
        }

        /// <inheritdoc />
        public string Name => "ParallelLogger";

        /// <inheritdoc />
        public bool Log(LogMessage logMessage) {
            if(IsLoggingDisabled(logMessage.LogLevel))
                return false;

            lock(SyncRoot) {
                if(Initialized == false)
                    return false;
                _queuedMessages.Enqueue(logMessage);
                _logWait.Set();
            }
            return true;
        }

        /// <inheritdoc />
        public bool Initialize() {
            lock(SyncRoot) {
                if(Initialized)
                    return false;
                Initialized = true;

                if(_loggersChanged) {
                    _currentReceivers = _receivers.ToArray();
                    if(_currentReceivers.All(l => l.Initialize()) == false)
                        return false;
                }
            }
            _loggingThread.Start();
            return true;
        }

        /// <inheritdoc />
        public bool Shutdown() {
            lock(SyncRoot) {
                if(Initialized == false)
                    return false;
                Initialized = false;
            }
            _receiverWait.Set();
            _logWait.Set();
            _loggingThread.Join();

            while(_queuedMessages.Count > 0) {
                var queuedMessage = _queuedMessages.Dequeue();
                foreach(var currentReceiver in _receivers)
                    currentReceiver.Log(queuedMessage);
            }

            if (_currentReceivers.All(l => l.Shutdown()) == false)
                return false;
            return true;
        }

        private void LoggerMain() {
            while(Initialized) {
                _receiverWait.WaitOne();
                _logWait.WaitOne();
                LogMessage currentMessage;
                lock(SyncRoot) {
                    if(_loggersChanged) {
                        _currentReceivers = _receivers.ToArray();
                        _loggersChanged = false;
                    }

                    if(_currentReceivers.Length == 0) {
                        _receiverWait.Reset();
                        continue;
                    }

                    if(_queuedMessages.Count == 0) {
                        _logWait.Reset();
                        continue;
                    }

                    currentMessage = _queuedMessages.Dequeue();
                    if(IsLoggingDisabled(currentMessage.LogLevel))
                        continue;
                }

                foreach(var currentReceiver in _currentReceivers)
                    currentReceiver.Log(currentMessage);
            }
        }

        /// <summary>
        /// Attach loggers to the hub
        /// </summary>
        /// <param name="loggers"></param>
        /// <returns></returns>
        public bool AttachLogger(IEnumerable<ILogger> loggers) {
            if(loggers == null)
                return true;

            var loggerList = loggers.ToList();
            if(loggerList.Count == 0)
                return true;

            lock(SyncRoot) {
                var count = _receivers.Count;
                _receivers.UnionWith(loggerList);
                if(count == _receivers.Count)
                    return false;
                _loggersChanged = true;
                _receiverWait.Set();
            }
            return loggerList.All(l => l.Initialize());
        }

        /// <summary>
        /// Detach loggers to the hub
        /// </summary>
        /// <param name="loggers"></param>
        /// <returns></returns>
        public bool DetachLogger(IEnumerable<ILogger> loggers) {
            if(loggers == null)
                return true;

            var loggerList = loggers.ToList();
            if(loggerList.Count == 0)
                return true;

            lock(SyncRoot) {
                var count = _receivers.Count;
                _receivers.ExceptWith(loggerList);
                if(count == _receivers.Count)
                    return false;
                _loggersChanged = true;
            }
            return loggerList.All(l => l.Shutdown());
        }

        /// <summary>
        /// Determines if a log level is high enough
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        public bool IsLoggingEnabled(LogLevels logLevel) {
            return CurrentLogLevel <= logLevel;
        }

        /// <summary>
        /// Determines if a log level is to low
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        public bool IsLoggingDisabled(LogLevels logLevel) {
            return CurrentLogLevel > logLevel;
        }
    }
}
