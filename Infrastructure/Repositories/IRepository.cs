// ==================================================
// Copyright 2016(C) , DotLogix
// File:  IRepository.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  23.06.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.Entities;
#endregion

namespace DotLogix.Architecture.Infrastructure.Repositories {
    public interface IRepository {
        
    }

    public interface IRepository<TEntity> : IRepository where TEntity : class, ISimpleEntity {
        #region Get
        Task<TEntity> GetAsync(int id);
        Task<TEntity> GetAsync(Guid guid);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> FilterAllAsync(Expression<Func<TEntity, bool>> filterExpression);
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
