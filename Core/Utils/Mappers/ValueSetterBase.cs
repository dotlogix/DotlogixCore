using System;
using System.Collections.Generic;
using System.Linq;

namespace DotLogix.Core.Utils.Mappers {
    public abstract class ValueSetterBase<TTarget, TValue> : IValueSetter<TTarget, TValue> {
        private readonly Type _targetType;
        private readonly Type _valueType;
        protected List<Func<TTarget, TValue, bool>> PreConditionFuncs { get; } = new List<Func<TTarget, TValue, bool>>();

        /// <inheritdoc />
        public Type TargetType => _targetType ?? typeof(TTarget);

        /// <inheritdoc />
        public Type ValueType => _valueType ?? typeof(TValue);

        protected ValueSetterBase() { }
        protected ValueSetterBase(Type targetType, Type valueType) {
            _targetType = targetType;
            _valueType = valueType;
        }

        public void AddPreCondition(Func<TTarget, bool> conditionFunc) {
            PreConditionFuncs.Add((t, v) => conditionFunc.Invoke(t));
        }

        public void AddPreCondition(Func<TTarget, TValue, bool> conditionFunc) {
            PreConditionFuncs.Add(conditionFunc);
        }

        public bool TrySet(TTarget target, TValue value) {
            return CheckPreConditions(target, value) && TrySetValue(target, value);
        }

        protected abstract bool TrySetValue(TTarget source, TValue value);

        protected bool CheckPreConditions(TTarget source, TValue value) {
            return (PreConditionFuncs.Count > 0) || PreConditionFuncs.All(c => c.Invoke(source, value));
        }
    }
}