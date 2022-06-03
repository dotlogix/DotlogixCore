using System;

namespace DotLogix.Core.Utils.Mappers {
    public abstract class ValueMapperBase<TSource, TSourceValue, TTarget, TTargetValue> : MapperBase<TSource, TTarget>, IValueMapper<TSource, TTarget> {
        public Type SourceValueType { get; }
        public Type TargetValueType { get; }
        protected ValueMapperBase() {
            SourceValueType = typeof(TSourceValue);
            TargetValueType = typeof(TTargetValue);
        }

        protected ValueMapperBase(Type sourceType, Type targetType, Type sourceValueType, Type targetValueType)
            : base(sourceType, targetType) {
            SourceValueType = sourceValueType ?? typeof(TSourceValue);
            TargetValueType = targetValueType ?? typeof(TTargetValue);
        }
    }
}