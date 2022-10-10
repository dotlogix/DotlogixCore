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
}