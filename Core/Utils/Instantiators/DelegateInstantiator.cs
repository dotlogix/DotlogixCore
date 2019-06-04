using System;

namespace DotLogix.Core.Utils.Instantiators {
    public class DelegateInstantiator : IInstantiator {
        private readonly Func<object> _getInstanceFunc;

        public DelegateInstantiator(Func<object> instantiateFunc) {
            _getInstanceFunc = instantiateFunc ?? throw new ArgumentNullException(nameof(instantiateFunc));
        }

        public object GetInstance() {
            return _getInstanceFunc.Invoke();
        }
    }
}