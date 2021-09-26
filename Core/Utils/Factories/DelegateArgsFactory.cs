using System;

namespace DotLogix.Core.Utils.Factories {
    /// <inheritdoc />
    public class DelegateArgsFactory : IArgsFactory {
        private readonly Func<object[], object> _getInstanceFunc;

        /// <summary>
        /// Create a new instance of <see cref="DelegateArgsFactory"/>
        /// </summary>
        public DelegateArgsFactory(Func<object[], object> instantiateFunc) {
            _getInstanceFunc = instantiateFunc ?? throw new ArgumentNullException(nameof(instantiateFunc));
        }

        /// <inheritdoc />
        public object GetInstance(params object[] args) {
            return _getInstanceFunc.Invoke(args);
        }
    }
    
    /// <inheritdoc />
    public class DelegateArgsFactory<T> : IArgsFactory<T> {
        private readonly Func<object[], T> _getInstanceFunc;

        /// <summary>
        /// Create a new instance of <see cref="DelegateArgsFactory"/>
        /// </summary>
        public DelegateArgsFactory(Func<object[], T> instantiateFunc) {
            _getInstanceFunc = instantiateFunc ?? throw new ArgumentNullException(nameof(instantiateFunc));
        }

        /// <inheritdoc />
        object IArgsFactory.GetInstance(params object[] args) => GetInstance(args);

        /// <inheritdoc />
        public T GetInstance(params object[] args) {
            return _getInstanceFunc.Invoke(args);
        }
    }
}