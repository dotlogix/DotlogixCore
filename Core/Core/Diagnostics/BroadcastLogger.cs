// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  BroadcastLogger.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 22.08.2020 13:51
// LastEdited:  26.09.2021 22:27
// ==================================================

#region
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Core.Diagnostics; 

/// <summary>
///     A background-threaded hub for log messages
/// </summary>
public sealed class BroadcastLogger : IBroadcastLogger {
    private const int BufferSize = 1000;
    private readonly List<IAsyncLogTarget> _asyncLoggers;
    private readonly List<Func<LogMessage, ValueTask>> _asyncReceivers = new();
    private readonly List<ILogTarget> _loggers;
    private readonly ManualResetEventSlim _loggingThreadExit = new();
    private readonly BlockingCollection<LogMessage> _logMessages = new(BufferSize + 1);

    private readonly List<Action<LogMessage>> _receivers = new();
    private readonly object _syncRoot = new();
    private bool _disposed;
    private bool _initialized;
    private Thread _loggingThread;
    private int _skipped;

    /// <inheritdoc />
    public string Name => nameof(IBroadcastLogger);

    /// <summary>
    ///     Creates a new instance of <see cref="BroadcastLogger" />
    /// </summary>
    public BroadcastLogger() {
        _loggers = new List<ILogTarget>();
        _asyncLoggers = new List<IAsyncLogTarget>();
    }

    /// <summary>
    ///     Creates a new instance of <see cref="BroadcastLogger" />
    /// </summary>
    public BroadcastLogger(IEnumerable<ILogTarget> loggers = null, IEnumerable<IAsyncLogTarget> asyncLoggers = null) {
        _loggers = loggers.AsList() ?? new List<ILogTarget>();
        _asyncLoggers = asyncLoggers.AsList() ?? new List<IAsyncLogTarget>();
    }

    /// <inheritdoc />
    public void Log(LogMessage logMessage) {
        ThrowIfDisposed();
        EnsureInitialized();

        if(_skipped > 0) {
            if(_logMessages.Count == 0) {
                _logMessages.TryAdd(CreateLogMessage(LogLevels.Critical, $"Log message buffer overflow, skipped {_skipped} log messages to catch up again."));
                _skipped = 0;
                Interlocked.Exchange(ref _skipped, 0);
            } else {
                Interlocked.Increment(ref _skipped);
                return;
            }
        }

        if(_logMessages.Count < BufferSize) {
            _logMessages.TryAdd(logMessage);
        } else {
            while(_logMessages.Count > 0) {
                _logMessages.TryTake(out _);
            }
        }
    }

    /// <inheritdoc />
    public void Flush() {
        ThrowIfDisposed();
        if(_loggers.Count > 0) {
            _loggers.ForEach(r => r.Flush());
        }

        if(_asyncLoggers.Count > 0) {
            var taskBuffer = _asyncLoggers
               .Select(target => target.FlushAsync())
               .Where(valueTask => valueTask.IsCompletedSuccessfully == false)
               .Select(valueTask => valueTask.AsTask())
               .ToList();

            if(taskBuffer.Count > 0) {
                Task.WhenAll(taskBuffer).GetAwaiter().GetResult();
            }
        }
    }

    /// <inheritdoc />
    public bool Subscribe(Action<LogMessage> callback) {
        ThrowIfDisposed();
        if(_receivers.Contains(callback)) {
            return false;
        }

        _receivers.Add(callback);
        return true;
    }

    /// <inheritdoc />
    public bool Subscribe(Func<LogMessage, ValueTask> callback) {
        ThrowIfDisposed();
        if(_asyncReceivers.Contains(callback)) {
            return false;
        }

        _asyncReceivers.Add(callback);
        return true;
    }

    /// <inheritdoc />
    public bool Subscribe(ILogTarget logTarget) {
        ThrowIfDisposed();
        if(_loggers.Contains(logTarget)) {
            return false;
        }

        _loggers.Add(logTarget);
        return true;
    }

    /// <inheritdoc />
    public bool Subscribe(IAsyncLogTarget logTarget) {
        ThrowIfDisposed();
        if(_asyncLoggers.Contains(logTarget)) {
            return false;
        }

        _asyncLoggers.Add(logTarget);
        return true;
    }

