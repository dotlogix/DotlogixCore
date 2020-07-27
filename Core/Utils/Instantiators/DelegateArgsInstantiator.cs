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
    
    /// <inheritdoc />
    public class DelegateArgsInstantiator<T> : IArgsInstantiator<T> {
        private readonly Func<object[], T> _getInstanceFunc;

        /// <summary>
        /// Create a new instance of <see cref="DelegateArgsInstantiator"/>
        /// </summary>
        public DelegateArgsInstantiator(Func<object[], T> instantiateFunc) {
            _getInstanceFunc = instantiateFunc ?? throw new ArgumentNullException(nameof(instantiateFunc));
        }

        /// <inheritdoc />
        object IArgsInstantiator.GetInstance(params object[] args) => GetInstance(args);

        /// <inheritdoc />
        public T GetInstance(params object[] args) {
            return _getInstanceFunc.Invoke(args);
        }
    }
}