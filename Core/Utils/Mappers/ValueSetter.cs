using System;

namespace DotLogix.Core.Utils.Mappers {
    public class ValueSetter<TTarget, TValue> : ValueSetterBase<TTarget, TValue> {
        private readonly Action<TTarget, TValue> _setterFunc;

        public ValueSetter(Action<TTarget, TValue> setterFunc) {
            _setterFunc = setterFunc;
        }

        /// <inheritdoc />
        public ValueSetter(Type targetType, Type valueType, Action<TTarget, TValue> setterFunc) : base(targetType, valueType) {
            _setterFunc = setterFunc;
        }


        protected override bool TrySetValue(TTarget instance, TValue value) {
            _setterFunc.Invoke(instance, value);
            return true;
        }
    }
}