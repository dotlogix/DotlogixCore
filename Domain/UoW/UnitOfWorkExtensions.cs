// ==================================================
// Copyright 2018(C) , DotLogix
// File:  UnitOfWorkExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  09.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.Repositories;
#endregion

namespace DotLogix.Architecture.Domain.UoW {
    public static class UnitOfWorkExtensions {
        #region Get

        public static Task<TEntity> GetAsync<TEntity, TRepo>(this IUnitOfWorkContext context, Guid guid) where TRepo : IRepository<TEntity> where TEntity : class, ISimpleEntity {
            var repository = context.UseRepository<TRepo>();
            return repository.GetAsync(guid);
        }

        public static Task<IEnumerable<TEntity>> GetAllAsync<TEntity, TRepo>(this IUnitOfWorkContext context) where TRepo : IRepository<TEntity> where TEntity : class, ISimpleEntity {
            var repository = context.UseRepository<TRepo>();
            return repository.GetAllAsync();
        }

        public static Task<IEnumerable<TEntity>> FilterAllAsync<TEntity, TRepo>(this IUnitOfWorkContext context, Expression<Func<TEntity, bool>> filterExpression) where TRepo : IRepository<TEntity> where TEntity : class, ISimpleEntity {
            var repository = context.UseRepository<TRepo>();
            return repository.FilterAllAsync(filterExpression);
        }

        public static Task<IEnumerable<TEntity>> GetRangeAsync<TEntity, TRepo>(this IUnitOfWorkContext context, IEnumerable<Guid> guids) where TRepo : IRepository<TEntity> where TEntity : class, ISimpleEntity {
            var repository = context.UseRepository<TRepo>();
            return repository.GetRangeAsync(guids);
        }

        #endregion

        #region Remove

        public static void Add<TEntity, TRepo>(this IUnitOfWorkContext context, TEntity entity) where TRepo : IRepository<TEntity> where TEntity : class, ISimpleEntity
        {
            var repository = context.UseRepository<TRepo>();
            repository.Add(entity);
            
        }

        public static void AddRange<TEntity, TRepo>(this IUnitOfWorkContext context, IEnumerable<TEntity> entities) where TRepo : IRepository<TEntity> where TEntity : class, ISimpleEntity
        {
            var repository = context.UseRepository<TRepo>();
            repository.AddRange(entities);
        }

        #endregion


        #region Remove

        public static async Task RemoveAsync<TEntity, TRepo>(this IUnitOfWorkContext context, Guid guid) where TRepo : IRepository<TEntity> where TEntity : class, ISimpleEntity
        {
            var repository = context.UseRepository<TRepo>();
            var entity = await repository.GetAsync(guid);
            if (entity == null)
                return;

            repository.Remove(entity);
        }

        public static async Task RemoveWhereAsync<TEntity, TRepo>(this IUnitOfWorkContext context, Expression<Func<TEntity, bool>> filterExpression) where TRepo : IRepository<TEntity> where TEntity : class, ISimpleEntity
        {
            var repository = context.UseRepository<TRepo>();
            var entities = await repository.FilterAllAsync(filterExpression);
            if (entities == null)
                return;

            repository.RemoveRange(entities);
        }

        public static Task RemoveRangeAsync<TEntity, TRepo>(this IUnitOfWorkContext context, IEnumerable<Guid> guids) where TRepo : IRepository<TEntity> where TEntity : class, ISimpleEntity
        {
            return RemoveWhereAsync<TEntity, TRepo>(context, e => guids.Contains(e.Guid));
        }

        #endregion
    }
}
