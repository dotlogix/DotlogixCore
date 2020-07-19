using System.Collections.Generic;

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
}