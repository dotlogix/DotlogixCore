using DotLogix.Core.Reflection.Dynamics;

namespace DotLogix.Core.Utils.Instantiators {
    public class DynamicArgsInstantiator : IArgsInstantiator
    {
        private readonly DynamicCtor _ctor;
        public DynamicArgsInstantiator(DynamicCtor ctor) {
            _ctor = ctor;
        }

        public object GetInstance(params object[] args)
        {
            return _ctor.Invoke(args);
        }
    }
}