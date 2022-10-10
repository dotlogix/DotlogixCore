// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ProjectionMapper.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.04.2018
// LastEdited:  06.04.2018
// ==================================================

#region
#endregion

using DotLogix.Architecture.Domain.Models.Base;
using DotLogix.Core.Reflection.Projections;

namespace DotLogix.Architecture.Domain.Mapper {
    /// <summary>
    /// A basic implementation of the <see cref="IEntityMapper{TEntity,TModel}"/> using reflection and shallow copying for 1:1 mapping of entities to their corresponding model
    /// </summary>
    public class ProjectionEntityMapper<TEntity, TModel> : EntityMapperBase<TEntity, TModel> where TModel : SimpleModel, new() where TEntity : new() {
        private readonly TypeProjector _projector;

        /// <summary>
        /// Creates a new instance of <see cref="ProjectionEntityMapper{TEntity,TModel}"/>
        /// </summary>
        public ProjectionEntityMapper(CreateProjectionsCallback callback) {
            _projector = TypeProjector.Create<TEntity, TModel>(callback);
        }

        /// <summary>
        /// Creates a new instance of <see cref="ProjectionEntityMapper{TEntity,TModel}"/>
        /// </summary>
        public ProjectionEntityMapper(IProjectionFactory factory = null) {
            _projector = TypeProjector.Create<TEntity, TModel>(factory);
        }

        /// <inheritdoc />
        public override void MapEntityToModel(TEntity source, TModel target) {
            _projector.ProjectLeftToRight(source, target);
        }

        /// <inheritdoc />
        public override void MapModelToEntity(TEntity target, TModel source) {
            _projector.ProjectRightToLeft(target, source);
        }
    }
}
