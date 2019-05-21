// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ProjectionMapper.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.04.2018
// LastEdited:  06.04.2018
// ==================================================

#region
#endregion

using DotLogix.Architecture.Common.Models.Base;
using DotLogix.Core.Reflection.Projections;

namespace DotLogix.Architecture.Common.Mapper {
    public class ProjectionMapper<TEntity, TModel> : MapperBase<TEntity, TModel> where TModel : SimpleModel, new() where TEntity : new() {
        private readonly TypeProjector _projector;

        public ProjectionMapper(CreateProjectionsCallback callback) {
            _projector = TypeProjector.Create<TEntity, TModel>(callback);
        }

        public ProjectionMapper(IProjectionFactory factory = null) {
            _projector = TypeProjector.Create<TEntity, TModel>(factory);
        }

        public override void FromEntityToModel(TEntity source, TModel target) {
            _projector.ProjectLeftToRight(source, target);
        }

        public override void FromModelToEntity(TEntity target, TModel source) {
            _projector.ProjectRightToLeft(target, source);
        }
    }
}
