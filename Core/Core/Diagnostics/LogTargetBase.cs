using System;
using DotLogix.Core.Extensions;

namespace DotLogix.Core.Diagnostics {
    /// <summary>
    ///     A base class for <see cref="ILogTarget" />
    /// </summary>
    public abstract class LogTargetBase : ILogTarget {
        private readonly object _syncRoot = new();
        private bool _disposed;
        private bool _initialized;

        /// <inheritdoc />
        public virtual string Name => GetType().GetFriendlyName();

        void ILogTarget.Log(LogMessage message) {
            ThrowIfDisposed();
            EnsureInitialized();
            Log(message);
        }

        void ILogTarget.Flush() {
            ThrowIfDisposed();
            EnsureInitialized();
            Flush();
        }

        /// <inheritdoc />
        public void Dispose() {
            if(_disposed) {
                return;
            }

            _disposed = true;
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc cref="ILogTarget.Log" />
        protected abstract void Log(LogMessage message);

        /// <inheritdoc cref="ILogTarget.Flush" />
        protected virtual void Flush() { }

        /// <summary>
        ///     Initializes all data required to receive log messages
        /// </summary>
        protected virtual void Initialize() { }

        /// <inheritdoc cref="Dispose()" />
        protected virtual void Dispose(bool disposing) { }

        private void ThrowIfDisposed() {
            if(_disposed) {
                throw new ObjectDisposedException(Name);
            }
        }

        private void EnsureInitialized() {
            if(_initialized) {
                return;
            }

            lock(_syncRoot) {
                if(_initialized) {
                    return;
                }

                Initialize();
                _initialized = true;
            }
        }
    }
}