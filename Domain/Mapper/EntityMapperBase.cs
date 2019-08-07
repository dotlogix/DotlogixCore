using System;
using System.Collections.Generic;
using System.Linq;
using DotLogix.Architecture.Domain.Models.Base;

namespace DotLogix.Architecture.Domain.Mapper {
    /// <summary>
    /// A basic implementation of the <see cref="IEntityMapper{TEntity,TModel}"/> for 1:1 mapping of entities to their corresponding model
    /// </summary>
    public abstract class EntityMapperBase<TEntity, TModel> : IEntityMapper<TEntity, TModel> where TModel : SimpleModel, new() where TEntity : new()
    {
        #region Map
        /// <inheritdoc />
        public abstract void MapEntityToModel(TEntity source, TModel target);
        /// <inheritdoc />
        public abstract void MapModelToEntity(TEntity target, TModel source);
        #endregion

        #region ToModel

        /// <inheritdoc />
        public virtual TModel ToModel(TEntity source)
        {
            var model = new TModel();
            MapEntityToModel(source, model);
            return model;
        }

        /// <inheritdoc />
        public virtual TDerived ToModel<TDerived>(TEntity source) where TDerived : TModel, new()
        {
            var model = new TDerived();
            MapEntityToModel(source, model);
            return model;
        }

        /// <inheritdoc />
        public virtual IEnumerable<TModel> ToModels(IEnumerable<TEntity> source)
        {
            return source.Select(ToModel).ToList();
        }

        /// <inheritdoc />
        public virtual IEnumerable<TDerived> ToModels<TDerived>(IEnumerable<TEntity> source) where TDerived : TModel, new()
        {
            return source.Select(ToModel<TDerived>).ToList();
        }

        #endregion

        #region ToEntity

        /// <inheritdoc />
        public virtual TEntity ToEntity(TModel source)
        {
            var entity = new TEntity();
            MapModelToEntity(entity, source);
            return entity;
        }


        /// <inheritdoc />
        public virtual IEnumerable<TEntity> ToEntities(IEnumerable<TModel> source)
        {
            return source.Select(ToEntity).ToList();
        }

        #endregion
    }
}