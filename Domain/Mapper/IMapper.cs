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
    public interface IMapper<TEntity, TModel> {
        #region Map

        void MapEntityToModel(TEntity source, TModel target);
        void MapModelToEntity(TEntity target, TModel source);

        #endregion

        #region ToModel

        TModel ToModel(TEntity source);
        TDerived ToModel<TDerived>(TEntity source) where TDerived : TModel, new();

        IEnumerable<TModel> ToModels(IEnumerable<TEntity> source);
        IEnumerable<TDerived> ToModels<TDerived>(IEnumerable<TEntity> source) where TDerived : TModel, new();

        #endregion

        #region ToEntity

        TEntity ToEntity(TModel source);
        IEnumerable<TEntity> ToEntities(IEnumerable<TModel> source);

        #endregion
    }
}
