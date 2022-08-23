using System;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Core.Extensions;

namespace DotLogix.Core.Diagnostics; 

/// <summary>
/// An base class to receive log messages asynchronously
/// </summary>
public abstract class AsyncLogTargetBase : IAsyncLogTarget {
    private readonly SemaphoreSlim _syncRoot = new(1);
    private bool _initialized;
    private bool _disposed;
        
    /// <inheritdoc />
    public virtual string Name => GetType().GetFriendlyName();
        
    async ValueTask IAsyncLogTarget.LogAsync(LogMessage message) {
        ThrowIfDisposed();
        await EnsureInitializedAsync();
        await LogAsync(message);
    }
        
    async ValueTask IAsyncLogTarget.FlushAsync() {
        ThrowIfDisposed();
        await EnsureInitializedAsync();
        await FlushAsync();
    }
        
    /// <inheritdoc />
    public async ValueTask DisposeAsync() {
        if(_disposed)
            return;
                
        _disposed = true;
        await DisposeAsync(true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc cref="IAsyncLogTarget.LogAsync" />
    protected abstract ValueTask LogAsync(LogMessage message);

    /// <inheritdoc cref="IAsyncLogTarget.FlushAsync" />
    protected virtual ValueTask FlushAsync() {
        return ValueTask.CompletedTask;
    }

    /// <summary>
    ///     Initializes all data required to receive log messages asynchronously
    /// </summary>
    protected virtual ValueTask InitializeAsync() {
        return ValueTask.CompletedTask;
    }

    /// <inheritdoc cref="DisposeAsync()" />
    protected virtual ValueTask DisposeAsync(bool disposing) {
        return ValueTask.CompletedTask;
    }
        
    private void ThrowIfDisposed() {
        if(_disposed)
            throw new ObjectDisposedException(Name);
    }
        
    private async ValueTask EnsureInitializedAsync() {
        if(_initialized) {
            return;
        }

        await _syncRoot.WaitAsync();
        try {
            if(_initialized) {
                return;
            }
        
            await InitializeAsync();
            _initialized = true;
        } finally {
            _syncRoot.Release();
        }
    }
}