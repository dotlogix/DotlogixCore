//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Threading.Tasks;
//using DotLogix.Architecture.Common.Options;
//using DotLogix.Architecture.Infrastructure.Queries;
//using DotLogix.Architecture.Infrastructure.Repositories;
//using ManageIt.Common.QueryFilter;
//using ManageIt.Infrastructure;
//using ManageIt.Infrastructure.Extensions;
//
//namespace ManageIt.Domain.Extensions {
//    public static class UnitOfWorkExtensions {
//        #region Get
//
//        /// <summary>
//        ///     Get a single entity by guid
//        /// </summary>
//        public static Task<TEntity> GetAsync<TEntity>(this IDomainContext context, Guid guid) where TEntity : class, IGuid, new() {
//            var repository = context.GetRequiredService<IRepository<Guid, TEntity>>();
//            return repository.GetAsync(guid);
//        }
//
//        /// <summary>
//        ///     Get all entities
//        /// </summary>
//        public static Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(this IDomainContext context) where TEntity : class, IGuid, new() {
//            var repository = context.GetRequiredService<IRepository<Guid, TEntity>>();
//            return repository.GetAllAsync();
//        }
//
//        /// <summary>
//        ///     Get a range of entities by guid
//        /// </summary>
//        public static Task<IEnumerable<TEntity>> GetRangeAsync<TEntity>(this IDomainContext context, IEnumerable<Guid> guids) where TEntity : class, IGuid, new() {
//            var repository = context.GetRequiredService<IRepository<Guid, TEntity>>();
//            return repository.GetRangeAsync(guids);
//        }
//
//        /// <summary>
//        ///     Get all entities matching an expression
//        /// </summary>
//        public static Task<IEnumerable<TEntity>> WhereAsync<TEntity>(this IDomainContext context, Expression<Func<TEntity, bool>> filterExpression) where TEntity : class, IGuid, new() {
//            var repository = context.GetRequiredService<IRepository<Guid, TEntity>>();
//            return repository.WhereAsync(filterExpression);
//        }
//
//        /// <summary>
//        ///     Get all entities matching an expression
//        /// </summary>
//        public static Task<IEnumerable<TEntity>> WhereAsync<TEntity>(this IDomainContext context, IQueryModifier<TEntity> filter) where TEntity : class, IGuid, new() {
//            var repository = context.GetRequiredService<IEfRepository<Guid, TEntity>>();
//            return repository.WhereAsync(filter);
//        }
//        
//        /// <summary>
//        ///     Get paged entities
//        /// </summary>
//        public static Task<PaginationResult<TEntity>> GetPagedAsync<TEntity>(this IDomainContext context, PaginationFilter filter) where TEntity : class, IGuid, new() {
//            var repository = context.GetRequiredService<IEfRepository<Guid, TEntity>>();
//            return repository.GetPagedAsync(filter);
//        }
//        
//        /// <summary>
//        ///     Get paged entities matching an expression
//        /// </summary>
//        public static Task<PaginationResult<TEntity>> GetPagedAsync<TEntity>(this IDomainContext context, IQueryModifier<TEntity> filter, PaginationFilter paginationFilter) where TEntity : class, IGuid, new() {
//            var repository = context.GetRequiredService<IEfRepository<Guid, TEntity>>();
//            return repository.GetPagedAsync(filter, paginationFilter);
//        }
//
//        #endregion
//
//        #region Add
//
//        /// <summary>
//        ///     Add a single entity to the set
//        /// </summary>
//        public static Task<TEntity> AddAsync<TEntity>(this IDomainContext context, TEntity entity) where TEntity : class, IGuid, new() {
//            var repository = context.GetRequiredService<IRepository<Guid, TEntity>>();
//            return repository.AddAsync(entity);
//        }
//        
//        /// <summary>
//        ///     Add a range of entities to the set
//        /// </summary>
//        public static Task<IEnumerable<TEntity>> AddRangeAsync<TEntity>(this IDomainContext context, IEnumerable<TEntity> entities) where TEntity : class, IGuid, new() {
//            var repository = context.GetRequiredService<IRepository<Guid, TEntity>>();
//            return repository.AddRangeAsync(entities);
//        }
//
//        #endregion
//
//
//        #region Remove
//
//        /// <summary>
//        ///     Queries the matching entity and remove it
//        /// </summary>
//        public static async Task<bool> RemoveAsync<TEntity>(this IDomainContext context, Guid guid) where TEntity : class, IGuid, new() {
//            var repository = context.GetRequiredService<IRepository<Guid, TEntity>>();
//            var entity = await repository.GetAsync(guid);
//            if (entity == null)
//                return false;
//
//            await repository.RemoveAsync(entity);
//            return true;
//        }
//
//        /// <summary>
//        ///     Queries the matching entities and remove them
//        /// </summary>
//        public static async Task RemoveWhereAsync<TEntity>(this IDomainContext context, Expression<Func<TEntity, bool>> filterExpression) where TEntity : class, IGuid, new() {
//            var repository = context.GetRequiredService<IRepository<Guid, TEntity>>();
//            var entities = await repository.WhereAsync(filterExpression);
//            if (entities == null)
//                return;
//
//            await repository.RemoveRangeAsync(entities);
//        }
//
//        /// <summary>
//        ///     Queries the matching entity and remove it
//        /// </summary>
//        public static Task RemoveRangeAsync<TEntity>(this IDomainContext context, IEnumerable<Guid> guids) where TEntity : class, IGuid, new() {
//            return RemoveWhereAsync<TEntity>(context, e => guids.Contains(e.Guid));
//        }
//
//        #endregion
//    }
//}


