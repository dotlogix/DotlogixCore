using System;
using DotLogix.Core.Reflection.Dynamics;

namespace DotLogix.Core.Utils.Instantiators {
    /// <inheritdoc />
    public class SingletonInstantiator : IInstantiator {
        private readonly DynamicProperty _property;
        /// <summary>
        /// Create a new instance of <see cref="SingletonInstantiator"/>
        /// </summary>
        public SingletonInstantiator(DynamicProperty property) {
            _property = property ?? throw new ArgumentNullException(nameof(property));
        }

        /// <inheritdoc />
        public object GetInstance() {
            return _property.GetValue();
        }
    }
    
    /// <inheritdoc />
    public class SingletonInstantiator<T> : IInstantiator<T> {
        private readonly DynamicProperty _property;
        /// <summary>
        /// Create a new instance of <see cref="SingletonInstantiator"/>
        /// </summary>
        public SingletonInstantiator(DynamicProperty property) {
            _property = property ?? throw new ArgumentNullException(nameof(property));
        }

        /// <inheritdoc />
        object IInstantiator.GetInstance() => GetInstance();

        /// <inheritdoc />
        public T GetInstance() {
            return (T)_property.GetValue();
        }
    }
}