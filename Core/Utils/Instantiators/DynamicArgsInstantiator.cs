using System;
using DotLogix.Core.Extensions;
using DotLogix.Core.Reflection.Dynamics;

namespace DotLogix.Core.Utils.Instantiators {
    /// <inheritdoc />
    public class DynamicArgsInstantiator : IArgsInstantiator
    {
        private readonly DynamicCtor _ctor;
        /// <summary>
        /// Create a new instance of <see cref="DynamicArgsInstantiator"/>
        /// </summary>
        public DynamicArgsInstantiator(DynamicCtor ctor) {
            _ctor = ctor;
        }

        /// <inheritdoc />
        public object GetInstance(params object[] args)
        {
            return _ctor.Invoke(args);
        }
    }
    
    /// <inheritdoc />
    public class DynamicArgsInstantiator<T> : IArgsInstantiator<T> {
        private readonly DynamicCtor _ctor;
        /// <summary>
        /// Create a new instance of <see cref="DynamicArgsInstantiator"/>
        /// </summary>
        public DynamicArgsInstantiator(DynamicCtor ctor) {
            _ctor = ctor;

            if(ctor.ReflectedType.IsAssignableTo<T>() == false)
                throw new ArgumentException($"Type {typeof(T).Name} is not assignable to generic type {ctor.ReflectedType.Name}");
        }

        /// <inheritdoc />
        object IArgsInstantiator.GetInstance(params object[] args) => GetInstance(args);

        /// <inheritdoc />
        public T GetInstance(params object[] args)
        {
            return (T)_ctor.Invoke(args);
        }
    }
}