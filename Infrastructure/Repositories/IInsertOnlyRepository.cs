// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IInsertOnlyRepository.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
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
