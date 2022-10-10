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
}