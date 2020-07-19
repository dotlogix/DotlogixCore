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
    public static class Mappers {
        /// <summary>
        /// Creates a mapper based on a configuration function
        /// </summary>
        public static IMapper<TSource, TTarget> Map<TSource, TTarget>(Action<MapBuilder<TSource, TTarget>> configure = null) {
            var mapBuilder = new MapBuilder<TSource, TTarget>();
            configure?.Invoke(mapBuilder);
            return mapBuilder.Build();
        }

        /// <summary>
        /// Creates a mapper based on a configuration function auto-maps all supported properties (s.[name] => t.[name])
        /// </summary>
        public static IMapper<TSource, TTarget> AutoMap<TSource, TTarget>(Action<MapBuilder<TSource, TTarget>> configure = null) {
            var mapBuilder = new MapBuilder<TSource, TTarget>();
            mapBuilder.AutoMap();
            configure?.Invoke(mapBuilder);
            return mapBuilder.Build();
        }


        /// <summary>
        /// Creates a mapper based on a configuration function
        /// </summary>
        public static IMapper<object, object> Map(Type sourceType, Type targetType, Action <MapBuilder<object, object>> configure = null) {
            var mapBuilder = new MapBuilder<object, object>(sourceType, targetType);
            configure?.Invoke(mapBuilder);
            return mapBuilder.Build();
        }
        /// <summary>
        /// Creates a mapper based on a configuration function auto-maps all supported properties (s.[name] => t.[name])
        /// </summary>
        public static IMapper<object, object> AutoMap(Type sourceType, Type targetType, Action<MapBuilder<object, object>> configure = null)
        {
            var mapBuilder = new MapBuilder<object, object>(sourceType, targetType);
            mapBuilder.AutoMap();
            configure?.Invoke(mapBuilder);
            return mapBuilder.Build();
        }
    }
}
