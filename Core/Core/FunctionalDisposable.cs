using System;

namespace DotLogix.Core {
    /// <summary>
    /// A functional class to wrap a value as a disposable to execute an action after disposal
    /// </summary>
    public class FunctionalDisposable : IDisposable {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public FunctionalDisposable(Action onDisposeFunc) {
            OnDisposeFunc = onDisposeFunc;
        }
        /// <summary>
        /// The dispose method
        /// </summary>
        protected Action OnDisposeFunc { get; }


        /// <inheritdoc />
        public void Dispose() {
            OnDisposeFunc?.Invoke();
        }
    }
}