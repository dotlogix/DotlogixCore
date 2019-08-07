using System;

namespace DotLogix.Core.Utils.Mappers {
    public class ValueSetter<TTarget, TValue> : ValueSetterBase<TTarget, TValue> {
        private readonly Action<TTarget, TValue> _setterFunc;

        public ValueSetter(Action<TTarget, TValue> setterFunc) {
            _setterFunc = setterFunc;
        }


        protected override bool TrySetValue(TTarget source, TValue value) {
            _setterFunc.Invoke(source, value);
            return true;
        }
    }
}