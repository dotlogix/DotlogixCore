using System;
using System.Collections.Generic;
using System.Linq;
using DotLogix.Architecture.Common.Models.Base;

namespace DotLogix.Architecture.Common.Mapper {
    public abstract class MapperBase<TEntity, TModel> : IMapper<TEntity, TModel> where TModel : SimpleModel, new() where TEntity : new() {
        public abstract void FromEntityToModel(TEntity source, TModel target);

        public abstract void FromModelToEntity(TEntity target, TModel source);

        public TModel ToModel(TEntity source) {
            var model = new TModel();
            FromEntityToModel(source, model);
            return model;
        }

        public TDerived ToModel<TDerived>(TEntity source) where TDerived : TModel, new() {
            var model = new TDerived();
            FromEntityToModel(source, model);
            return model;
        }

        public TEntity ToEntity(TModel source) {
            if(source.Guid == Guid.Empty)
                source.Guid = Guid.NewGuid();

            var entity = new TEntity();
            FromModelToEntity(entity, source);
            return entity;
        }


        public IEnumerable<TModel> ToModels(IEnumerable<TEntity> source) {
            return source.Select(ToModel).ToList();
        }

        public IEnumerable<TDerived> ToModels<TDerived>(IEnumerable<TEntity> source) where TDerived : TModel, new() {
            return source.Select(ToModel<TDerived>).ToList();
        }

        public IEnumerable<TEntity> ToEntities(IEnumerable<TModel> source) {
            return source.Select(ToEntity).ToList();
        }
    }
}