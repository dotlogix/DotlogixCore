using System;

namespace DotLogix.Core.Utils.Mappers {
    public class ValueGetter<TSource, TValue> : ValueGetterBase<TSource, TValue> {
        private readonly Func<TSource, TValue> _getterFunc;

        /// <inheritdoc />
        public ValueGetter(Func<TSource, TValue> getterFunc) {
            _getterFunc = getterFunc;
        }

        /// <inheritdoc />
        public ValueGetter(Type sourceType, Type valueType, Func<TSource, TValue> getterFunc) : base(sourceType, valueType) {
            _getterFunc = getterFunc;
        }

        protected override bool TryGetValue(TSource source, out TValue value) {
            value = _getterFunc.Invoke(source);
            return true;
        }
    }
}