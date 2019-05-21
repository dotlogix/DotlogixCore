// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IEntitySetProvider.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  04.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Architecture.Common.Entities;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityContext {
    public interface IEntitySetProvider {
        IEntitySet<TEntity> UseSet<TEntity>() where TEntity : class, ISimpleEntity;
    }
}
