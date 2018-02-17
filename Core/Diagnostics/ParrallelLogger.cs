// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ParrallelLogger.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Collections.Generic;
using System.Linq;
using System.Threading;
#endregion

namespace DotLogix.Core.Diagnostics {
    public class ParrallelLogger : ILogger {
        private readonly Thread _loggingThread;
        private readonly ManualResetEvent _logWait;
        private readonly Queue<LogMessage> _queuedMessages = new Queue<LogMessage>();
        private readonly HashSet<ILogger> _receivers = new HashSet<ILogger>();
        private readonly ManualResetEvent _receiverWait;
        private ILogger[] _currentReceivers = new ILogger[0];
        private bool _loggersChanged;

        public static ParrallelLogger Instance { get; } = new ParrallelLogger();

        public object SyncRoot { get; }
        public bool Initialized { get; private set; }
        public LogLevels CurrentLogLevel { get; set; }

        private ParrallelLogger() {
            SyncRoot = new object();
            _loggingThread = new Thread(LoggerMain) {
                                                        Name = "LoggerThread",
                                                        Priority = ThreadPriority.BelowNormal,
                                                        IsBackground = true
                                                    };
            _logWait = new ManualResetEvent(false);
            _receiverWait = new ManualResetEvent(false);
        }

        public string Name => "ParrallelLogger";

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

        public bool Initialize() {
            lock(SyncRoot) {
                if(Initialized)
                    return false;
                Initialized = true;
                if(_currentReceivers.Any(logger => !logger.Initialize()))
                    return false;
            }
            _loggingThread.Start();
            return true;
        }

        public bool Shutdown() {
            lock(SyncRoot) {
                if(Initialized == false)
                    return false;
                Initialized = false;
                if(_currentReceivers.Any(logger => !logger.Shutdown()))
                    return false;
            }
            _receiverWait.Set();
            _logWait.Set();
            _loggingThread.Join();

            while(_queuedMessages.Count > 0) {
                var queuedMessage = _queuedMessages.Dequeue();
                foreach(var currentReceiver in _receivers)
                    currentReceiver.Log(queuedMessage);
            }
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

        public bool AttachLogger(params ILogger[] receiver) {
            if((receiver == null) || (receiver.Length == 0))
                return true;

            lock(SyncRoot) {
                var count = _receivers.Count;
                _receivers.UnionWith(receiver);
                if(count == _receivers.Count)
                    return false;
                _loggersChanged = true;
                _receiverWait.Set();
            }
            return true;
        }

        public bool DetachLogger(params ILogger[] receiver) {
            if((receiver == null) || (receiver.Length == 0))
                return true;

            lock(SyncRoot) {
                var count = _receivers.Count;
                _receivers.ExceptWith(receiver);
                if(count == _receivers.Count)
                    return false;
                _loggersChanged = true;
            }
            return true;
        }

        public bool IsLoggingEnabled(LogLevels logLevel) {
            return CurrentLogLevel <= logLevel;
        }

        public bool IsLoggingDisabled(LogLevels logLevel) {
            return CurrentLogLevel > logLevel;
        }

        ~ParrallelLogger() {
            Shutdown();
        }
    }
}
