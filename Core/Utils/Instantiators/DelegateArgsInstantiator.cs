using System;

namespace DotLogix.Core.Utils.Instantiators {
    /// <inheritdoc />
    public class DelegateArgsInstantiator : IArgsInstantiator {
        private readonly Func<object[], object> _getInstanceFunc;

        /// <summary>
        /// Create a new instance of <see cref="DelegateArgsInstantiator"/>
        /// </summary>
        public DelegateArgsInstantiator(Func<object[], object> instantiateFunc) {
            _getInstanceFunc = instantiateFunc ?? throw new ArgumentNullException(nameof(instantiateFunc));
        }

        /// <inheritdoc />
        public object GetInstance(params object[] args) {
            return _getInstanceFunc.Invoke(args);
        }
    }
}