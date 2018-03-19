using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.Repositories;

namespace DotLogix.Architecture.Domain.UoW
{
    public static class UnitOfWorkExtensions
    {
        public static Task<TEntity> GetAsync<TEntity, TRepo>(this IUnitOfWorkContext context, Guid guid) where TRepo:IRepository<TEntity> where TEntity : class, ISimpleEntity {
            var repository = context.UseRepository<TRepo>();
            return repository.GetAsync(guid);
        }

        public static Task<IEnumerable<TEntity>> GetAllAsync<TEntity, TRepo>(this IUnitOfWorkContext context) where TRepo : IRepository<TEntity> where TEntity : class, ISimpleEntity
        {
            var repository = context.UseRepository<TRepo>();
            return repository.GetAllAsync();
        }

        public static Task<IEnumerable<TEntity>> FilterAllAsync<TEntity, TRepo>(this IUnitOfWorkContext context, Expression<Func<TEntity, bool>> filterExpression) where TRepo : IRepository<TEntity> where TEntity : class, ISimpleEntity
        {
            var repository = context.UseRepository<TRepo>();
            return repository.FilterAllAsync(filterExpression);
        }
    }
}
