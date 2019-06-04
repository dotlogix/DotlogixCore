using System;

namespace DotLogix.Core.Utils.Instantiators {
    public class DelegateArgsInstantiator : IArgsInstantiator {
        private readonly Func<object[], object> _getInstanceFunc;

        public DelegateArgsInstantiator(Func<object[], object> instantiateFunc) {
            _getInstanceFunc = instantiateFunc ?? throw new ArgumentNullException(nameof(instantiateFunc));
        }

        public object GetInstance(params object[] args) {
            return _getInstanceFunc.Invoke(args);
        }
    }
}