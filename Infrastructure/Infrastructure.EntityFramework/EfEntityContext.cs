// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EfEntityContext.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  19.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using DotLogix.Core.Diagnostics;
using DotLogix.Core.Extensions;
using DotLogix.Infrastructure.EntityFramework.Hooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.Internal;
#endregion

namespace DotLogix.Infrastructure.EntityFramework {
    /// <summary>
    ///     An implementation of the <see cref="IEntityContext" /> interface for entity framework
    /// </summary>
    [SuppressMessage("ReSharper", "EF1001")]
    public class EfEntityContext<TDbContext> : IEfEntityContext where TDbContext : DbContext {
        private TDbContext _dbContext;
        /// <summary>
        /// The current DbContextFactory
        /// </summary>
        protected IDbContextFactory<TDbContext> DbContextFactory { get; }
        private Dictionary<Type, object> EntitySetDict { get; } = new Dictionary<Type, object>();
        
        /// <inheritdoc />
        public ILogSource LogSource { get; }

        /// <inheritdoc />
        public ILogSourceProvider LogSourceProvider { get; }

        /// <inheritdoc />
        DbContext IEfEntityContext.DbContext => DbContext;

        /// <inheritdoc cref="IEfEntityContext.DbContext" />
        public TDbContext DbContext => _dbContext ??= CreateDbContext();

        /// <inheritdoc />
        public IEfEventManager EventManager { get; protected set; }

        /// <summary>
        ///     Create a new instance of <see cref="EfEntityContext{TDbContext}" />
        /// </summary>
        public EfEntityContext(IDbContextFactory<TDbContext> dbContextFactory, ILogSourceProvider logSourceProvider) {
            DbContextFactory = dbContextFactory;
            LogSourceProvider = logSourceProvider;
            LogSource = logSourceProvider.Create(GetType().FullName);
            EventManager = new EfEventManager();
        }


        /// <inheritdoc />
        public virtual IEntitySet<TEntity> GetEntitySet<TEntity>() where TEntity : class, new() {
            return (IEntitySet<TEntity>)EntitySetDict.GetOrAdd(typeof(TEntity), OnCreateEntitySet<TEntity>);
        }

        protected virtual IEntitySet<TEntity> OnCreateEntitySet<TEntity>(Type entityType) where TEntity : class, new() {
            return new EfEntitySet<TEntity>(this);
        }

        #region EventHandlers

        /// <inheritdoc />
        public virtual async Task<int> CompleteAsync() {
            try {
                var entries = DbContext.ChangeTracker.Entries().ToList();
                foreach(var entry in entries) {
                    OnEntityCommit(new EntityCommitEventArgs(entry));
                }

                var affectedEntities = await DbContext.SaveChangesAsync();
                
                foreach(var entry in entries) {
                    OnEntityCommitted(new EntityCommitEventArgs(entry));
                }

                return affectedEntities;
            } finally {
                RenewDbContext();
            }
        }

        /// <summary>
        ///     A callback happening if an entity is queried by the db context
        /// </summary>
        protected virtual void OnQueryCompile(object sender, QueryCompileEventArgs e) {
            var entityType = ExtractEntityType(e.ResultType);
            OnInvokeEvent(entityType, e, EventManager.QueryCompile);
        }

        /// <summary>
        ///     A callback happening if an entity is queried by the db context
        /// </summary>
        protected virtual void OnQueryExecuting(object sender, QueryResultEventArgs e) {
            var entityType = ExtractEntityType(e.ResultType);
            OnInvokeEvent(entityType, e, EventManager.QueryExecuting);
        }

        /// <summary>
        ///     A callback happening if an entity is queried by the db context
        /// </summary>
        protected virtual void OnQueryExecuted(object sender, QueryResultEventArgs e) {
            var entityType = ExtractEntityType(e.ResultType);
            OnInvokeEvent(entityType, e, EventManager.QueryExecuted);
        }

        /// <summary>
        ///     A callback happening if an entity query failed
        /// </summary>
        protected virtual void OnQueryFailed(object sender, QueryResultEventArgs e) {
            var entityType = ExtractEntityType(e.ResultType);
            OnInvokeEvent(entityType, e, EventManager.QueryFailed);
        }

