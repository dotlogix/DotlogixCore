using System;
using System.Collections.Generic;
using System.Linq;
using DotLogix.Core.Utils.Mappers;

namespace DotLogix.Architecture.Domain.Mapper {
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

            var mapBuilder = new OptionalMapBuilder<TParams, TEntity>(ignoreUndefinedOptional);
            if (autoMap) {
                mapBuilder.AutoMap();
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

    }
}