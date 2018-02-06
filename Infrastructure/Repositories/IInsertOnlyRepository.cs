// ==================================================
// Copyright 2016(C) , DotLogix
// File:  IInsertOnlyRepository.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  18.07.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System.Collections.Generic;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.Entities.Options;
using DotLogix.Architecture.Infrastructure.Repositories.Enums;
#endregion

namespace DotLogix.Architecture.Infrastructure.Repositories {
    public interface IInsertOnlyRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, ISimpleEntity, IInsertOnly {
        Task<TEntity> GetAsync(int id, InsertOnlyFilterModes includeFilter = InsertOnlyFilterModes.Active);
        Task<IEnumerable<TEntity>> GetAllAsync(InsertOnlyFilterModes includeFilter = InsertOnlyFilterModes.Active);
    }
}
