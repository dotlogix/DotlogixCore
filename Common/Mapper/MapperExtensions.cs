// ==================================================
// Copyright 2018(C) , DotLogix
// File:  MapperExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.04.2018
// LastEdited:  06.04.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using DotLogix.Architecture.Common.Options;
#endregion

namespace DotLogix.Architecture.Common.Mapper {
    public static class MapperExtensions {
        public static TModel ToModel<TModel, TEntity>(this IMapper<TEntity, TModel> mapper, TEntity source) where TModel : new() {
            var model = new TModel();
            mapper.FromEntityToModel(source, model);
            return model;
        }

        public static TDerivedModel ToModel<TDerivedModel, TModel, TEntity>(this IMapper<TEntity, TModel> mapper, TEntity source) where TDerivedModel : TModel, new() {
            var model = new TDerivedModel();
            mapper.FromEntityToModel(source, model);
            return model;
        }

        public static TEntity ToEntity<TModel, TEntity>(this IMapper<TEntity, TModel> mapper, TModel source) where TEntity : new() where TModel : IGuid {
            if(source.Guid == Guid.Empty)
                source.Guid = Guid.NewGuid();

            var entity = new TEntity();
            mapper.FromModelToEntity(entity, source);
            return entity;
        }


        public static IEnumerable<TModel> ToModels<TModel, TEntity>(this IMapper<TEntity, TModel> mapper, IEnumerable<TEntity> source) where TModel : new() {
            return source.Select(mapper.ToModel).ToList();
        }

        public static IEnumerable<TEntity> ToEntities<TModel, TEntity>(this IMapper<TEntity, TModel> mapper, IEnumerable<TModel> source) where TModel : IGuid where TEntity : new() {
            return source.Select(mapper.ToEntity).ToList();
        }
    }
}
