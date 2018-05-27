// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IEntitySet.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  02.04.2018
// LastEdited:  02.04.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.Queries;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityContext {
    public interface IEntitySet<TEntity> where TEntity : ISimpleEntity {
        #region Query
        IQuery<TEntity> Query();
        #endregion

        #region Get
        Task<TEntity> GetAsync(int id, CancellationToken cancellationToken = default(CancellationToken));
        Task<IEnumerable<TEntity>> GetRangeAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default(CancellationToken));
        Task<TEntity> GetAsync(Guid guid, CancellationToken cancellationToken = default(CancellationToken));
        Task<IEnumerable<TEntity>> GetRangeAsync(IEnumerable<Guid> guids, CancellationToken cancellationToken = default(CancellationToken));
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));
        #endregion

        #region Add
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        #endregion

        #region Remove
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
        #endregion

    }
}
