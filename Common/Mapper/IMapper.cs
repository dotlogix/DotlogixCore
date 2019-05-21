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

namespace DotLogix.Architecture.Common.Mapper {
    public interface IMapper<TEntity, TModel> {
        void FromEntityToModel(TEntity source, TModel target);
        void FromModelToEntity(TEntity target, TModel source);
        TModel ToModel(TEntity source);
        TDerived ToModel<TDerived>(TEntity source) where TDerived : TModel, new();
        TEntity ToEntity(TModel source);
        IEnumerable<TModel> ToModels(IEnumerable<TEntity> source);
        IEnumerable<TDerived> ToModels<TDerived>(IEnumerable<TEntity> source) where TDerived : TModel, new();
        IEnumerable<TEntity> ToEntities(IEnumerable<TModel> source);
    }
}