        /// <summary>
        ///     A callback happening if an entity is newly tracked in the db context
        /// </summary>
        protected virtual void OnEntityTracked(EntityTrackEventArgs e) {
            var entityType = e.EntityEntry.Metadata.ClrType;
            OnInvokeEvent(entityType, e, EventManager.Tracked);
            
            var stateEventArgs = new EntityStateEventArgs(e.EntityEntry, EntityState.Detached, e.EntityEntry.State);
            OnEntityStateChanged(stateEventArgs);
        }

        /// <summary>
        ///     A callback happening each time an entity changes it's state. If an entity is newly added to the db context it is
        ///     only track-able with the <see cref="OnEntityTracked" /> method
        /// </summary>
        protected virtual void OnEntityStateChanged(EntityStateEventArgs e) {
            var entityType = e.EntityEntry.Metadata.ClrType;
            OnInvokeEvent(entityType, e, EventManager.StateChanged);
        }

        /// <summary>
        ///     A callback happening each time the user commits the store to the database.
        /// </summary>
        protected virtual void OnEntityCommit(EntityCommitEventArgs e) {
            var entityType = e.EntityEntry.Metadata.ClrType;
            OnInvokeEvent(entityType, e, EventManager.Commit);
        }

        /// <summary>
        ///     A callback happening each time the user committed the store to the database.
        /// </summary>
        protected virtual void OnEntityCommitted(EntityCommitEventArgs e) {
            var entityType = e.EntityEntry.Metadata.ClrType;
            OnInvokeEvent(entityType, e, EventManager.Committed);
        }
        
        /// <summary>
        ///     A callback happening each time an event is invoked
        /// </summary>
        protected virtual void OnInvokeEvent<TArgs>(Type entityType, TArgs args, EventHandlerCollection<TArgs> eventHandlers) {
            if(eventHandlers.IsEmpty) {
                return;
            }
            
            var handlers = entityType == null
                               ? eventHandlers.GetGlobalHandlers()
                               : eventHandlers.GetHandlers(entityType);

            if(handlers == null) {
                return;
            }

            foreach(var handler in handlers) {
                handler.Invoke(this, args);
            }
        }

        #endregion
        
        
        /// <inheritdoc cref="IDisposable.Dispose" />
        protected virtual void Dispose(bool disposing) {
            if(disposing) {
                _dbContext?.Dispose();
            }
        }

        /// <inheritdoc />
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        
        protected void RenewDbContext() {
            _dbContext?.Dispose();
            _dbContext = null;
        }

        protected virtual TDbContext CreateDbContext() {
            var dbContext = DbContextFactory.CreateDbContext();
            dbContext.ChangeTracker.StateChanged += (sender, e) => OnEntityStateChanged(new EntityStateEventArgs(e.Entry, e.OldState, e.NewState));
            dbContext.ChangeTracker.Tracked += (sender, e) => OnEntityTracked(new EntityTrackEventArgs(e.Entry, e.FromQuery));

            if(dbContext.GetService<IQueryCompiler>() is IEfQueryCompiler queryCompiler) {
                queryCompiler.QueryCompile += OnQueryCompile;
                queryCompiler.QueryExecuting += OnQueryExecuting;
                queryCompiler.QueryExecuted += OnQueryExecuted;
                queryCompiler.QueryFailed += OnQueryFailed;
            }

            return dbContext;
        }
        
        protected virtual Type ExtractEntityType(Type resultType) {
            if(resultType.IsAssignableToGeneric(typeof(IReadOnlyDictionary<,>), out var parameters)) {
                resultType = parameters[1];
            } else if(resultType.IsAssignableToGeneric(typeof(IDictionary<,>), out parameters)) {
                resultType = parameters[1];
            } else if(resultType.IsAssignableToGeneric(typeof(ILookup<,>), out parameters)) {
                resultType = parameters[1];
            } else if(resultType.IsAssignableToGeneric(typeof(IEnumerable<>), out parameters)) {
                resultType = parameters[0];
            }

            var entityType = DbContext.Model.FindEntityType(resultType);
            return entityType?.ClrType;
        }

    }
}
