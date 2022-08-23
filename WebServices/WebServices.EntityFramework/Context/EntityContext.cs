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
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Core.Caching;
using DotLogix.Core.Diagnostics;
using DotLogix.Core.Extensions;
using DotLogix.WebServices.Core;
using DotLogix.WebServices.EntityFramework.Context.Events;
using DotLogix.WebServices.EntityFramework.Database;
using DotLogix.WebServices.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
#endregion

namespace DotLogix.WebServices.EntityFramework.Context; 

/// <summary>
///     An implementation of the <see cref="IEntityContext" /> interface for entity framework
/// </summary>
[SuppressMessage("ReSharper", "EF1001")]
public class EntityContext : IEntityContext {
    private IUnitOfWork _unitOfWork;
    private ICache<object, object> _entityCache;

    /// <summary>
    /// The current unit of work
    /// </summary>
    protected IUnitOfWork UnitOfWork => _unitOfWork ??= CreateUnitOfWork();

    /// <summary>
    /// The global cache provider of this entity context
    /// </summary>
    protected ICacheProvider CacheProvider { get; }

    /// <summary>
    /// The unit of work factory
    /// </summary>
    protected IUnitOfWorkFactory UnitOfWorkFactory { get; }
        
    /// <summary>
    /// The service provider of this entity context
    /// </summary>
    protected IServiceProvider ServiceProvider { get; }
        
    /// <summary>
    /// The log source provider of this entity context
    /// </summary>
    protected ILogSourceProvider LogSourceProvider { get; }
        
    /// <summary>
    /// The current db context of this entity context
    /// </summary>
    protected DbContext DbContext => UnitOfWork.DbContext;
        
    /// <inheritdoc />
    public IEntityTypeManager TypeManager => UnitOfWork.TypeManager;
        
    /// <inheritdoc />
    public IEntityDatabaseOperations Operations => UnitOfWork.Operations;
        
    /// <inheritdoc />
    public IEntityStateManager StateManager => UnitOfWork.StateManager;
        
    /// <inheritdoc />
    public IEntityEventManager EventManager => UnitOfWork.EventManager;
        
    public ICache<object, object> EntityCache => _entityCache ??= CacheProvider.GetOrCreateEntityCache();

    public EntityContext(
        IServiceProvider serviceProvider,
        IUnitOfWorkFactory unitOfWorkFactory,
        ICacheProvider cacheProvider,
        ILogSourceProvider logSourceProvider
    ) {
        UnitOfWorkFactory = unitOfWorkFactory;
        ServiceProvider = serviceProvider;
        LogSourceProvider = logSourceProvider;
        CacheProvider = cacheProvider;
    }

    /// <inheritdoc />
    public IEntitySet<TEntity> Set<TEntity>() where TEntity : class {
        return new EntitySet<TEntity>(DbContext.Set<TEntity>());
    }

    /// <inheritdoc />
    public virtual async Task<int> CompleteAsync(CancellationToken cancellationToken = default) {
        try {
            if(_unitOfWork is null) {
                return 0;
            }
                
            var entries = new List<EntityEntry>();
            foreach(var entry in StateManager.GetEntries()) {
                entry.DetectChanges();
                if(entry.HasChanges()) {
                    entries.Add(entry);
                }
            }
                
            if(entries.Count == 0) {
                return 0;
            }
                
            foreach(var entry in entries) {
                OnEntityCommit(new EntityCommitEventArgs(entry));
            }

            var affectedEntities = await DbContext.SaveChangesAsync(cancellationToken);
                
            foreach(var entry in entries) {
                OnEntityCommitted(new EntityCommitEventArgs(entry));
            }

            return affectedEntities;
        } finally {
            ResetUnitOfWork();
        }
    }

    /// <inheritdoc />
    public virtual int Complete() {
        return CompleteAsync().GetAwaiter().GetResult();
    }

    /// <inheritdoc />
    public virtual void Reset() {
        ResetUnitOfWork();
    }

    /// <inheritdoc />
    public virtual void Dispose() {
        Reset();
    }
        

    #region StateManagement
    /// <inheritdoc />
    public void Attach(object entity) {
        StateManager.Attach(entity);
    }

    /// <inheritdoc />
    public void AttachRange(params object[] entities) {
        StateManager.Attach(entities);
    }

    /// <inheritdoc />
    public void AttachRange(IEnumerable<object> entities) {
        StateManager.Attach(entities);
    }
    /// <inheritdoc />
    public void Detach(object entity) {
        StateManager.Detach(entity);
    }

    /// <inheritdoc />
    public void DetachRange(params object[] entities) {
        StateManager.Detach(entities);
    }

    /// <inheritdoc />
    public void DetachRange(IEnumerable<object> entities) {
        StateManager.Detach(entities);
    }
        
    /// <inheritdoc />
    public void Add(object entity) {
        StateManager.MarkAdded(entity);
    }

    /// <inheritdoc />
    public void AddRange(params object[] entities) {
        StateManager.MarkAdded(entities);
    }

    /// <inheritdoc />
    public void AddRange(IEnumerable<object> entities) {
        StateManager.MarkAdded(entities);
    }

    /// <inheritdoc />
    public void Update(object entity) {
        StateManager.MarkModified(entity);
    }

    /// <inheritdoc />
    public void UpdateRange(params object[] entities) {
        StateManager.MarkModified(entities);
    }

    /// <inheritdoc />
    public void UpdateRange(IEnumerable<object> entities) {
        StateManager.MarkModified(entities);
    }

    /// <inheritdoc />
    public void Remove(object entity) {
        StateManager.MarkRemoved(entity);
    }

