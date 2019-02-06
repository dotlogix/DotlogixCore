using System;
using System.Collections.Generic;
using System.Text;

namespace DotLogix.Core
{
    public class FunctionalDisposable : IDisposable {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public FunctionalDisposable(Action onDisposeFunc) {
            OnDisposeFunc = onDisposeFunc;
        }
        protected Action OnDisposeFunc { get; }

        public void Dispose() {
            OnDisposeFunc?.Invoke();
        }
    }
}
