// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IDurationRepository.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotLogix.Architecture.Common.Entities;
using DotLogix.Architecture.Common.Options;
#endregion

namespace DotLogix.Architecture.Infrastructure.Repositories {
    public interface IDurationRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, ISimpleEntity, IDuration {
        #region InRange
        Task<IEnumerable<TEntity>> InRangeAsync(DateTime fromUtc, DateTime untilUtc);
        Task<IEnumerable<TEntity>> StartedInRangeAsync(DateTime fromUtc, DateTime untilUtc);
        Task<IEnumerable<TEntity>> EndedInRangeAsync(DateTime fromUtc, DateTime untilUtc);
        #endregion

        #region Before
        Task<IEnumerable<TEntity>> StartedBeforeAsync(DateTime timeUtc);
        Task<IEnumerable<TEntity>> EndedBeforeAsync(DateTime timeUtc);
        #endregion

        #region After
        Task<IEnumerable<TEntity>> StartedAfterAsync(DateTime timeUtc);
        Task<IEnumerable<TEntity>> EndedAfterAsync(DateTime timeUtc);
        #endregion
    }
}
