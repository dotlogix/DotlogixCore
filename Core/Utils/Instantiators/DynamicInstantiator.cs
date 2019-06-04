using DotLogix.Core.Reflection.Dynamics;

namespace DotLogix.Core.Utils.Instantiators {
    public class DynamicInstantiator : IInstantiator
    {
        private readonly DynamicCtor _ctor;
        public DynamicInstantiator(DynamicCtor ctor) {
            _ctor = ctor;
        }

        public object GetInstance()
        {
            return _ctor.Invoke();
        }
    }
}