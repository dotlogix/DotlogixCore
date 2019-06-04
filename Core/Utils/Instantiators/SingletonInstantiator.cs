using System;
using DotLogix.Core.Reflection.Dynamics;

namespace DotLogix.Core.Utils.Instantiators {
    public class SingletonInstantiator : IInstantiator {
        private readonly DynamicProperty _property;
        public SingletonInstantiator(DynamicProperty property) {
            _property = property ?? throw new ArgumentNullException(nameof(property));
        }

        public object GetInstance() {
            return _property.GetValue();
        }
    }
}