    /// <inheritdoc />
    public bool Unsubscribe(ILogTarget logTarget) {
        ThrowIfDisposed();
        return _loggers.Remove(logTarget);
    }

    /// <inheritdoc />
    public bool Unsubscribe(IAsyncLogTarget logTarget) {
        ThrowIfDisposed();
        return _asyncLoggers.Remove(logTarget);
    }

    /// <inheritdoc />
    public bool Unsubscribe(Action<LogMessage> callback) {
        ThrowIfDisposed();
        return _receivers.Remove(callback);
    }

    /// <inheritdoc />
    public bool Unsubscribe(Func<LogMessage, ValueTask> callback) {
        ThrowIfDisposed();
        return _asyncReceivers.Remove(callback);
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync() {
        if(_disposed) {
            return;
        }
        _disposed = true;

        _logMessages.CompleteAdding();
        await _loggingThreadExit.WaitHandle.AsTask().ConfigureAwait(false);
        _loggingThreadExit.Dispose();
        _loggingThread?.Join();
        _loggingThread = null;

        if(_loggers.Count > 0) {
            _loggers.ForEach(target => target.Dispose());
        }

        if(_asyncLoggers.Count > 0) {
            var taskBuffer = _asyncLoggers
               .Select(target => target.DisposeAsync())
               .Where(valueTask => valueTask.IsCompletedSuccessfully == false)
               .Select(valueTask => valueTask.AsTask())
               .ToList();

            if(taskBuffer.Count > 0) {
                Task.WhenAll(taskBuffer).GetAwaiter().GetResult();
            }
        }
    }

    /// <inheritdoc />
    public void Dispose() {
        DisposeAsync().ConfigureAwait(false).GetAwaiter().GetResult();
    }

    private void LoggerMain() {
        var taskBuffer = new List<Task>();
        foreach(var logMessage in _logMessages.GetConsumingEnumerable()) {
            if(_receivers.Count > 0) {
                _receivers.ForEach(r => r.Invoke(logMessage));
            }

            if(_loggers.Count > 0) {
                _loggers.ForEach(r => r.Log(logMessage));
            }

            if(_asyncReceivers.Count > 0) {
                taskBuffer.AddRange(_asyncReceivers
                   .Select(receiver => receiver.Invoke(logMessage))
                   .Where(valueTask => valueTask.IsCompletedSuccessfully == false)
                   .Select(valueTask => valueTask.AsTask())
                );
            }

            if(_asyncLoggers.Count > 0) {
                taskBuffer.AddRange(_asyncLoggers
                   .Select(target => target.LogAsync(logMessage))
                   .Where(valueTask => valueTask.IsCompletedSuccessfully == false)
                   .Select(valueTask => valueTask.AsTask())
                );
            }

            if(taskBuffer.Count > 0) {
                Task.WhenAll(taskBuffer).GetAwaiter().GetResult();
                taskBuffer.Clear();
            }
        }
        _loggingThreadExit.Set();
    }

    private static LogMessage CreateLogMessage(LogLevels logLevel, string message) {
        var utcNow = DateTime.UtcNow;
        var thread = Thread.CurrentThread;
        var threadName = thread.Name ?? (thread.IsThreadPoolThread ? $"PoolThread {thread.ManagedThreadId}" : $"Thread {thread.ManagedThreadId}");

        var logMessage = new LogMessage {
            LogLevel = logLevel,
            UtcTimeStamp = utcNow,
            MethodName = nameof(Log),
            TypeName = nameof(BroadcastLogger),
            ThreadName = threadName,
            Message = message
        };

        return logMessage;
    }

    private void EnsureInitialized() {
        if(_initialized) {
            return;
        }

        lock(_syncRoot) {
            if(_initialized) {
                return;
            }

            _initialized = true;

            _loggingThread ??= new Thread(LoggerMain) {
                Name = "LoggerThread",
                Priority = ThreadPriority.BelowNormal,
                IsBackground = true
            };

            if(_loggingThread.IsAlive == false) {
                _loggingThread.Start();
            }

            _loggingThreadExit.Reset();
        }
    }

    private void ThrowIfDisposed() {
        if(_disposed)
            throw new ObjectDisposedException(nameof(BroadcastLogger));
    }
}
