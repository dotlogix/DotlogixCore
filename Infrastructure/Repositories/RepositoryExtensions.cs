﻿using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.Entities;

namespace DotLogix.Architecture.Infrastructure.Repositories {
    public static class RepositoryExtensions {
        public static async Task RemoveByFilterAsync<TEntity>(this IRepository<TEntity> repository, Expression<Func<TEntity, bool>> filterExpression) where TEntity : class, ISimpleEntity {
            var entities = await repository.FilterAllAsync(filterExpression);
            repository.RemoveRange(entities);
        }

        public static async Task RemoveByIdAsync<TEntity>(this IRepository<TEntity> repository, int id) where TEntity : class, ISimpleEntity
        {
            var entity = await repository.GetAsync(id);
            if (entity != null)
                repository.Remove(entity);
        }

        public static async Task RemoveByGuidAsync<TEntity>(this IRepository<TEntity> repository, Guid guid) where TEntity : class, ISimpleEntity
        {
            var entity = await repository.GetAsync(guid);
            if (entity != null)
                repository.Remove(entity);
        }
    }
}