using System;

namespace DotLogix.Core.Utils.Instantiators {
    /// <inheritdoc />
    public class DelegateInstantiator : IInstantiator {
        private readonly Func<object> _getInstanceFunc;

        /// <summary>
        /// Create a new instance of <see cref="DelegateInstantiator"/>
        /// </summary>
        public DelegateInstantiator(Func<object> instantiateFunc) {
            _getInstanceFunc = instantiateFunc ?? throw new ArgumentNullException(nameof(instantiateFunc));
        }

        /// <inheritdoc />
        public object GetInstance() {
            return _getInstanceFunc.Invoke();
        }
    }
}