// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IInsertOnlyRepository.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  19.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Architecture.Common.Entities;
using DotLogix.Architecture.Common.Options;
#endregion

namespace DotLogix.Architecture.Infrastructure.Repositories {
    public interface IInsertOnlyRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, ISimpleEntity, IInsertOnly { }
}
