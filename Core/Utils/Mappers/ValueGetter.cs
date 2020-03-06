using System;

namespace DotLogix.Core.Utils.Mappers {
    public class ValueGetter<TSource, TValue> : ValueGetterBase<TSource, TValue> {
        private readonly Func<TSource, TValue> _getterFunc;

        public ValueGetter(Func<TSource, TValue> getterFunc) {
            _getterFunc = getterFunc;
        }


        protected override bool TryGetValue(TSource source, out TValue value) {
            value = _getterFunc.Invoke(source);
            return true;
        }
    }
}