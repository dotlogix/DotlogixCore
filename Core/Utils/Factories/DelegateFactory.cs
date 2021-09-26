using System;

namespace DotLogix.Core.Utils.Factories {
    /// <inheritdoc />
    public class DelegateFactory : IFactory {
        private readonly Func<object> _getInstanceFunc;

        /// <summary>
        /// Create a new instance of <see cref="DelegateFactory"/>
        /// </summary>
        public DelegateFactory(Func<object> instantiateFunc) {
            _getInstanceFunc = instantiateFunc ?? throw new ArgumentNullException(nameof(instantiateFunc));
        }

        /// <inheritdoc />
        public object GetInstance() {
            return _getInstanceFunc.Invoke();
        }
    }
    
    /// <inheritdoc />
    public class DelegateFactory<T> : IFactory<T> {
        private readonly Func<T> _getInstanceFunc;

        /// <summary>
        /// Create a new instance of <see cref="DelegateFactory"/>
        /// </summary>
        public DelegateFactory(Func<T> instantiateFunc) {
            _getInstanceFunc = instantiateFunc ?? throw new ArgumentNullException(nameof(instantiateFunc));
        }

        /// <inheritdoc />
        object IFactory.GetInstance() {
            return GetInstance();
        }

        /// <inheritdoc />
        public T GetInstance() {
            return _getInstanceFunc.Invoke();
        }
    }
}