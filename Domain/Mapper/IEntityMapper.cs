// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IMapper.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.04.2018
// LastEdited:  06.04.2018
// ==================================================

#region
using System.Collections.Generic;
#endregion

namespace DotLogix.Architecture.Domain.Mapper {
    /// <summary>
    /// An interface for 1:1 mapping of entities with their corresponding model
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TModel"></typeparam>
    public interface IEntityMapper<TEntity, TModel> {
        #region Map

        /// <summary>
        /// Maps an entity to its corresponding model
        /// </summary>
        void MapEntityToModel(TEntity source, TModel target);
        /// <summary>
        /// Maps an model to its corresponding entity
        /// </summary>
        void MapModelToEntity(TEntity target, TModel source);

        #endregion

        #region ToModel

        /// <summary>
        /// Maps an entity to a new model
        /// </summary>
        TModel ToModel(TEntity source);
        /// <summary>
        /// Maps an entity to a new model derived of the original one
        /// </summary>
        TDerived ToModel<TDerived>(TEntity source) where TDerived : TModel, new();

        /// <summary>
        /// Maps a range of entities to new models
        /// </summary>
        IEnumerable<TModel> ToModels(IEnumerable<TEntity> source);
        /// <summary>
        /// Maps a range of entities to new models derived of the original one
        /// </summary>
        IEnumerable<TDerived> ToModels<TDerived>(IEnumerable<TEntity> source) where TDerived : TModel, new();

        #endregion

        #region ToEntity


        /// <summary>
        /// Maps a model to a new entity
        /// </summary>
        TEntity ToEntity(TModel source);

        /// <summary>
        /// Maps a range of models to new entities
        /// </summary>
        IEnumerable<TEntity> ToEntities(IEnumerable<TModel> source);

        #endregion
    }
}
