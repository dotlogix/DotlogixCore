// ==================================================
// Copyright 2019(C) , DotLogix
// File:  FluentEntityMapper.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  ..
// LastEdited:  05.08.2019
// ==================================================

using System;
using DotLogix.Architecture.Domain.Models;
using DotLogix.Core.Utils.Mappers;

namespace DotLogix.Architecture.Domain.Mapper {
    /// <summary>
    /// A basic implementation of the <see cref="IEntityMapper{TEntity,TModel}"/> using reflection and shallow copying for 1:1 mapping of entities to their corresponding model
    /// </summary>
    public class FluentEntityMapper<TEntity, TModel> : EntityMapperBase<TEntity, TModel> where TModel : ISimpleModel, new() where TEntity : new()
    {
        private readonly IMapper<TEntity, TModel> _entityToModel;
        private readonly IMapper<TModel, TEntity> _modelToEntity;

        /// <summary>
        /// Creates a new instance of <see cref="FluentEntityMapper{TEntity,TModel}"/>
        /// </summary>
        public FluentEntityMapper(bool autoMap = true, Action<MapBuilder<TEntity, TModel>> configEntityToModel = null, Action<MapBuilder<TModel, TEntity>> configModelToEntity = null) {
            _entityToModel = autoMap
                             ? Mappers.AutoMap(configEntityToModel)
                             : Mappers.Map(configEntityToModel);
            _modelToEntity = autoMap
                             ? Mappers.AutoMap(configModelToEntity)
                             : Mappers.Map(configModelToEntity);
        }

        /// <inheritdoc />
        public override void MapEntityToModel(TEntity source, TModel target)
        {
            _entityToModel.Map(source, target);
        }

        /// <inheritdoc />
        public override void MapModelToEntity(TEntity target, TModel source)
        {
            _modelToEntity.Map(source, target);
        }
    }
}