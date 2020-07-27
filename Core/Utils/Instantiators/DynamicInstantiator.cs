using System;
using DotLogix.Core.Extensions;
using DotLogix.Core.Reflection.Dynamics;

namespace DotLogix.Core.Utils.Instantiators {
    /// <inheritdoc />
    public class DynamicInstantiator : IInstantiator
    {
        private readonly DynamicCtor _ctor;
        /// <summary>
        /// Create a new instance of <see cref="DynamicInstantiator"/>
        /// </summary>
        public DynamicInstantiator(DynamicCtor ctor) {
            _ctor = ctor;
        }

        /// <inheritdoc />
        public object GetInstance()
        {
            return _ctor.Invoke();
        }
    }
    
    /// <inheritdoc />
    public class DynamicInstantiator<T> : IInstantiator<T> {
        private readonly DynamicCtor _ctor;
        /// <summary>
        /// Create a new instance of <see cref="DynamicInstantiator"/>
        /// </summary>
        public DynamicInstantiator(DynamicCtor ctor) {
            _ctor = ctor;

            if (ctor.ReflectedType.IsAssignableTo<T>() == false)
                throw new ArgumentException($"Type {typeof(T).Name} is not assignable to generic type {ctor.ReflectedType.Name}");
        }

        /// <inheritdoc />
        object IInstantiator.GetInstance() => GetInstance();

        /// <inheritdoc />
        public T GetInstance()
        {
            return (T)_ctor.Invoke();
        }
    }
}