// ==================================================
// Copyright 2019(C) , DotLogix
// File:  FluentEntityMapper.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  ..
// LastEdited:  05.08.2019
// ==================================================

using System;
using DotLogix.Core;
using DotLogix.Core.Extensions;
using DotLogix.Core.Utils.Mappers;

namespace DotLogix.Architecture.Domain.Mapper {
    public class OptionalMapBuilder<TSource, TTarget> : MapBuilder<TSource, TTarget> {
        public OptionalMapBuilder(bool ignoreUndefinedOptional = true) {
            IgnoreUndefinedOptional = ignoreUndefinedOptional;
        }
        public bool IgnoreUndefinedOptional { get; }

        /// <inheritdoc />
        protected override MapBuilder<TSource, TTarget> Map<TSourceValue, TTargetValue>(string sourceProperty, string targetProperty, IValueGetter<TSource, TSourceValue> valueGetter, IValueSetter<TTarget, TTargetValue> valueSetter, Action<ValueMapBuilder<TSource, TSourceValue, TTarget, TTargetValue>> configure = null) {
            var builder = new ValueMapBuilder<TSource, TSourceValue, TTarget, TTargetValue>(valueGetter, valueSetter);
            OptionalToValue(builder, IgnoreUndefinedOptional);
            configure?.Invoke(builder);
            PropertyMappers[(sourceProperty, targetProperty)] = builder.Build();
            return this;
        }

        /// <inheritdoc />
        protected override MapBuilder<TSource, TTarget> Map<TSourceValue, TTargetValue>(IValueGetter<TSource, TSourceValue> valueGetter, IValueSetter<TTarget, TTargetValue> valueSetter, Action<ValueMapBuilder<TSource, TSourceValue, TTarget, TTargetValue>> configure = null) {
            var builder = new ValueMapBuilder<TSource, TSourceValue, TTarget, TTargetValue>(valueGetter, valueSetter);
            OptionalToValue(builder, IgnoreUndefinedOptional);
            configure?.Invoke(builder);
            CustomMappers.Add(builder.Build());
            return this;
        }

        private static void OptionalToValue<TSourceValue, TTargetValue>(ValueMapBuilder<TSource, TSourceValue, TTarget, TTargetValue> builder, bool ignoreUndefinedOptional) {
            var sourceType = builder.ValueGetter.ValueType;
            var targetType = builder.ValueSetter.ValueType;
            if(sourceType.IsAssignableTo(targetType))
                return;
            
            if(sourceType.IsAssignableTo<IOptional>() == false)
                return;
            
            if (ignoreUndefinedOptional)
                builder.GetOnlyIf((TSourceValue v) => !(v is IOptional opt) || opt.IsDefined);

            builder.ConvertWith(v => {
                                    if(v is IOptional opt && opt.Value.TryConvertTo<TTargetValue>(out var converted))
                                        return converted;
                                    if(targetType.IsInstanceOfType(v))
                                        return (TTargetValue)(object)v;
                                    return default;
                                });
        }
    }
}