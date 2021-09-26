// ==================================================
// Copyright 2020(C) , Alexander Schill
// File:  Mappers.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// ==================================================

using System;

namespace DotLogix.Core.Utils.Mappers {
    /// <summary>
    /// A static class class helping to create type to type mappers
    /// </summary>
    public static class OptionalMapBuilders {
        /// <summary>
        /// Creates a mapper based on a configuration function
        /// </summary>
        public static IMapper<TSource, TTarget> Map<TSource, TTarget>(Action<MapBuilder<TSource, TTarget>> configure = null, bool ignoreUndefinedOptional = true) {
            var mapBuilder = new OptionalMapBuilder<TSource, TTarget> {
                IgnoreUndefinedOptional = ignoreUndefinedOptional
            };
            configure?.Invoke(mapBuilder);
            return mapBuilder.Build();
        }

        /// <summary>
        /// Creates a mapper based on a configuration function auto-maps all supported properties (s.[name] => t.[name])
        /// </summary>
        public static IMapper<TSource, TTarget> AutoMap<TSource, TTarget>(Action<MapBuilder<TSource, TTarget>> configure = null, bool ignoreUndefinedOptional = true) {
            var mapBuilder = new OptionalMapBuilder<TSource, TTarget> {
                IgnoreUndefinedOptional = ignoreUndefinedOptional
            };
            mapBuilder.AutoMap();
            configure?.Invoke(mapBuilder);
            return mapBuilder.Build();
        }


        /// <summary>
        /// Creates a mapper based on a configuration function
        /// </summary>
        public static IMapper<object, object> Map(Type sourceType, Type targetType, Action <MapBuilder<object, object>> configure = null, bool ignoreUndefinedOptional = true) {
            var mapBuilder = new OptionalMapBuilder<object, object>(sourceType, targetType) {
                IgnoreUndefinedOptional = ignoreUndefinedOptional
            };
            configure?.Invoke(mapBuilder);
            return mapBuilder.Build();
        }
        /// <summary>
        /// Creates a mapper based on a configuration function auto-maps all supported properties (s.[name] => t.[name])
        /// </summary>
        public static IMapper<object, object> AutoMap(Type sourceType, Type targetType, Action<MapBuilder<object, object>> configure = null, bool ignoreUndefinedOptional = true)
        {
            var mapBuilder = new OptionalMapBuilder<object, object>(sourceType, targetType) {
                IgnoreUndefinedOptional = ignoreUndefinedOptional
            };
            mapBuilder.AutoMap();
            configure?.Invoke(mapBuilder);
            return mapBuilder.Build();
        }
    }
}
