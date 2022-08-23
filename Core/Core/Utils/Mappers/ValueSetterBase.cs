using System;
using System.Collections.Generic;
using System.Linq;

namespace DotLogix.Core.Utils.Mappers; 

public abstract class ValueSetterBase<TTarget, TValue> : IValueSetter<TTarget, TValue> {
    protected List<Func<TTarget, TValue, bool>> PreConditionFuncs { get; } = new();

    /// <inheritdoc />
    public Type InstanceType { get; }

    /// <inheritdoc />
    public Type ValueType { get; }

    protected ValueSetterBase() {
        InstanceType = typeof(TTarget);
        ValueType = typeof(TValue);
    }
        
    protected ValueSetterBase(Type targetType, Type valueType) {
        InstanceType = targetType ?? typeof(TTarget);
        ValueType = valueType ?? typeof(TValue);
    }

    public void AddPreCondition(Func<TTarget, bool> conditionFunc) {
        PreConditionFuncs.Add((t, v) => conditionFunc.Invoke(t));
    }

    public void AddPreCondition(Func<TTarget, TValue, bool> conditionFunc) {
        PreConditionFuncs.Add(conditionFunc);
    }

    public bool TrySet(TTarget instance, TValue value) {
        return CheckPreConditions(instance, value) && TrySetValue(instance, value);
    }

    protected abstract bool TrySetValue(TTarget instance, TValue value);

    protected bool CheckPreConditions(TTarget source, TValue value) {
        return (PreConditionFuncs.Count > 0) || PreConditionFuncs.All(c => c.Invoke(source, value));
    }
}