    /// <inheritdoc />
    public void RemoveRange(params object[] entities) {
        StateManager.MarkRemoved(entities);
    }

    /// <inheritdoc />
    public void RemoveRange(IEnumerable<object> entities) {
        StateManager.MarkRemoved(entities);
    }
    #endregion
        
    #region EventHandlers
    private void RegisterEventHooks(DbContext dbContext) {
        if(dbContext.GetStateManager() is {} stateManager) {
            stateManager.StateChanged += OnChangeTrackerOnStateChanged;
            stateManager.Tracked += OnChangeTrackerOnTracked;
        }

        if(dbContext.GetEntityQueryCompiler() is {} queryCompiler) {
            queryCompiler.QueryCompile += OnQueryCompile;
            queryCompiler.QueryExecuting += OnQueryExecuting;
            queryCompiler.QueryExecuted += OnQueryExecuted;
            queryCompiler.QueryFailed += OnQueryFailed;
        }
    }

    private void UnregisterEventHooks(DbContext dbContext) {
        if(dbContext.GetStateManager() is {} stateManager) {
            stateManager.StateChanged -= OnChangeTrackerOnStateChanged;
            stateManager.Tracked -= OnChangeTrackerOnTracked;
        }

        if(dbContext.GetEntityQueryCompiler() is {} queryCompiler) {
            queryCompiler.QueryCompile -= OnQueryCompile;
            queryCompiler.QueryExecuting -= OnQueryExecuting;
            queryCompiler.QueryExecuted -= OnQueryExecuted;
            queryCompiler.QueryFailed -= OnQueryFailed;
        }
    }

    private void OnChangeTrackerOnTracked(object sender, EntityTrackedEventArgs e) {
        OnEntityTracked(new EntityTrackEventArgs(e.Entry, e.FromQuery));
    }

    private void OnChangeTrackerOnStateChanged(object sender, EntityStateChangedEventArgs e) {
        OnEntityStateChanged(new EntityStateEventArgs(e.Entry, e.OldState, e.NewState));
    }

    /// <summary>
    ///     A callback happening if an entity is queried by the db context
    /// </summary>
    protected virtual void OnQueryCompile(object sender, QueryCompileEventArgs args) {
        var entityType = ExtractEntityType(args.ResultType);
        OnInvokeEvent(EventManager.QueryCompile, entityType, args);
    }

    /// <summary>
    ///     A callback happening if an entity is queried by the db context
    /// </summary>
    protected virtual void OnQueryExecuting(object sender, QueryResultEventArgs args) {
        var entityType = ExtractEntityType(args.ResultType);
        OnInvokeEvent(EventManager.QueryExecuting, entityType, args);
    }

    /// <summary>
    ///     A callback happening if an entity is queried by the db context
    /// </summary>
    protected virtual void OnQueryExecuted(object sender, QueryResultEventArgs args) {
        var entityType = ExtractEntityType(args.ResultType);
        OnInvokeEvent(EventManager.QueryExecuted, entityType, args);
    }

    /// <summary>
    ///     A callback happening if an entity query failed
    /// </summary>
    protected virtual void OnQueryFailed(object sender, QueryResultEventArgs args) {
        var entityType = ExtractEntityType(args.ResultType);
        OnInvokeEvent(EventManager.QueryFailed, entityType, args);
    }

    /// <summary>
    ///     A callback happening if an entity is newly tracked in the db context
    /// </summary>
    protected virtual void OnEntityTracked(EntityTrackEventArgs args) {
        var entityType = args.EntityEntry.Metadata.ClrType;
        OnInvokeEvent(EventManager.Tracked, entityType, args);
            
        var stateEventArgs = new EntityStateEventArgs(args.EntityEntry, EntityState.Detached, args.EntityEntry.State);
        OnEntityStateChanged(stateEventArgs);
    }

    /// <summary>
    ///     A callback happening each time an entity changes it's state. If an entity is newly added to the db context it is
    ///     only track-able with the <see cref="OnEntityTracked" /> method
    /// </summary>
    protected virtual void OnEntityStateChanged(EntityStateEventArgs args) {
        var entityType = args.EntityEntry.Metadata.ClrType;
        OnInvokeEvent(EventManager.StateChanged, entityType, args);
    }

    /// <summary>
    ///     A callback happening each time the user commits the store to the database.
    /// </summary>
    protected virtual void OnEntityCommit(EntityCommitEventArgs args) {
        var entityType = args.EntityEntry.Metadata.ClrType;
        OnInvokeEvent(EventManager.Commit, entityType, args);
    }

    /// <summary>
    ///     A callback happening each time the user committed the store to the database.
    /// </summary>
    protected virtual void OnEntityCommitted(EntityCommitEventArgs args) {
        var entityType = args.EntityEntry.Metadata.ClrType;
        OnInvokeEvent(EventManager.Committed, entityType, args);
    }
        
    /// <summary>
    ///     A callback happening each time an event is invoked
    /// </summary>
    protected virtual void OnInvokeEvent<TArgs>(EntityEvent<TArgs> eventHandlers, Type entityType, TArgs args) {
        eventHandlers.Trigger(entityType, this, args);
    }
        
    #endregion
        
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

        var entityType = TypeManager.GetEntityType(resultType);
        return entityType?.ClrType;
    }
        
    private IUnitOfWork CreateUnitOfWork() {
        var unitOfWork = UnitOfWorkFactory.Create();
        RegisterEventHooks(unitOfWork.DbContext);
        return unitOfWork;
    }
        
    private void ResetUnitOfWork() {
        if(_unitOfWork is null)
            return;
            
        UnregisterEventHooks(_unitOfWork.DbContext);
        _unitOfWork.Dispose();
        _unitOfWork = null;
    }
}