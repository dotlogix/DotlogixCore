using System;
using System.Collections.Generic;
using System.Linq;
using DotLogix.Architecture.Domain.Models.Base;

namespace DotLogix.Architecture.Domain.Mapper {
    public abstract class MapperBase<TEntity, TModel> : IMapper<TEntity, TModel> where TModel : SimpleModel, new() where TEntity : new() {
        #region Map

        public abstract void MapEntityToModel(TEntity source, TModel target);
        public abstract void MapModelToEntity(TEntity target, TModel source);
        #endregion

        #region ToModel

        public virtual TModel ToModel(TEntity source) {
            var model = new TModel();
            MapEntityToModel(source, model);
            return model;
        }

        public virtual TDerived ToModel<TDerived>(TEntity source) where TDerived : TModel, new() {
            var model = new TDerived();
            MapEntityToModel(source, model);
            return model;
        }

        public virtual IEnumerable<TModel> ToModels(IEnumerable<TEntity> source) {
            return source.Select(ToModel).ToList();
        }

        public virtual IEnumerable<TDerived> ToModels<TDerived>(IEnumerable<TEntity> source) where TDerived : TModel, new() {
            return source.Select(ToModel<TDerived>).ToList();
        }

        #endregion

        #region ToEntity

        public virtual TEntity ToEntity(TModel source) {
            if(source.Guid == Guid.Empty)
                source.Guid = Guid.NewGuid();

            var entity = new TEntity();
            MapModelToEntity(entity, source);
            return entity;
        }


        public virtual IEnumerable<TEntity> ToEntities(IEnumerable<TModel> source) {
            return source.Select(ToEntity).ToList();
        }

        #endregion
    }
}