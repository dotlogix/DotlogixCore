// ==================================================
// Copyright 2019(C) , DotLogix
// File:  FluentEntityMapper.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  ..
// LastEdited:  05.08.2019
// ==================================================

using System;
using System.Collections.Generic;
using System.Linq;
using DotLogix.Core;
using DotLogix.Core.Extensions;
using DotLogix.Core.Utils.Mappers;

namespace DotLogix.Architecture.Domain.Mapper {
    /// <summary>
    /// An interface for 1:1 mapping of params with their corresponding entities
    /// </summary>
    public interface IParamMapper<TParams, TEntity> where TEntity : new() {
        /// <summary>
        /// Maps a param model to its corresponding entity
        /// </summary>
        void MapParamsToEntity(TParams source, TEntity target);

        /// <summary>
        /// Maps a param model to a new entity
        /// </summary>
        TEntity ToEntity(TParams source);

        /// <summary>
        /// Maps a range of param models to new entities
        /// </summary>
        IEnumerable<TEntity> ToEntities(IEnumerable<TParams> source);
    }

    /// <summary>
    /// A basic implementation of the <see cref="IParamMapper{TEntity,TModel}"/> using reflection and shallow copying for 1:1 mapping of entities to their corresponding model
    /// </summary>
    public class FluentParamMapper<TParams, TEntity> : IParamMapper<TParams, TEntity> where TEntity : new()
    {
        private readonly IMapper<TParams, TEntity> _paramsToEntity;

        /// <summary>
        /// Creates a new instance of <see cref="FluentEntityMapper{TEntity,TModel}"/>
        /// </summary>
        public FluentParamMapper(bool autoMap = true, bool ignoreUndefinedOptional = true, Action<MapBuilder<TParams, TEntity>> configEntityToModel = null)
        {

            var mapBuilder = new MapBuilder<TParams, TEntity>();
            if (autoMap) {
                foreach (var property in mapBuilder.DynamicSourceType.Properties)
                {
                    var targetProperty = mapBuilder.DynamicTargetType.GetProperty(property.Name);
                    if (targetProperty == null)
                        continue;

                    if (property.ValueType.IsAssignableTo<IOptional>())
                        mapBuilder.Map(property.Name, b => OptionalToValue(b, targetProperty.ValueType, ignoreUndefinedOptional));
                    else
                        mapBuilder.Map(property.Name);
                }
            }
            configEntityToModel?.Invoke(mapBuilder);
            _paramsToEntity = mapBuilder.Build();
        }

        /// <summary>
        /// Maps a param model to its corresponding entity
        /// </summary>
        public void MapParamsToEntity(TParams source, TEntity target)
        {
            _paramsToEntity.Map(source, target);
        }


        /// <summary>
        /// Maps a param model to a new entity
        /// </summary>
        public virtual TEntity ToEntity(TParams source)
        {
            var entity = new TEntity();
            MapParamsToEntity(source, entity);
            return entity;
        }


        /// <summary>
        /// Maps a range of param models to new entities
        /// </summary>
        public virtual IEnumerable<TEntity> ToEntities(IEnumerable<TParams> source)
        {
            return source.Select(ToEntity).ToList();
        }

        private static void OptionalToValue(ValueMapBuilder<TParams, object, TEntity, object> builder, Type targetType, bool ignoreUndefinedOptional) {
            if(ignoreUndefinedOptional)
                builder.GetOnlyIf((object v) => !(v is IOptional opt) || opt.IsDefined);
            builder.ConvertWith(v => v is IOptional opt && opt.Value.TryConvertTo(targetType, out var converted) ? converted : default);
        }
    }
}